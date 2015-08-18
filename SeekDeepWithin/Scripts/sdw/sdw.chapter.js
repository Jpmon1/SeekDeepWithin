$(document).ready(function () {
   $('#passageList').data('loc', 'on');
   $(window).resize(chapter_resize);
   chapter_resize();
});

function chapter_resize() {
   var passageList = $('#passageList');
   var loc = passageList.data('loc');
   if (Foundation.utils.is_small_only()) {
      if (loc === 'on') {
         passageList.remove();
         $('#panel_left').html(passageList);
         passageList.data('loc', 'off');
      }
      passageList.css({ 'height': '', 'margin-bottom': '0' });
   } else {
      if (loc === 'off') {
         passageList.remove();
         panels_hideLeft();
         panels_hideOverlay();
         $('#parentListContainer').append(passageList);
         passageList.data('loc', 'on');
      }
      passageList.css({ 'height': $('#workArea').height(), 'margin-bottom': '1.25rem' });
   }
}

function chapter_edit() {
   sdw_post('/Chapter/Edit/', {
      id: $('#chapterId').val(),
      name: $('#chapterName').val(),
      order: $('#chapterOrder').val(),
      header: $('#chapterHeader').val(),
      footer: $('#chapterFooter').val(),
      visible: $('#chapterVisi').prop('checked'),
      para: !$('#chapterReadMode').prop('checked')
   }, 'Editing Chapter, please wait...');
}

function chapter_edit_header() {
   sdw_get_edit('/Chapter/EditHeader/' + $('#chapterId').val(), true);
}

function chapter_edit_footer() {
   sdw_get_edit('/Chapter/EditFooter/' + $('#chapterId').val(), true);
}
