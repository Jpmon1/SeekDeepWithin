var sdw = {
   sel: function (id) {
      $('#lightList').empty();
      sdw.love(id, '', true);
   },

   light: function () {
      SdwCommon.loadStart();
      $('#lightList').empty();
      SdwCommon.get('/Light/Get', {}, function (d) {
         $('#lightList').html(d).masonry({itemSelector: '.column'});
         SdwCommon.loadStop();
      });
   },

   love: function (lightId, parents, root) {
      SdwCommon.loadStart();
      if (parents == undefined) parents = 0;
      var isEditable = $('#editable').length > 0;
      var last = $('#l' + lightId + '_' + parents);
      if (last.length > 0) {
         last.toggleClass('selected');
         if (isEditable) {
            SdwEdit.truthEdit(last.data('tid'));
            SdwEdit.truthAdd(lightId);
         }
         var clicked = last.data('c');
         if (!last.hasClass('selected') || clicked == 1) {
            SdwCommon.loadStop();
            return;
         }
      } else if (isEditable) {
         SdwEdit.truthAdd(lightId);
      }
      SdwCommon.post('/Love/Get', { clicked: lightId, items: parents, root: root }, function (d) {
         window.ignoreHistory = true;
         var container = $('#lightList');
         if (root) {
            container.append(d);
            $('#l' + lightId + '_').data('c', 1);
         } else {
            var col = $('#c' + lightId + "_" + parents);
            col.after(d);
            last.data('c', 1);
         }
         container.masonry('reloadItems');
         container.masonry('layout').one('layoutComplete', function () {
            if (root) {
               $('#l' + lightId + '_').velocity("scroll", { duration: 1500 });
            }
         });
         window.ignoreHistory = false;
         SdwCommon.loadStop();
      });
   },

   loveAll: function () {
      SdwCommon.loadStart();
      var l = SdwCommon.getParam('l');
      SdwCommon.get('/Love/Load', { data: l }, function (d) {
         var container = $('#lightList');
         var sandbox = $('#sandbox');
         container.empty();
         sandbox.html(d);
         var histId = $('#historyId');
         histId.remove();
         var children = sandbox.children();
         var count = children.length;
         for (var i = 0; i < count; i++) {
            var child = $(children[i]);
            var key = child.attr('id').substr(2);
            child = child.detach();
            container.append(child);
            child.attr('id', 's' + key);
            child.velocity('transition.fadeIn');
         }
         SdwCommon.loadStop();
      });
   },

   histChange: function () {
      if (window.ignoreHistory) return;
      var page = $('#pageId').val();
      if (page == 'index') {
         var l = SdwCommon.getParam('l');
         if (l == null || l == '') {
            sdw.light();
         } else {
            sdw.loveAll();
         }
      } else {
         SdwCommon.loadStop();
      }
   },
}
$(document).foundation();
$(document).ready(function() {
   History.Adapter.bind(window, 'statechange', sdw.histChange);
   sdw.histChange();
   $('#searchText').autocomplete({
      serviceUrl: '/Light/AutoComplete',
      paramName: 'text',
      onSelect: function (suggestion) {
         $('#searchText').val(suggestion.value);
         $('#searchText').data('lightId', suggestion.data);
         sdw.love(suggestion.data, '', true);
      }
   });
});
