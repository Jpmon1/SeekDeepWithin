$(document).ready(function () {
   $('#smallLeftMenuIcon').show();
   $('#contents').data('loc', 'on');
   $(window).resize(read_resize);
   read_resize();
   //$.ajax('/Chapter/Contents/' + $('#chapterId').val()).done(function(d) {
   //   $('#contents').append(d);
   //});
});

function read_resize() {
   var contents = $('#contents');
   var loc = contents.data('loc');
   if (Foundation.utils.is_small_only()) {
      if (loc === 'on') {
         contents.remove();
         $('#panel_left').append(contents);
         contents.data('loc', 'off');
      }
      contents.css({ 'height': '', 'margin-bottom': '0' });
   } else {
      if (loc === 'off') {
         contents.remove();
         panels_hideLeft();
         panels_hideOverlay();
         $('#contentContainer').append(contents);
         contents.data('loc', 'on');
      }
      var height = $('#readingArea').height();
      if (height < 200)
         height = 200;
      contents.css({ 'height': height, 'margin-bottom': '1.25rem' });
   }
}

function showContents(id) {
   $('#subBook_' + id).slideToggle();
}
