$(document).ready(function () {
   $('#contents').data('loc', 'on');
   $(window).resize(read_resize);
   var sbId = $('#subBookId').val();
   read_resize();
   $("#contentArea").scrollTop($('#subBookTitle_' + sbId).position().top);
});

function read_resize() {
   var contents = $('#contents');
   var loc = contents.data('loc');
   if (is_small_only()) {
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
      var height = $('#readingArea').height() - $('#contentTitle').height();
      if (height < 400)
         height = 400;
      contents.css({ 'height': height, 'margin-bottom': '1.25rem' });
   }
}

function showContents(id) {
   $('#subBook_' + id).slideToggle();
}
