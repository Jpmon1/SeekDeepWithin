var sdw = {

   love: function (id, parents, key, search) {
      if (parents == undefined) parents = '';
      var isEditable = $('#editable').length > 0;
      var last = $('#l' + key);
      if (isEditable) { SdwEdit.truthAdd(id); }
      if (last.length > 0) {
         last.toggleClass('selected');
         if (isEditable) { SdwEdit.truthEdit(last.data('tid')); }
         var isClicked = last.data('c');
         if (!last.hasClass('selected') || isClicked == 1) { return; }
      }
      SdwCommon.loadStart();
      var l = SdwCommon.getParam('l');
      SdwCommon.get('/Love/Get', { id: id, items: parents, current: parseInt(l) }, function (d) {
         window.ignoreHistory = true;
         var added = $(d);
         var container = $('#lightList');
         added.each(function (i, o) {
            var r = $(o);
            if (r.prop("tagName") == 'EM') {
               var text = r.text();
               var it = $('#c' + text);
               container.masonry('remove', it);
               r.remove();
            }
         });
         var hist = added.first('#historyId');
         History.pushState('Seek Deep Within', 'Seek Deep Within', '?l=' + hist.val());
         hist.remove();
         if (last.length > 0) {
            var col = $('#c' + key);
            col.after(added);
            last.data('c', 1);
         } else {
            container.append(added);
            $('#l' + key).data('c', 1);
         }
         container.masonry('reloadItems');
         container.masonry('layout').one('layoutComplete', function () {
            if (search) { $('#c' + key).velocity('scroll', { duration: 1500 }); }
         });
         window.ignoreHistory = false;
         SdwCommon.loadStop();
      });
   },

   search: function () {
      SdwCommon.get('/Love/Search', { text: $('#searchText').val() }, function (d) {
         window.ignoreHistory = true;
         var container = $('#lightList');
         var last = container.children().last().find('.callout').first();
         container.append(d);
         container.masonry('reloadItems');
         container.masonry('layout').one('layoutComplete', function () {
            last.velocity('scroll', { duration: 1500 });
         });
         window.ignoreHistory = false;
         SdwCommon.loadStop();
      });
   },

   histChange: function () {
      if (window.ignoreHistory) return;
      var page = $('#pageId').val();
      if (page == 'index') {
         var l = SdwCommon.getParam('l');
         if (l == null || l == '') {
            sdw.love();
         } else {
            /*var states = History.savedStates;
            var prevUrl = states[states.length - 2];*/
            SdwCommon.get('/Love/Load', { id: parseInt(l) }, function (d) {
               var container = $('#lightList');
               container.html(d);
               container.masonry('reloadItems');
               container.masonry('layout').one('layoutComplete', function () {
                  var item = container.children().last().find('.selected').last();
                  if (item.length > 0) { item.velocity('scroll', { duration: 1500 }); }
               });;
               SdwCommon.loadStop();
            });
         }
      } else {
         SdwCommon.loadStop();
      }
   },
}
$(document).foundation();
$(document).ready(function () {
   $('#lightList').masonry({ itemSelector: '.column' });
   History.Adapter.bind(window, 'statechange', sdw.histChange);
   sdw.histChange();
   $('#searchText').autocomplete({
      serviceUrl: '/Light/AutoComplete',
      paramName: 'text',
      onSelect: function (suggestion) {
         $('#searchText').val(suggestion.value);
         $('#searchText').data('lightId', suggestion.data);
         sdw.love(suggestion.data, '', '', true);
      }
   });
});
