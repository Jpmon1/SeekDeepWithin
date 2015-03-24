
$(document).ready(function () {
   $('#saveReadStyleCheck').hide();
   $('#verseRadio').change(verseParaChanged);
   $('#paraRadio').change(verseParaChanged);
});

function verseParaChanged() {
   var para = $('#paraRadio').prop("checked");
   var form = $('#__AjaxAntiForgeryForm');
   var token = $('input[name="__RequestVerificationToken"]', form).val();
   $.ajax({
      type: 'POST',
      url: '/Chapter/ReadStyle/',
      data: {
         __RequestVerificationToken: token,
         paragraph: para,
         id: $('#chapterId').val()
      }
   }).done(function () {
      $('#saveReadStyleCheck').show(200, function () {
         setTimeout(function () { $('#saveReadStyleCheck').hide(100); }, 2000);
      });
   }).fail(function (data) {
      alert(data.responseText);
   });
}
