
function setAsDefaultVersion() {
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
      alert(d.responseText);
   });
}
