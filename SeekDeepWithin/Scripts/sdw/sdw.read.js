$(document).ready(function () {
   $('#smallLeftMenuIcon').show();
   $('#leftMenu').data('loc', 'on');
   var scotchPanel = $('#smallLeftMenu').scotchPanel({
      containerSelector: 'body',
      direction: 'left',
      duration: 300,
      transition: 'ease-out',
      distanceX: '250px',
      forceMinHeight: true,
      clickSelector: '.toggle-left-panel',
      enableEscapeKey: true
   });
   document.rightMenu = scotchPanel;
   $('#smallLeftMenu').show();
   $(window).resize(function () {
      resizeReadMenu(scotchPanel);
   });
   resizeReadMenu(scotchPanel);
   $('.overlay').click(function () {
      // CLOSE ONLY
      scotchPanel.close();
   });
});

function resizeReadMenu(scotchPanel) {
   var contents = $('#leftMenu');
   var loc = contents.data('loc');
   if (Foundation.utils.is_small_only()) {
      if (loc === 'on') {
         contents.remove();
         $('#smallLeftMenuContent').append(contents);
         contents.data('loc', 'off');
      }
      $('#smallLeftMenuContent').css({ 'height': $('body').height() });
   } else {
      if (loc === 'off') {
         scotchPanel.close();
         contents.remove();
         $('#contentPanel').append(contents);
         contents.data('loc', 'on');
      }
      $('#contentPanel').css({ 'height': $('#readingArea').height() });
   }
}

function showContents(id) {
   $('#subBook_' + id).slideToggle();
}
