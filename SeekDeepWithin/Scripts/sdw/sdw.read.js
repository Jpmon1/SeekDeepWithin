$(document).ready(function () {
   $('#readMenu').data('isOpen', false);
   $('#readMenu').data('isStuck', false);
   $(document)
      .on('open.fndtn.offcanvas', '[data-offcanvas]', function () {
         $('#readMenu').data('isOpen', true);
         if ($('#readMenu').data('isStuck') === true) {
            $('#readMenu').css({ 'left': $('#readNavBar').offset().left });
         }
      })
      .on('close.fndtn.offcanvas', '[data-offcanvas]', function () {
         $('#readMenu').data('isOpen', false);
         $('#readMenu').css({ 'left': '' });
      });
   var topNavHeight = $('#siteNavDiv').height();
   var readNavHeight = $('#readNavBar').height();
   resizeReadMenu(topNavHeight, readNavHeight);
   $('#readNavBar').stick_in_parent({ offset_top: topNavHeight });
   $('#readMenu').stick_in_parent({ offset_top: topNavHeight + readNavHeight })
      .on("sticky_kit:stick", function () {
         $('#readMenu').data('isStuck', true);
         if ($('#readMenu').data('isOpen') === true) {
            $('#readMenu').css({ 'left': $('#readNavBar').offset().left });
         }
      }).on("sticky_kit:unbottom", function () {
         $('#readMenu').data('isStuck', true);
         if ($('#readMenu').data('isOpen') === true) {
            $('#readMenu').css({ 'left': $('#readNavBar').offset().left });
         }
      }).on("sticky_kit:unstick", function () {
         $('#readMenu').data('isStuck', false);
         $('#readMenu').css({ 'left': '' });
      }).on("sticky_kit:bottom", function () {
         $('#readMenu').data('isStuck', false);
         $('#readMenu').css({ 'left': '' });
      });
   $(window).resize(function () {
      resizeReadMenu(topNavHeight, readNavHeight);
   });
});

function resizeReadMenu(topNavHeight, readNavHeight) {
   var viewHeight = Math.max(document.documentElement.clientHeight, window.innerHeight || 0);
   var maxMenuHeight = viewHeight - (topNavHeight + readNavHeight);
   $('#readMenu').css({ 'height': maxMenuHeight });
}
