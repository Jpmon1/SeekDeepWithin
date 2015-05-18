
$(document).ready(function () {
   $('#saveReadStyleCheck').hide();
   $('#verseRadio').change(verseParaChanged);
   $('#paraRadio').change(verseParaChanged);
   $('#smallLeftMenuIcon').show();
   $('#leftMenu').data('loc', 'on');
   $(window).resize(function () {
      chapter_resize();
   });
   chapter_resize();
});

function chapter_resize() {
   var contents = $('#leftMenu');
   var loc = contents.data('loc');
   if (Foundation.utils.is_small_only()) {
      if (loc === 'on') {
         contents.remove();
         $('#panel_left').html(contents);
         contents.data('loc', 'off');
      }
   } else {
      if (loc === 'off') {
         contents.remove();
         panels_hideLeft();
         panels_hideOverlay();
         $('#contentPanel').append(contents);
         contents.data('loc', 'on');
      }
      $('#contentPanel').css({ 'height': $('#workArea').height() });
   }
}

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
