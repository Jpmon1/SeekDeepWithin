
function version_default() {
   var form = $('#__AjaxAntiForgeryForm');
   var token = $('input[name="__RequestVerificationToken"]', form).val();
   $.ajax({
      type: 'POST',
      url: '/Version/SetDefaultVersion/',
      data: {
         __RequestVerificationToken: token,
         bookId: $('#BookId').val(),
         versionId: $('#Id').val()
      }
   }).done(function () {
      $('#saveCheck').show(200, function () {
         setTimeout(function () { $('#saveCheck').hide(100); }, 2000);
      });
      createToc();
   }).fail(function (d) {
      $('#modalClose').show();
      $('#modalText').text(d.responseText);
      $('#modal').foundation('reveal', 'open');
   });
}

function version_subBookOrder() {
   var form = $('#__AjaxAntiForgeryForm');
   var token = $('input[name="__RequestVerificationToken"]', form).val();
   $.ajax({
      type: 'POST',
      url: '/Version/UpdateSubBookOrder/',
      data: {
         __RequestVerificationToken: token,
         id: $('#Id').val(),
         start: $('#startOrder').val()
      }
   }).done(function () {
      $('#orderCheck').show(200, function () {
         setTimeout(function () { $('#saveCheck').hide(100); }, 2000);
      });
      createToc();
   }).fail(function (d) {
      $('#modalClose').show();
      $('#modalText').text(d.responseText);
      $('#modal').foundation('reveal', 'open');
   });
}

function version_assignWriter() {

   var form = $('#__AjaxAntiForgeryForm');
   var token = $('input[name="__RequestVerificationToken"]', form).val();
   $.ajax({
      type: 'POST',
      url: '/Version/AssignWriter/',
      data: {
         __RequestVerificationToken: token,
         id: $('#Id').val(),
         writerId: $('#writerId').val(),
         isTranslator: $('#isTranslator').prop("checked")
      }
   }).done(function (d) {
      if ($('#noWriters').length > 0)
         $('#noWriters').remove();
      $('#writers').append('<div class="row" id="writer_' + d.writerId + '">' +
         '<div class="small-1 columns">' +
         '<a href="javascript:void(0)" onclick="version_removeWriter(' + d.id + ', ' + d.writerId + ')" title="Remove">' +
         '<i class="icon-remove-circle" style="color:red;"></i></a></div><div class="small-11 columns">' + d.writer + '</div></div>');
      $('#writerSaved').show(200, function () {
         setTimeout(function () { $('#writerSaved').hide(100); }, 2000);
      });
   }).fail(function (d) {
      $('#modalClose').show();
      $('#modalText').text(d.responseText);
      $('#modal').foundation('reveal', 'open');
   });
}

function version_removeWriter(versionId, writerId) {
   var form = $('#__AjaxAntiForgeryForm');
   var token = $('input[name="__RequestVerificationToken"]', form).val();
   $.ajax({
      type: 'POST',
      url: '/Version/RemoveWriter/',
      data: {
         __RequestVerificationToken: token,
         subBookId: versionId,
         writerId: writerId
      }
   }).done(function () {
      $('#writer_' + writerId).remove();
   }).fail(function (d) {
      $('#modalClose').show();
      $('#modalText').text(d.responseText);
      $('#modal').foundation('reveal', 'open');
   });
}
