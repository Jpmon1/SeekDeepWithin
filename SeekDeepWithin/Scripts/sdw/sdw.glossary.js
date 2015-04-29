
function entry_get(id) {
   $.ajax({
      type: 'GET',
      url: '/Glossary/GetEntry/' + id
   }).done(function (data) {
      $('#entryText').val(data.text);
      $('#entryOrder').val(data.order);
      $('#editEntryId').text(data.entryId);
      $('#btnEditLinks').attr('href', '/Link/EditEntry?id=' + data.entryId);
      $('#btnEditStyles').attr('href', '/Style/EditEntry?id=' + data.entryId);
      $('#btnEditHeaders').attr('href', '/Header/Edit?id=' + data.entryId + "&type=Entry");
      $('#btnEditFooters').attr('href', '/Footer/Edit?id=' + data.entryId + "&type=Entry");
   }).fail(function (data) {
      $('#modalClose').show();
      $('#modal').foundation('reveal', 'open');
      $('#modalText').text('An error occured - ' + data.responseText);
   });
}

function entry_update() {
   $('#modalClose').hide();
   $('#modalText').text('Saving Entry, please wait...');
   $('#modal').foundation('reveal', 'open');
   var form = $('#__AjaxAntiForgeryForm');
   var token = $('input[name="__RequestVerificationToken"]', form).val();
   $.ajax({
      type: 'POST',
      url: '/Glossary/UpdateEntry/',
      data: {
         __RequestVerificationToken: token,
         text: $('#entryText').val(),
         order: $('#entryOrder').val(),
         entryId: $('#editEntryId').text()
      }
   }).done(function () {
      setTimeout(function () { $('#modal').foundation('reveal', 'close'); }, 700);
   }).fail(function (data) {
      $('#modalClose').show();
      $('#modalText').text('An error occured - ' + data.responseText);
   });
}

function entry_delete() {
   $('#modalClose').hide();
   $('#modalText').text('Deleting Entry, please wait...');
   $('#modal').foundation('reveal', 'open');
   var form = $('#__AjaxAntiForgeryForm');
   var token = $('input[name="__RequestVerificationToken"]', form).val();
   $.ajax({
      type: 'POST',
      url: '/Glossary/DeleteEntry/',
      data: {
         __RequestVerificationToken: token,
         entryId: $('#editEntryId').text()
      }
   }).done(function () {
      $('#modalText').text('Success!');
      var entryId = $('#editEntryId').text();
      $('#item_' + entryId).remove();
      $('#entryText').val('');
      $('#entryOrder').val('');
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

function entry_next() {
   var entryId = $('#editEntryId').text();
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
   var entryId = $('#editEntryId').text();
   var item = $('#item_' + entryId);
   if (item.length > 0) {
      var prevItem = item.prev();
      if (prevItem.length > 0) {
         var id = prevItem.attr('id');
         entry_get(id.substring(5));
      }
   }
}
