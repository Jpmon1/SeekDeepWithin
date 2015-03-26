
$(document).ready(function() {
   $('#rowUpdate').hide();
   $('#createStyleCheck').hide();
   $('#updateStyleCheck').hide();
});

function style_create() {
   style_post('create', function (d) {
      $('#styleStart').val('');
      $('#styleEnd').val('');
      style_render();
      $('#createStyleCheck').show(200, function () {
         setTimeout(function () { $('#createStyleCheck').hide(100); }, 2000);
      });
      $('#styleList').append('<li id="listItem_' + d.id + '" class="bullet-item"><a href="javascript:void(0)" onclick="style_edit(' + d.id +
         ')">Start: ' + d.startIndex + ' End: ' + d.endIndex + '</a></li>');
   });
}

function style_update() {
   style_post('update', function() {
      $('#updateStyleCheck').show(200, function() {
         setTimeout(function () { $('#updateStyleCheck').hide(100); }, 2000);
      });
      style_render();
      $('#listItem_' + $('#editId').val()).remove();
      $('#styleList').append('<li class="bullet-item" id="listItem_' + $('#editId').val() +
         '"><a href="javascript:void(0)" onclick="style_edit(' + $('#editId').val() +
         ')">Start: ' + $('#startIndex').val() + ' (' + escapeHtml($('#styleStart').val()) + ') End: ' +
         $('#endIndex').val() + ' (' + escapeHtml($('#styleEnd').val()) + ')</li>');
      $('#styleStart').val('');
      $('#styleEnd').val('');
   });
}

function escapeHtml(str) {
   var div = document.createElement('div');
   div.appendChild(document.createTextNode(str));
   return div.innerHTML;
};

function style_post(action, done) {
   var startIndex = $('#startIndex').val();
   var endIndex = $('#endIndex').val();
   if (startIndex != '' && endIndex != '') {
      var type = $('#itemType').val();
      var form = $('#__AjaxAntiForgeryForm');
      var token = $('input[name="__RequestVerificationToken"]', form).val();
      $.ajax({
         type: 'POST',
         url: '/Style/' + action + type + '/',
         data: {
            __RequestVerificationToken: token,
            startIndex: startIndex,
            endIndex: endIndex,
            id: $('#editId').val(),
            spansMultiple: $('#multiSpan').prop('checked'),
            startStyle: encodeURIComponent($('#styleStart').val()),
            endStyle: encodeURIComponent($('#styleEnd').val()),
            parentId: $('#itemId').val()
         }
      }).done(done).fail(function (data) {
         alert(data.responseText);
      });
   } else {
      alert('Please select where in the text you would like the style.');
   }
}

function style_delete() {
   var type = $('#itemType').val();
   var form = $('#__AjaxAntiForgeryForm');
   var token = $('input[name="__RequestVerificationToken"]', form).val();
   $.ajax({
      type: 'POST',
      url: '/Style/Delete' + type + '/',
      data: {
         __RequestVerificationToken: token,
         id: $('#editId').val(),
         itemId: $('#itemId').val()
      }
   }).done(function () {
      var id = $('#editId').val();
      $('#listItem_' + id).remove();
      style_new();
      style_render();
   }).fail(function (data) {
      alert(data.responseText);
   });
}

function style_edit(itemId) {
   $.ajax({
      url: '/Style/Get/',
      data: {
         itemId: $('#itemId').val(),
         id: itemId,
         itemType: $('#itemType').val()
      }
   }).done(function (data) {
      $('#rowCreate').hide();
      $('#rowUpdate').show();
      $('#editId').val(itemId);
      $('#styleStart').val(data.start);
      $('#styleEnd').val(data.end);
      select_word(data.startIndex, data.endIndex);
   }).fail(function (data) {
      alert(data.responseText);
   });
}

function style_render() {
   $.ajax({
      url: '/Style/Render/',
      data: {
         itemId: $('#itemId').val(),
         itemType: $('#itemType').val()
      }
   }).done(function(data) {
      $('#renderedText').html(data.html);
      $('#generatedHtml').text(data.html);
   });
}

function style_new() {
   $('#rowCreate').show();
   $('#rowUpdate').hide();
   $('#styleStart').val('');
   $('#styleEnd').val('');
}

function style_bold() {
   $('#styleStart').val('<strong>');
   $('#styleEnd').val('</strong>');
}

function style_italic() {
   $('#styleStart').val('<em>');
   $('#styleEnd').val('</em>');
}

function style_blockQuote() {
   $('#styleStart').val('<blockquote>');
   $('#styleEnd').val('</blockquote>');
}

function style_list() {
   $('#styleStart').val('<ul>');
   $('#styleEnd').val('</ul>');
}

function style_orderedList() {
   $('#styleStart').val('<ol>');
   $('#styleEnd').val('</ol>');
}

function style_listItem() {
   $('#styleStart').val('<li>');
   $('#styleEnd').val('</li>');
}
