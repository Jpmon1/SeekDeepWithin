var sdw = {
   love: function (clicked) {
      var id = clicked;
      var parents = '';
      var history = '';
      var item = $(clicked);
      if (clicked && isNaN(clicked)) {
         item.off();
         id = item.data('l');
         parents = item.data('p');
         history = item.data('h');
         var light = item.closest('.light');
         light.addClass('selected');
         item.addClass('selected');
      }
      SdwCommon.get('/Seek/Love', { id: id, items: parents, history: history }, function (d) {
         var added = $(d);
         added.find('.truth').each(function(i,o) {
            $(o).click(function(e) {
               sdw.love(e.currentTarget);
            });
         });
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
         var last = container.children().last().find('.light').first();
         var added = $(d);
         added.find('.truth').each(function (i, o) {
            $(o).click(function (e) {
               sdw.love(e.currentTarget);
            });
         });
         container.append(added);
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
   $('#lightList').masonry({ itemSelector: '.column' });
   $('#loadLove').click(function(e) {
      e.preventDefault();
      sdw.love();
   });
   $('#btnSearch').click(function (e) {
      e.preventDefault();
      $('#searchText').blur();
      sdw.search();
   });
   sdw.love();
   setSearch($('#searchText'));
   window.menusearch = false;
   $(window).scroll(SdwCommon.throttle(function () {
      var top = $(window).scrollTop();
      var searchTop = $('#searchRow').offset().top;
      if (!window.menusearch && top > searchTop) {
         window.menusearch = true;
         $('#searchText').detach()
            .addClass('top-menu-search').appendTo('#menuSearch');
         $('#btnSearch').detach()
            .addClass('top-menu-search white')
            .removeClass('alt')
            .css({ 'padding': '0.333rem' })
            .appendTo('#menuBtnSearch');
      } else if (window.menusearch && top < searchTop) {
         window.menusearch = false;
         $('#searchText').detach()
            .removeClass('top-menu-search').appendTo('#mainSearch');
         $('#btnSearch').detach()
            .removeClass('top-menu-search white')
            .addClass('alt').css({ 'padding': '0.625rem 1.25rem 0.6875rem 1.25rem' })
            .appendTo('#mainBtnSearch');
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
