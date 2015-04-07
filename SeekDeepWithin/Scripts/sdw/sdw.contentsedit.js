$(document).ready(function () {
   $('#hideSaved').hide();
   $('#contentsSaved').hide();
   var topNavHeight = $('#siteNavDiv').height();
   $('#contentsNavBar').stick_in_parent({ offset_top: topNavHeight });

   $('#hideSwitch').change(function () {
      var ref = $('#contentTree').jstree(true);
      var sel = ref.get_selected();
      if (!sel.length) { return; }
      sel = sel[0];
      if (sel === 'root') { return; }
      var node = ref.get_node(sel);
      var type = ref.get_type(node.id);
      node.data.hide = !$(this).is(":checked");

      var form = $('#__AjaxAntiForgeryForm');
      var token = $('input[name="__RequestVerificationToken"]', form).val();
      $.ajax({
         type: 'POST',
         url: '/' + type + '/Hide/',
         data: {
            __RequestVerificationToken: token,
            hide: node.data.hide,
            id: node.data.id
         }
      }).done(function () {
         $('#hideSaved').show(200, function () {
            setTimeout(function () { $('#hideSaved').hide(100); }, 2000);
         });
         createToc();
      }).fail(function (d) {
         alert(d.responseText);
      });
   });

   $('#contentTree').on("rename_node.jstree", function (e, data) {
      // Send added item to server...
      var ref = data.instance;
      var node = data.node;
      var type = ref.get_type(node.id);
      var name = data.text.replace(/^\s+|\s+$/g, '');
      var form = $('#__AjaxAntiForgeryForm');
      var token = $('input[name="__RequestVerificationToken"]', form).val();
      if (node.data == null) {
         var parentId = $('#versionId').val();
         if (type === 'chapter') {
            var parent = ref.get_parent(node.id);
            var parentNode = ref.get_node(parent);
            parentId = parentNode.data.id;
         }
         $.ajax({
            type: 'POST',
            url: '/' + type + '/Create/',
            data: {
               __RequestVerificationToken: token,
               name: name,
               versionId: parentId,
               subBookId: parentId,
               bookId: $('#bookId').val()
            }
         }).done(function(d) {
            node.data = { id: d.id, hide: false, itemId: d.itemId };
            createToc();
         }).fail(function(d) {
            alert(d.responseText);
         });
      } else {
         $.ajax({
            type: 'POST',
            url: '/' + type + '/Rename/',
            data: {
               __RequestVerificationToken: token,
               name: name,
               id: node.data.id
            }
         }).done(function () {
            createToc();
         }).fail(function (d) {
            alert(d.responseText);
         });
      }
   }).on("select_node.jstree", function (e, data) {
      var node = data.node;
      if (data.node.id === 'root') {
         $('#hideSwitch').prop("checked", true);
         return;
      }
      $('#hideSwitch').prop("checked", !node.data.hide);
   }).on("move_node.jstree", function () {
      createToc();
   }).on("copy_node.jstree", function () {
      createToc();
   }).jstree({
      'core': {
         'animation': 0,
         "multiple": false,
         'check_callback': true,
         'themes': {
            'responsive': true
         }
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
   $('#modalClose').hide();
   $('#modalText').text('Saving Contents, please wait...');
   $('#modal').foundation('reveal', 'open');
   var ref = $('#contentTree').jstree(true);
   var json = ref.get_json()[0];
   var contents = new Array(json.children.length);
   for (var i = 0; i < json.children.length; i++) {
      var subBook = json.children[i];
      var subBookJson = { name: subBook.text.replace(/^\s+|\s+$/g, ''), id: subBook.data.id, itemId: subBook.data.itemId };
      if (subBook.data.hide === true || subBook.data.hide === 'True') {
         subBookJson.hide = true;
      }
      subBookJson.chapters = new Array(subBook.children.length);
      for (var j = 0; j < subBook.children.length; j++) {
         var chapter = subBook.children[j];
         subBookJson.chapters[j] = { name: chapter.text.replace(/^\s+|\s+$/g, ''), id: chapter.data.id, itemId: chapter.data.itemId };
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
   }).done(function () {
      $('#modalText').text('Contents Saved!');
      setTimeout(function () { $('#modal').foundation('reveal', 'close'); }, 700);
   }).fail(function (d) {
      $('#modalClose').show();
      $('#modalText').text('An error occured - ' + d.responseText);
   });
}

function collapseAll() {
   var ref = $('#contentTree').jstree(true);
   var root = ref.get_node('root');
   ref.close_all(root);
}

function createSubBook() {
   var ref = $('#contentTree').jstree(true);
   var root = ref.get_node('root');
   var sel = ref.create_node(root, { 'type': 'subbook', 'text': 'new' });
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

   var type = ref.get_type(sel);
   var node = ref.get_node(sel);
   if (type === 'chapter') {
      window.location.href = '/Chapter/Edit/' + node.data.id;
   } else if (type === 'subbook') {
      window.location.href = '/SubBook/Edit/' + node.data.id;
   }
}

function renameItem() {
   var ref = $('#contentTree').jstree(true);
   var sel = ref.get_selected();
   if (!sel.length) { return; }
   sel = sel[0];
   if (sel === 'root') { return; }
   var type = ref.get_type(sel);
   $('#renameItemType').text(type);
   $('#renameModal').foundation('reveal', 'open');
}

function okToRename() {
   var ref = $('#contentTree').jstree(true);
   var sel = ref.get_selected();
   if (!sel.length) { return; }
   sel = sel[0];
   if (sel === 'root') { return; }
   sel = ref.get_node(sel);
   $('#renameModal').foundation('reveal', 'close');
   if (sel) {
      ref.edit(sel);
   }
}

function setDefaultChapter() {
   var ref = $('#contentTree').jstree(true);
   var sel = ref.get_selected();
   if (!sel.length) { return; }
   sel = sel[0];
   if (sel === 'root') { return; }

   var type = ref.get_type(sel);
   var node = ref.get_node(sel);
   if (type === 'chapter') {
      var form = $('#__AjaxAntiForgeryForm');
      var token = $('input[name="__RequestVerificationToken"]', form).val();
      $.ajax({
         type: 'POST',
         url: '/Version/DefaultReadChapter/',
         data: {
            __RequestVerificationToken: token,
            id: $('#versionId').val(),
            chapterId: node.data.id
         }
      }).fail(function (d) {
         alert(d.responseText);
      });
   } else {
      alert('Only chapters can be set as default to read.');
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

function okToDelete() {
   var ref = $('#contentTree').jstree(true);
   var sel = ref.get_selected();
   if (!sel.length) { return; }
   sel = sel[0];
   if (sel === 'root') { return; }
   var node = ref.get_node(sel);
   ref.delete_node(node);
   createToc();
   $('#deleteModal').foundation('reveal', 'close');
}

function showAddList() {
   var ref = $('#contentTree').jstree(true);
   var sel = ref.get_selected();
   if (!sel.length) { return; }
   sel = sel[0];
   var type = ref.get_type(sel);
   if (type == 'chapter') {
      alert('You cannot add a new list of items to a chapter.');
      return;
   }
   $('#addListModal').foundation('reveal', 'open');
}

function createList() {
   var ref = $('#contentTree').jstree(true);
   var sel = ref.get_selected();
   if (!sel.length) { return; }
   sel = sel[0];
   var type = ref.get_type(sel);

   var node = ref.get_node(sel);
   var items = $('#addListText').val();
   var itemSplit = items.split(/\n/g);
   $('#addListModal').foundation('reveal', 'close');
   createContents(type, itemSplit, node, ref);
}

function createListRange() {
   var ref = $('#contentTree').jstree(true);
   var sel = ref.get_selected();
   if (!sel.length) { return; }
   sel = sel[0];
   var type = ref.get_type(sel);

   var node = ref.get_node(sel);
   var start = $('#addRangeStart').val();
   var end = $('#addRangeEnd').val();
   var prefix = $('#addRangePrefix').val();
   var append = $('#addRangeAppend').val();
   var itemSplit = [];
   for (var a = start; a <= end; a++) {
      itemSplit[itemSplit.length] = prefix + a + append;
   }
   $('#addListModal').foundation('reveal', 'close');
   createContents(type, itemSplit, node, ref);
}

function createContents(type, items, parent, ref) {
   if (items.length > 0) {
      var newItem = items[0].replace(/^\s+|\s+$/g, '');
      if (newItem != '') {
         var newType;
         var nodeId;
         var parentId = $('#versionId').val();
         if (type == 'subbook') {
            newType = 'Chapter';
            nodeId = ref.create_node(parent, { 'type': 'chapter', 'text': newItem });
            parentId = parent.data.id;
         } else {
            newType = 'Subbook';
            nodeId = ref.create_node(parent, { 'type': 'subbook', 'text': newItem });
         }
         var form = $('#__AjaxAntiForgeryForm');
         var token = $('input[name="__RequestVerificationToken"]', form).val();
         $.ajax({
            type: 'POST',
            url: '/' + newType + '/Create/',
            data: {
               __RequestVerificationToken: token,
               name: newItem,
               versionId: parentId,
               subBookId: parentId,
               bookId: $('#bookId').val()
            }
         }).done(function (d) {
            var node = ref.get_node(nodeId);
            node.data = { id: d.id, hide: false, itemId: d.itemId };
            items.splice(0, 1);
            createContents(type, items, parent, ref);
         }).fail(function (d) {
            alert(d.responseText);
         });
      }
   } else {
      $('#addListText').val('');
      $('#addRangeStart').val('');
      $('#addRangeEnd').val('');
      createToc();
   }
}