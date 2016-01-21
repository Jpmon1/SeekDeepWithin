var sdw = {
   love: function (clicked) {
      var id = clicked;
      var parents = '';
      var history = '';
      var item = $(clicked);
      if (clicked && isNaN(clicked)) {
         id = item.data('l');
         parents = item.data('p');
         history = item.data('h');
         var light = item.closest('.callout');
         light.toggleClass('selected');
         if (window.isEditable) {
            SdwEdit.lightAdd(id);
            SdwEdit.truthEdit(item.data('t'));
         }
         var isClicked = item.data('c');
         if (!light.hasClass('selected') || isClicked == 1) { return; }
         item.data('c', 1);
      }
      SdwCommon.loadStart();
      SdwCommon.get('/Seek/Love', { id: id, items: parents, history: history }, function (d) {
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
      SdwCommon.loadStart();
      SdwCommon.get('/Seek/Search', { text: $('#searchText').val() }, function (d) {
         var container = $('#lightList');
         var last = container.children().last().find('.callout').first();
         container.append(d);
         container.masonry('reloadItems');
         container.masonry('layout').one('layoutComplete', function () {
            last.velocity('scroll', { duration: 1000 });
         });
         SdwCommon.loadStop();
      });
   },
}

$(document).foundation();
$(document).ready(function () {
   $('#menuSearchText').velocity('transition.fadeOut', false, false);
   window.isEditable = $('#editable').length > 0;
   $('#lightList').masonry({ itemSelector: '.column' });
   var page = $('#pageId').val();
   if (page == 'index') {
      sdw.love();
   } else {
      SdwCommon.loadStop();
   }
   setSearch($('#searchText'));
   setSearch($('#menuSearchText'));
   window.menusearch = false;
   $(window).scroll(Foundation.util.throttle(function () {
      var top = $(window).scrollTop();
      var searchTop = $('#searchText').offset().top;
      if (!window.menusearch && top > searchTop) {
         window.menusearch = true;
         $('#menuSearchText').velocity('transition.fadeIn', false, false);
      } else if (window.menusearch && top < searchTop) {
         window.menusearch = false;
         $('#menuSearchText').velocity('transition.fadeOut', false, false);
      }
      if (top == $(document).height() - $(window).height()) {
         if (!window.isEditable) {
            sdw.love();
         }
      }
   }, 300));
});

function setSearch(search) {
   search.autocomplete({
      serviceUrl: '/Seek/AutoComplete',
      paramName: 'text'
   });
   search.keypress(function (e) {
      var keycode = (e.keyCode ? e.keyCode : e.which);
      if (keycode == '13') {
         search.blur();
         sdw.search();
      }
   });
}
