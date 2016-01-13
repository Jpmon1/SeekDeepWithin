var sdw = {
   sel: function (id) {
      $('#lightWeb').show();
      $('#lightList').hide();
      $('#lightWeb').append('<div class="row love" id="lvl_0"><div class="small-12 medium-4 medium-offset-4 large-4 large-offset-4 column">' +
         '<div class="callout" id="light_' + id + '" onclick="sdw.love(' + id + ');">' + $('#l' + id).text() + '</div></div></div>');
      $('#light_' + id).velocity("transition.fadeIn");
      $('#lightList').empty();
      sdw.love(id);
   },

   light: function () {
      $('#lightWeb').hide();
      $('#lightWeb').empty();
      $('#lightList').show();
      $('#lightList').empty();
      SdwCommon.get('/Light/Get', {}, function (d) {
         var count = d.count;
         var container = $('#lightList');
         for (var i = 0; i < count; i++) {
            var light = d.light[i];
            container.append('<div class="small-6 medium-4 large-3 column' + (i == (count - 1) ? ' end' : '') +
               '"><div class="callout" onclick="sdw.sel(' + light.id + ');" id="l' + light.id + '">' + light.text + '</a></div>');
         }
         SdwCommon.loadStop();
      });
   },

   love: function (selected) {
      var data = [];
      var last = $('#light_' + selected);
      last.toggleClass('selected');
      $('.love').each(function (i, o) {
         var level = [];
         $(o).find('.callout').each(function (cI, cO) {
            var light = $(cO);
            var id = parseInt(light.attr("id").substr(6));
            var lObj = { i: id, t: light.data('tid'), l: light.data('lid') };
            if (light.hasClass('selected')) {
               lObj.s = 1;
            }
            level.push(lObj);
         });
         data.push(level);
      });
      if ($('#editable').length > 0) { SdwEdit.truthEdit(last); }
      SdwCommon.get('/Love/Get', { lastId: selected, data: JSON.stringify(data) }, function (d) {
         window.ignoreHistory = true;
         var web = $('#lightWeb');
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
            var index = child.attr('id').substr(8);
            var current = $('#lvl_' + index);
            if (current.length > 0) {
               current.html(child.html());
               child.remove();
               current.velocity('callout.bounce');
            } else {
               child = child.detach();
               web.append(child);
               child.attr('id', 'lvl_' + index);
               child.velocity('transition.fadeIn');
            }
            window.ignoreHistory = false;
         }
         SdwCommon.loadStop();
      });
   },

   loveAll: function () {
      if (window.ignoreHistory)return;
      var l = SdwCommon.getParam('l');
      SdwCommon.get('/Love/Load', { id: l }, function (d) {
         var web = $('#lightWeb');
         var sandbox = $('#sandbox');
         web.empty();
         sandbox.html(d);
         var histId = $('#historyId');
         histId.remove();
         var children = sandbox.children();
         var count = children.length;
         for (var i = 0; i < count; i++) {
            var child = $(children[i]);
            var index = child.attr('id').substr(8);
            child = child.detach();
            web.append(child);
            child.attr('id', 'lvl_' + index);
            child.velocity('transition.fadeIn');
         }
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
