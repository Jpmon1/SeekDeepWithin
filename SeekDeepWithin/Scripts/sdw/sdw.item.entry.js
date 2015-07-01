$(document).ready(function () {
   $('#smallLeftMenuIcon').show();
   $('#entryList').data('loc', 'on');
   $(window).resize(item_entry_resize);
   item_entry_resize();
});

function item_entry_resize() {
   var entryList = $('#entryList');
   var loc = entryList.data('loc');
   if (Foundation.utils.is_small_only()) {
      if (loc === 'on') {
         entryList.remove();
         $('#panel_left').html(entryList);
         entryList.data('loc', 'off');
      }
      entryList.css({ 'height': '', 'margin-bottom': '0' });
   } else {
      if (loc === 'off') {
         entryList.remove();
         panels_hideLeft();
         panels_hideOverlay();
         $('#parentListContainer').append(entryList);
         entryList.data('loc', 'on');
      }
      entryList.css({ 'height': $('#workArea').height(), 'margin-bottom': '1.25rem' });
   }
}

$(document).on('closed', '.remodal', function (e) {
   if (document.entryDel) {
      document.entryDel = false;
      if (e.reason === 'confirmation') {
         entry_delete();
      }
   }
});

function entry_get(id) {
   $.ajax({
      type: 'GET',
      url: '/ItemEntry/Get/' + id
   }).done(function (data) {
      $('#entryText').val(data.text);
      $('#entryOrder').val(data.order);
      $('#editEntryId').val(data.entryId);
      $('#entryHeader').val(data.header);
      var item = $('#item_' + document.prevEntryId);
      if (item.length > 0) {
         item.removeClass('active');
      }
      item = $('#item_' + data.entryId);
      if (item.length > 0) {
         item.addClass('active');
      }
      panels_hideLeft();
      panels_hideOverlay();
      document.prevEntryId = data.entryId;
      sdw_get_edit('/ItemEntry/Edit/' + data.entryId);
   }).fail(function () {
      $('#modal-content-close').show();
      $('#modal-content-title').text('Failed');
      $('#modal-content-text').text(d.responseText);
      document.modal.open();
   });
}

function entry_update() {
   sdw_post('/ItemEntry/Update/', {
      text: $('#entryText').val(),
      order: $('#entryOrder').val(),
      entryId: $('#editEntryId').val(),
      header: $('#entryHeader').val()
   }, 'Saving Entry, please wait...', function () {
      setTimeout(function () { $('#modal').foundation('reveal', 'close'); }, 700);
   });
}

function entry_edit_header() {
   sdw_get_edit('/ItemEntry/EditHeader/' + $('#editEntryId').val());
}

function entry_check_delete() {
   document.entryDel = true;
   $('#modal-content-close').show();
   $('#modal-content-title').text('Delete');
   $('#modal-content-text').text('Are you certain you want to delete this entry?');
   document.modal.open();
}

function entry_delete() {
   sdw_post('/ItemEntry/Delete/', {
      entryId: $('#editEntryId').val()
   }, 'Deleting Entry, please wait...', function () {
      var entryId = $('#editEntryId').val();
      $('#item_' + entryId).remove();
      $('#entryText').val('');
      $('#entryOrder').val('');
      $('#editEntryId').val('');
      $('#btnEditLinks').attr('href', '#');
      $('#btnEditStyles').attr('href', '#');
      $('#btnEditHeaders').attr('href', '#');
      $('#btnEditFooters').attr('href', '#');
   });
}

function entry_next() {
   var entryId = $('#editEntryId').val();
   var item = $('#item_' + entryId);
   if (item.length > 0) {
      var nextItem = item.next();
      if (nextItem.length > 0) {
         var id = nextItem.attr('id');
         entry_get(id.substring(5));
      }
   }
}

function entry_previous() {
   var entryId = $('#editEntryId').val();
   var item = $('#item_' + entryId);
   if (item.length > 0) {
      var prevItem = item.prev();
      if (prevItem.length > 0) {
         var id = prevItem.attr('id');
         entry_get(id.substring(5));
      }
   }
}
