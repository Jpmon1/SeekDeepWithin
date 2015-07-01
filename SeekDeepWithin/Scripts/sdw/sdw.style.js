function style_setup() {
   $('#styleList').change(function () {
      var selected = $("#styleList").val();
      if (selected == 0) {
         $('#styleStart').val('');
         $('#styleEnd').val('');
      } else if (selected == 1) {
         $('#styleStart').val('<strong>');
         $('#styleEnd').val('</strong>');
      } else if (selected == 2) {
         $('#styleStart').val('<em>');
         $('#styleEnd').val('</em>');
      } else if (selected == 3) {
         $('#styleStart').val('<blockquote>');
         $('#styleEnd').val('</blockquote>');
      } else if (selected == 4) {
         $('#styleStart').val('<ul>');
         $('#styleEnd').val('</ul>');
      } else if (selected == 5) {
         $('#styleStart').val('<ol>');
         $('#styleEnd').val('</ol>');
      } else if (selected == 6) {
         $('#styleStart').val('<li>');
         $('#styleEnd').val('</li>');
      } else if (selected == 7) {
         $('#styleStart').val('<small>');
         $('#styleEnd').val('</small>');
      } else if (selected == 8) {
         $('#styleStart').val('<div>');
         $('#styleEnd').val('</div>');
      } else if (selected == 9) {
         $('#styleStart').val('<div class="tCenter">');
         $('#styleEnd').val('</div>');
      } else if (selected == 10) {
         $('#styleStart').val('<div class="tRight">');
         $('#styleEnd').val('</div>');
      } else if (selected == 11) {
         $('#styleStart').val('<h5 class="text-center" style="font-weight:bold;">');
         $('#styleEnd').val('</h5>');
      } else if (selected == 12) {
         $('#styleStart').val('<div class="text-center" style="font-weight:bold;font-style:italic;">');
         $('#styleEnd').val('</div>');
      } else if (selected == 13) {
         $('#styleStart').val('<div class="text-center" style="font-size:smaller;margin:0.825rem">');
         $('#styleEnd').val('</div>');
      }
   });
}

function style_create() {
   sdw_post('/Style/Create' + $('#editItemType').val(), {
      footerId: $('#editItemFooterId').val(),
      itemId: $('#editItemId').val(),
      startStyle: encodeURIComponent($('#styleStart').val()),
      endStyle: encodeURIComponent($('#styleEnd').val()),
      startIndex: $('#startIndex').val(),
      endIndex: $('#endIndex').val(),
      spansMultiple: $('#styleSpan').prop('checked')
   }, 'Creating Style, please wait...', function (d) {
      style_new();
      $('#currentStyles').append('<a href="javascript:void(0)" class="sdw-button white small expand" onclick="style_edit(' + d.id +
         ')" id="style_' + d.id + '">Start: ' + d.startIndex + ' End: ' + d.endIndex + '</a>');
   });
}

function style_edit(id) {
   $.ajax({
      url: '/Style/Get' + $('#editItemType').val(),
      data: {
         id: id,
         itemId: $('#editItemId').val(),
         footerId: $('#editItemFooterId').val()
      }
   }).done(function (data) {
      $('#styleList').val(0);
      $('#styleCreate').hide();
      $('#styleUpdate').show();
      $('#styleEditId').val(data.id);
      $('#styleStart').val(data.start);
      $('#styleEnd').val(data.end);
      select_word(data.startIndex, data.endIndex);
   });
}

function style_update() {
   sdw_post('/Style/Update' + $('#editItemType').val(), {
      id: $('#styleEditId').val(),
      footerId: $('#editItemFooterId').val(),
      itemId: $('#editItemId').val(),
      startStyle: encodeURIComponent($('#styleStart').val()),
      endStyle: encodeURIComponent($('#styleEnd').val()),
      startIndex: $('#startIndex').val(),
      endIndex: $('#endIndex').val(),
      spansMultiple: $('#styleSpan').prop('checked')

   }, 'Updating Style, please wait...', function () {
      $('#style_' + $('#styleEditId').val()).text('Start: ' + $('#startIndex').val() + ' (' +
         escapeHtml($('#styleStart').val()) + ') End: ' + $('#endIndex').val() + ' (' + escapeHtml($('#styleEnd').val()) + ')');
      style_new();
   });
}

function style_delete() {
   sdw_post('/Style/Delete' + $('#editItemType').val(), {
      id: $('#styleEditId').val(),
      footerId: $('#editItemFooterId').val(),
      itemId: $('#editItemId').val(),
   }, 'Deleting Style, please wait...', function () {
      var id = $('#styleEditId').val();
      $('#style_' + id).remove();
      style_new();
   });
}

function style_new() {
   $('#styleList').val(0);
   $('#styleCreate').show();
   $('#styleUpdate').hide();
   $('#styleStart').val('');
   $('#styleEnd').val('');
}