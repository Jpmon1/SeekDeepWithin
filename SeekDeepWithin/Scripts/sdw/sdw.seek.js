var sdw = {
   sel: function (id) {
      $('#lightList').masonry('destroy');
      $('#lightList').empty();
      sdw.love(id);
   },

   light: function () {
      SdwCommon.loadStart();
      $('#lightList').empty();
      SdwCommon.get('/Light/Get', {}, function (d) {
         $('#lightList').html(d).masonry({itemSelector: '.column'});
         SdwCommon.loadStop();
      });
   },

   love: function (lightId, levelId) {
      SdwCommon.loadStart();
      if (levelId == undefined) levelId = 0;
      var isEditable = $('#editable').length > 0;
      var last = $('#l' + lightId + "_" + levelId);
      if (last.length > 0) {
         last.toggleClass('selected');
         if (isEditable) {
            SdwEdit.truthEdit(last.data('tid'));
            SdwEdit.truthAdd(lightId);
         }
         if (!last.hasClass('selected')) { return; }
      } else if (isEditable) {
         SdwEdit.truthAdd(lightId);
      }
      var data = [];
      var hash = new Hashids('GodisLove');
      $('.love').each(function (i, o) {
         var love = $(o);
         var key = parseInt(love.attr('id').substr(1));
         data.push(0);
         data.push(key);
         love.find('.selected').each(function (cI, cO) {
            var light = $(cO);
            var id = parseInt(light.attr("id").substr(1));
            data.push(id);
         });
      });
      // Remove lights and Re-layout current level
      // SEND TO SERVER = list of levels, list of selected, list of get all, selected id
      // Add new lights
      // Add layed out Removed lights
      SdwCommon.post('/Love/Get', { clicked: lightId, data: hash.encode(data), levelId: levelId }, function (d) {
         window.ignoreHistory = true;
         var container = $('#lightList');
         var sandbox = $('#sandbox');
         sandbox.html(d);
         var histId = $('#historyId');
         var history = '?l=' + histId.val();
         History.pushState('Seek Deep Within', 'Seek Deep Within', history);
         histId.remove();
         var children = sandbox.children();
         var count = children.length;
         for (var i = 0; i < count; i++) {
            var child = $(children[i]);
            var rKey = parseInt(child.data('rkey'));
            var pKey = parseInt(child.data('pkey'));
            var key = child.attr('id').substr(2);
            if (rKey > 0) {
               var replace = $('#s' + rKey);
               replace.html(child.html());
               replace.attr('id', 's' + key);
               child.remove();
               replace.velocity('callout.bounce');
            } else if (pKey > 0) {
               var parent = $('#s' + pKey);
               child = child.detach();
               parent.after(child);
               child.attr('id', 's' + key);
               child.velocity('transition.slideDownIn');
            } else {
               child = child.detach();
               container.append(child);
               child.attr('id', 's' + key);
               child.velocity('transition.slideDownIn');
            }
         }
         window.ignoreHistory = false;
         SdwCommon.loadStop();
      });
   },

   loveAll: function () {
      SdwCommon.loadStart();
      if (window.ignoreHistory) return;
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
         // TODO: Load/Append this item...
      }
   });
});
