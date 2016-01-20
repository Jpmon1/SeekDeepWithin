var sdw = {
   love: function (clicked) {
      var id = clicked;
      var parents = '';
      var item = $(clicked);
      if (clicked && isNaN(clicked)) {
         id = item.data('l');
         var isEditable = $('#editable').length > 0;
         parents = item.data('p');
         var light = item.closest('.callout');
         light.toggleClass('selected');
         if (isEditable) {
            SdwEdit.lightAdd(id);
            SdwEdit.truthEdit(item.data('t'));
         }
         var isClicked = item.data('c');
         if (!light.hasClass('selected') || isClicked == 1) { return; }
         item.data('c', 1);
      }
      SdwCommon.loadStart();
      SdwCommon.get('/Seek/Love', { id: id, items: parents }, function (d) {
         var added = $(d);
         var container = $('#lightList');
         if (clicked && isNaN(clicked)) {
            var column = item.closest('.column');
            column.after(added);
         } else {
             container.append(added);
         }
         container.masonry('reloadItems');
         container.masonry('layout');
         SdwCommon.loadStop();
      });
   },

   search: function () {
      SdwCommon.get('/Seek/Search', { text: $('#searchText').val() }, function (d) {
         var container = $('#lightList');
         var last = container.children().last().find('.callout').first();
         container.append(d);
         container.masonry('reloadItems');
         container.masonry('layout').one('layoutComplete', function () {
            last.velocity('scroll', { duration: 1500 });
         });
         SdwCommon.loadStop();
      });
   },
}

$(document).foundation();
$(document).ready(function () {
   $('#lightList').masonry({ itemSelector: '.column' });
   var page = $('#pageId').val();
   if (page == 'index') {
      sdw.love();
   } else {
      SdwCommon.loadStop();
   }
   $('#searchText').autocomplete({
      serviceUrl: '/Seek/AutoComplete',
      paramName: 'text',
      onSelect: function (suggestion) {
         $('#searchText').val(suggestion.value);
         $('#searchText').data('lightId', suggestion.data);
         sdw.search();
      }
   });
   $(window).scroll(Foundation.util.throttle(function() {
      if ($(window).scrollTop() == $(document).height() - $(window).height()) {
         sdw.love();
      }
   }, 300));
});
