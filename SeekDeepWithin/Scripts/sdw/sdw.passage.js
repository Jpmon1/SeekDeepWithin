
$(document).on('closed', '.remodal', function (e) {
   if (document.passageDel) {
      document.passageDel = false;
      if (e.reason === 'confirmation') {
         passage_delete();
      }
   }
});

function passage_update() {
   sdw_post('/Passage/Update/', {
      text: $('#passText').val(),
      order: $('#entryOrder').val(),
      number: $('#entryNumber').val(),
      entryId: $('#editEntryId').val(),
      header: $('#passHeader').val()
   }, 'Updating Passage, please wait...');
}

function passage_check_delete() {
   document.passageDel = true;
   $('#modal-content-close').show();
   $('#modal-content-title').text('Delete');
   $('#modal-content-text').text('Are you certain you want to delete this passage?');
   document.modal.open();
}

function passage_delete() {
   sdw_post('/Passage/Delete/', { entryId: $('#editEntryId').val() }, 'Deleting Passage, please wait...', function () {
      var entryId = $('#editEntryId').val();
      $('#item_' + entryId).remove();
      $('#passText').val('');
      $('#entryNumber').val('');
      $('#entryOrder').val('');
      $('#editPassId').val('');
      $('#editEntryId').val('');
      $('#btnEditLinks').attr('href', '#');
      $('#btnEditStyles').attr('href', '#');
      $('#btnEditHeaders').attr('href', '#');
      $('#btnEditFooters').attr('href', '#');
   });
}

function passage_get(id) {
   $('#editItem').html('<div class="tCenter"><img src="../../Content/ajax-loader_2.gif" alt="Loading..."/></div>');
   $.ajax({
      type: 'GET',
      url: '/Passage/Get/' + id
   }).done(function (data) {
      $('#passText').val(data.passageText);
      //$('#passText').selectRange(0);
      $('#entryNumber').val(data.passageNumber);
      $('#entryOrder').val(data.order);
      $('#editPassId').val(data.passageId);
      $('#editEntryId').val(data.entryId);
      $('#passHeader').val(data.header);
      var item = $('#item_' + document.prevEntryId);
      if (item.length > 0) {
         item.removeClass('active');
      }
      item = $('#item_' + data.entryId);
      if (item.length > 0) {
         item.addClass('active');
         $("#passageList").scrollTop(item.position().top);
      }
      panels_hideLeft();
      panels_hideOverlay();
      document.prevEntryId = data.entryId;

      sdw_get_edit('/Passage/Edit/' + data.entryId, false);
   }).fail(function (d) {
      $('#modal-content-close').show();
      $('#modal-content-title').text('Failed');
      $('#modal-content-text').text(d.responseText);
      document.modal.open();
   });
}

function passage_edit_header() {
   sdw_get_edit('/Passage/EditHeader/' + $('#editEntryId').val(), true);
}

function passage_next() {
   var entryId = $('#editEntryId').val();
   var item = $('#item_' + entryId);
   if (item.length > 0) {
      var nextItem = item.next();
      if (nextItem.length > 0) {
         var id = nextItem.attr('id');
         passage_get(id.substring(5));
      }
   }
}

function passage_previous() {
   var entryId = $('#editEntryId').val();
   var item = $('#item_' + entryId);
   if (item.length > 0) {
      var prevItem = item.prev();
      if (prevItem.length > 0) {
         var id = prevItem.attr('id');
         passage_get(id.substring(5));
      }
   }
}