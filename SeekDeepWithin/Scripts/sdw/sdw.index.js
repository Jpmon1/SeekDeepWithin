function index_create() {
   var form = $('#__AjaxAntiForgeryForm');
   var token = $('input[name="__RequestVerificationToken"]', form).val();
   $('#modalClose').hide();
   $('#modalText').text('Creating Index, please wait...');
   $('#modal').foundation('reveal', 'open');
   var start = parseInt($('#startIndex').val());
   var length = parseInt($('#lengthIndex').val());
   $.ajax({
      type: 'POST',
      url: '/Search/CreateIndex/',
      data: {
         __RequestVerificationToken: token,
         type: $('#selCreateIndex').val(),
         start: start,
         length: length
      }
   }).done(function (d) {
      $('#totalCount').text(d.count + ' total records.');
      $('#modalText').text('Success!');
      if (start != -1) {
         $('#startIndex').val(start + length);
      }
      setTimeout(function () { $('#modal').foundation('reveal', 'close'); }, 700);
   }).fail(function (d) {
      $('#modalClose').show();
      $('#modalText').text(d.responseText);
   });
}

function index_optimize() {
   var form = $('#__AjaxAntiForgeryForm');
   var token = $('input[name="__RequestVerificationToken"]', form).val();
   $('#modalClose').hide();
   $('#modalText').text('Optimizing Indexes, please wait...');
   $('#modal').foundation('reveal', 'open');
   $.ajax({
      type: 'POST',
      url: '/Search/OptimizeIndex/',
      data: {
         __RequestVerificationToken: token,
         type: $('#selOptIndex').val()
      }
   }).done(function () {
      $('#modalText').text('Success!');
      setTimeout(function () { $('#modal').foundation('reveal', 'close'); }, 700);
   }).fail(function (d) {
      $('#modalClose').show();
      $('#modalText').text(d.responseText);
   });
}

function index_delete() {
   var form = $('#__AjaxAntiForgeryForm');
   var token = $('input[name="__RequestVerificationToken"]', form).val();
   $('#modalClose').hide();
   $('#modalText').text('Deleting Indexes, please wait...');
   $('#modal').foundation('reveal', 'open');
   $.ajax({
      type: 'POST',
      url: '/Search/DeleteIndex/',
      data: {
         __RequestVerificationToken: token,
         type: $('#selDelIndex').val()
      }
   }).done(function () {
      $('#modalText').text('Success!');
      setTimeout(function () { $('#modal').foundation('reveal', 'close'); }, 700);
   }).fail(function (d) {
      $('#modalClose').show();
      $('#modalText').text(d.responseText);
   });
}
