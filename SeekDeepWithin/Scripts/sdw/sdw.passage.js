
function passage_update() {
   $('#modalClose').hide();
   $('#modalText').text('Updating Passage, please wait...');
   $('#modal').foundation('reveal', 'open');
   var form = $('#__AjaxAntiForgeryForm');
   var token = $('input[name="__RequestVerificationToken"]', form).val();
   $.ajax({
      type: 'POST',
      url: '/Passage/Update/',
      data: {
         __RequestVerificationToken: token,
         text: $('#passText').val(),
         order: $('#entryOrder').val(),
         number: $('#entryNumber').val(),
         entryId: $('#editEntryId').text()
      }
   }).done(function () {
      $('#modalText').text('Success!');
      setTimeout(function () { $('#modal').foundation('reveal', 'close'); }, 700);
   }).fail(function (data) {
      $('#modalClose').show();
      $('#modalText').text('An error occured - ' + data.responseText);
   });
}

function passage_delete() {
   $('#modalClose').hide();
   $('#modalText').text('Deleting Passage, please wait...');
   $('#modal').foundation('reveal', 'open');
   var form = $('#__AjaxAntiForgeryForm');
   var token = $('input[name="__RequestVerificationToken"]', form).val();
   $.ajax({
      type: 'POST',
      url: '/Passage/Delete/',
      data: {
         __RequestVerificationToken: token,
         entryId: $('#editEntryId').text()
      }
   }).done(function () {
      $('#modalText').text('Success!');
      var entryId = $('#editEntryId').text();
      $('#item_' + entryId).remove();
      $('#passText').val('');
      $('#entryNumber').val('');
      $('#entryOrder').val('');
      $('#editPassId').text('');
      $('#editEntryId').text('');
      $('#btnEditLinks').attr('href', '#');
      $('#btnEditStyles').attr('href', '#');
      $('#btnEditHeaders').attr('href', '#');
      $('#btnEditFooters').attr('href', '#');
      setTimeout(function () { $('#modal').foundation('reveal', 'close'); }, 700);
   }).fail(function (data) {
      $('#modalClose').show();
      $('#modalText').text('An error occured - ' + data.responseText);
   });
}

function passage_get(id) {
   $.ajax({
      type: 'GET',
      url: '/Passage/Get/' + id
   }).done(function (data) {
      $('#passText').val(data.passageText);
      $('#entryNumber').val(data.passageNumber);
      $('#entryOrder').val(data.order);
      $('#editPassId').text(data.passageId);
      $('#editEntryId').text(data.entryId);
      $('#btnEditLinks').attr('href', '/Link/EditPassage?id=' + data.entryId);
      $('#btnEditStyles').attr('href', '/Style/EditPassage?id=' + data.entryId);
      $('#btnEditHeaders').attr('href', '/Header/Edit?id=' + data.entryId + "&type=Passage");
      $('#btnEditFooters').attr('href', '/Footer/Edit?id=' + data.entryId + "&type=Passage");
   }).fail(function (data) {
      $('#modalClose').show();
      $('#modal').foundation('reveal', 'open');
      $('#modalText').text('An error occured - ' + data.responseText);
   });
}

function passage_next() {
   var entryId = $('#editEntryId').text();
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
   var entryId = $('#editEntryId').text();
   var item = $('#item_' + entryId);
   if (item.length > 0) {
      var prevItem = item.prev();
      if (prevItem.length > 0) {
         var id = prevItem.attr('id');
         passage_get(id.substring(5));
      }
   }
}