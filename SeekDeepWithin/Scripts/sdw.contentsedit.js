$(document).ready(function () {
   $('#contentTree').on("rename_node.jstree", function (e, data) {
      // Send added item to server...
      var ref = data.instance;
      var node = data.node;
      var type = ref.get_type(node.id);
      var name = data.text.replace(/^\s+|\s+$/g, '');
      var parentId = $('#versionId').val();
      if (type === 'chapter') {
         var parent = ref.get_parent(node.id);
         var parentNode = ref.get_node(parent);
         parentId = parentNode.data.id;
      }
      var form = $('#__AjaxAntiForgeryForm');
      var token = $('input[name="__RequestVerificationToken"]', form).val();
      $.ajax({
         type: 'POST',
         url: '/' + type + '/Create/',
         data: {
            __RequestVerificationToken: token,
            name: name,
            versionId: parentId,
            subBookId: parentId
         }
      }).done(function (d) {
         node.data = { id: d.id, hide: false, show: 'verse' };
         createToc();
      }).fail(function (d) {
         alert(d.responseText);
      });
   }).on("move_node.jstree", function () {
      createToc();
   }).on("copy_node.jstree", function () {
      createToc();
   }).jstree({
      'core': {
         'animation': 0,
         "multiple": false,
         'check_callback': true
      },
      'types': {
         '#': {
            'max_depth': 4,
            'valid_children': ['version']
         },
         'version': {
            'icon': 'icon-book',
            'valid_children': ['subbook']
         },
         'subbook': {
            'icon': 'icon-bookalt',
            'valid_children': ['chapter']
         },
         'chapter': {
            'icon': 'icon-bookmark',
            'valid_children': []
         }
      },
      'plugins': [
        'dnd', 'state', 'types', 'wholerow'
      ]
   });
});

function createToc() {
   var ref = $('#contentTree').jstree(true);
   var json = ref.get_json()[0];
   var contents = new Array(json.children.length);
   for (var i = 0; i < json.children.length; i++) {
      var subBook = json.children[i];
      var subBookJson = { name: subBook.text.replace(/^\s+|\s+$/g, ''), id: subBook.data.id };
      if (subBook.data.hide === true || subBook.data.hide === 'True') {
         subBookJson.hide = true;
      }
      subBookJson.chapters = new Array(subBook.children.length);
      for (var j = 0; j < subBook.children.length; j++) {
         var chapter = subBook.children[j];
         subBookJson.chapters[j] = { name: chapter.text.replace(/^\s+|\s+$/g, ''), id: chapter.data.id };
         if (chapter.data.hide === true || chapter.data.hide === 'True') {
            subBookJson.chapters[j].hide = true;
         }
      }
      contents[i] = subBookJson;
   }

   var form = $('#__AjaxAntiForgeryForm');
   var token = $('input[name="__RequestVerificationToken"]', form).val();
   $.ajax({
      type: 'POST',
      url: '/Version/UpdateContents/',
      data: {
         __RequestVerificationToken: token,
         id: $('#versionId').val(),
         contents: JSON.stringify(contents)
      }
   }).fail(function (d) {
      alert(d.responseText);
   });
}

function createSubBook() {
   var ref = $('#contentTree').jstree(true);
   var root = ref.get_node('root');
   var sel = ref.create_node(root, { 'type': 'subbook', 'text': 'default' });
   if (sel) {
      ref.edit(sel);
   }
}

function createChapter() {
   var ref = $('#contentTree').jstree(true);
   var sel = ref.get_selected();
   if (!sel.length) { return; }
   sel = sel[0];
   if (ref.get_type(sel) == 'chapter')
      sel = ref.get_parent(sel);
   sel = ref.create_node(sel, { 'type': 'chapter' });
   if (sel) {
      ref.edit(sel);
   }
}

function editItem() {
   var ref = $('#contentTree').jstree(true);
   var sel = ref.get_selected();
   if (!sel.length) { return; }
   sel = sel[0];
   if (sel === 'root') { return; }

   var node = ref.get_node(sel);
   $('#editNodeId').val(sel);
   $('#editNodeName').val(ref.get_text(sel).replace(/^\s+|\s+$/g, ''));
   var type = ref.get_type(sel);
   if (type === 'subbook') {
      $('#showColumn').hide();
   } else {
      $('#showColumn').show();
      $('#show' + node.data.show).prop('selected', true);
   }
   var h = node.data.hide === true || node.data.hide === 'True' ? 'hide' : 'show';
   $('#hide' + h).prop('selected', true);
   $('#modal').foundation('reveal', 'open');
}

function nodeEditDone() {
   // Save item to server...
   var ref = $('#contentTree').jstree(true);
   var nodeId = $('#editNodeId').val();
   var node = ref.get_node(nodeId);
   var type = ref.get_type(nodeId);
   var name = ref.get_text(nodeId).replace(/^\s+|\s+$/g, '');
   var hide = node.data.hide;
   var show = node.data.show;
   var newName = $('#editNodeName').val();
   var newHide = $("#editNodeHide option:selected").attr('id').substr(4) === 'hide';
   var newShow = $("#editNodeShow option:selected").attr('id').substr(4);
   node.data.hide = newHide;
   node.data.show = newShow;
   ref.set_text(nodeId, newName);
   if (name != newName || (show !== undefined && show != newShow)) {
      var itemId = node.data.id;
      var form = $('#__AjaxAntiForgeryForm');
      var token = $('input[name="__RequestVerificationToken"]', form).val();
      $.ajax({
         type: 'POST',
         url: '/' + type + '/Edit/',
         data: {
            __RequestVerificationToken: token,
            name: newName,
            Id: itemId,
            DefaultToParagraph: newShow === 'para'
         }
      }).done(function () {
         $('#modal').foundation('reveal', 'close');
         createToc();
      }).fail(function (data) {
         alert(data.responseText);
      });
   } else if (hide !== newHide) {
      $('#modal').foundation('reveal', 'close');
      createToc();
   }
}

function deleteItem() {
   var ref = $('#contentTree').jstree(true);
   var sel = ref.get_selected();
   if (!sel.length) { return; }
   sel = sel[0];
   if (sel === 'root') { return; }
   var type = ref.get_type(sel);
   $('#itemType').text(type);
   $('#deleteModal').foundation('reveal', 'open');
}