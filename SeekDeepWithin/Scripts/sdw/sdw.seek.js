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
         var light = item.closest('.light');
         light.toggleClass('selected');
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
         var last = container.children().last().find('.light').first();
         container.append(d);
         container.masonry('reloadItems');
         container.masonry('layout').one('layoutComplete', function () {
            last.velocity('scroll', { duration: 1000 });
         });
         SdwCommon.loadStop();
      });
   },
}

$(document).ready(function () {
   window.loadOnScroll = $('#loadOnScroll').val();
   $('.top-menu-search').velocity('transition.fadeOut', false, false);
   $('#lightList').masonry({ itemSelector: '.column' });
   sdw.love();
   setSearch($('#searchText'));
   setSearch($('#menuSearchText'));
   window.menusearch = false;
   $(window).scroll(SdwCommon.throttle(function () {
      var top = $(window).scrollTop();
      var searchTop = $('#searchText').offset().top;
      if (!window.menusearch && top > searchTop) {
         window.menusearch = true;
         $('.top-menu-search').velocity('transition.fadeIn', false, false);
      } else if (window.menusearch && top < searchTop) {
         window.menusearch = false;
         $('.top-menu-search').velocity('transition.fadeOut', false, false);
      }
      if (top == $(document).height() - $(window).height()) {
         if (window.loadOnScroll == 1) {
            sdw.love();
         }
      }
   }, 300));
});

function setSearch(search) {
   search.autocomplete({
      serviceUrl: '/Seek/AutoComplete',
      paramName: 'text',
      noCache: true,
      deferRequestBy: 500,
      triggerSelectOnValidInput:true
   });
   search.keypress(function (e) {
      var keycode = (e.keyCode ? e.keyCode : e.which);
      if (keycode == '13') {
         search.blur();
         sdw.search();
      }
   });
   search.autocomplete('clearCache');
}
