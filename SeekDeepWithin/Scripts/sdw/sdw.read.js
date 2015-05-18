$(document).ready(function () {
   $('#smallLeftMenuIcon').show();
   $('#leftMenu').data('loc', 'on');
   $(window).resize(function () {
      resizeReadMenu();
   });
   resizeReadMenu();
});

function resizeReadMenu() {
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
      $('#contentPanel').css({ 'height': $('#readingArea').height() });
   }
}

function showContents(id) {
   $('#subBook_' + id).slideToggle();
}
