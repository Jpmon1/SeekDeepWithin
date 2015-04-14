$(document).ready(function () {
   $('#smallLeftMenuSelector').show();
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
   $('#smallLeftMenu').show();
   $(window).resize(function () {
      resizeReadMenu(scotchPanel);
   });
   resizeReadMenu(scotchPanel);
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
   } else {
      if (loc === 'off') {
         scotchPanel.close();
         contents.remove();
         $('#contentPanel').append(contents);
         contents.data('loc', 'on');
      }
   }
}

function showContents(id) {
   $('#subBook_' + id).slideToggle();
}
