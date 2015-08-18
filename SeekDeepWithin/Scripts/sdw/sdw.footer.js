function footer_create() {
   sdw_post('/Footer/Create' + $('#editItemType').val(), {
      itemId: $('#editItemId').val(),
      text: $('#footerText').val(),
      index: $('#footerIndex').val()
   },
      'Creating Footer, please wait...', function (d) {
         var text = $('#footerText').val();
         var len = text.length < 15 ? text.length : 15;
         $('#currentFooters').append('<a href="javascript:void(0)" id="footer_' + d.id + '" onclick="footer_edit(' + d.id +
            ')" class="sdw-button white small expand">' + d.index + ' - ' + text.substring(0, len) + '</a>');
         footer_new();
      });
}

function sdw_item_edit_footer() {
   var type = $('#editItemType').val();
   if (type === 'Passage') {
      sdw_get_edit('/Passage/EditFooter?id=' + $('#editEntryId').val() + '&footerId=' + $('#footerEditId').val(), false);
   } else {
      sdw_get_edit('/ItemEntry/EditFooter?id=' + $('#editEntryId').val() + '&footerId=' + $('#footerEditId').val(), false);
   }
}

function footer_edit(id) {
   $.ajax({
      url: '/Footer/Get' + $('#editItemType').val(),
      data: {
         id: id,
         itemId: $('#editItemId').val()
      }
   }).done(function (data) {
      $('#footerCreate').hide();
      $('#footerUpdate').show();
      $('#footerEditId').val(data.id);
      $('#footerText').val(data.text);
      $('#footerIndex').val(data.index);
   });
}

function footer_update() {
   sdw_post('/Footer/Update' + $('#editItemType').val(), {
      itemId: $('#editItemId').val(),
      text: $('#footerText').val(),
      index: $('#footerIndex').val(),
      id: $('#footerEditId').val()
   }, 'Updating Footer, please wait...', function () {
      var text = $('#footerText').val();
      var len = text.length < 15 ? text.length : 15;
      $('#footer_' + $('#footerEditId').val()).text($('#footerIndex').val() + ' - ' + text.substring(0, len));
      footer_new();
   });
}

function footer_delete() {
   sdw_post('/Footer/Delete' + $('#editItemType').val(), {
      itemId: $('#editItemId').val(),
      id: $('#footerEditId').val()
   }, 'Deleting Footer, please wait...', function () {
      var id = $('#footerEditId').val();
      $('#footer_' + id).remove();
      footer_new();
   });
}

function footer_new() {
   $('#footerCreate').show();
   $('#footerUpdate').hide();
   $('#footerText').val('');
   $('#footerEditId').val('');
}