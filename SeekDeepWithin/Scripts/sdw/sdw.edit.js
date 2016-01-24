var SdwEdit = {

   init: function () {
      $('#addTruthAppendText').autocomplete({
         serviceUrl: '/Seek/AutoComplete',
         paramName: 'text',
         noCache: true,
         onSelect: function(suggestion) {
            $('#addTruthAppendText').val(suggestion.value);
            $('#addTruthAppendText').data('lightId', suggestion.data);
         }
      });
      $('#editLightSearch').autocomplete({
         serviceUrl: '/Seek/AutoComplete',
         paramName: 'text',
         noCache: true,
         onSelect: function (suggestion) {
            $('#editLightSearch').val(suggestion.value);
            $('#editLightSearch').data('lightId', suggestion.data);
         }
      });
      $('#addTruthFormatRegex').val($("#addTruthCurrentRegex option:selected").text());
      $('#addTruthCurrentRegex').change(function() {
         $('#addTruthFormatRegex').val($("#addTruthCurrentRegex option:selected").text());
      });
      $('#addTruthAppendText').autocomplete('clearCache');
      SdwEdit._getLinkTruths();
   },

   lightCreate: function () {
      SdwCommon.loadStart();
      SdwEdit._post('/Edit/Illuminate', { text: $('#textNewLight').val() }, function (d) {
         $('#textNewLight').val('');
         SdwCommon.loadStop();
         SdwEdit.lightGetItem(d.id);
      });
   },

   lightAddItem: function () {
      var id = $('#editLightSearch').data('lightId');
      $('#editLightSearch').val('');
      $('#editLightSearch').data('lightId', '');
      SdwEdit.lightGetItem(id);
   },

   lightAddLinkItem: function () {
      SdwCommon.loadStart();
      var id = $('#editLightSearch').data('lightId');
      SdwCommon.get('/Edit/Light', { id: id, link: true }, function (html) {
         $('#linkArea').append(html).velocity('transition.fadeIn');
         SdwCommon.loadStop();
         SdwEdit._getLinkTruths();
      });
   },

   lightGetItem: function (id) {
      SdwCommon.loadStart();
      SdwCommon.get('/Edit/Light', { id: id }, function (html) {
         $('#editLights').append(html).velocity('transition.fadeIn');
         SdwCommon.loadStop();
         SdwEdit._getLinks();
      });
   },

   lightEdit: function (id) {
      SdwCommon.loadStart();
      SdwCommon.get('/Edit/LightEdit', { id: id }, function (html) {
         $('#editArea').html(html).velocity('transition.fadeIn').velocity('scroll', { duration: 300 });
         $('#editLightOk').velocity('transition.fadeOut');
         SdwCommon.loadStop();
      });
   },

   lightSave: function () {
      SdwCommon.loadStart();
      SdwEdit._post('/Edit/LightEdit', { id: $('#editLightId').val(), text: $('#editLightText').val() }, function () {
         $('#editLightOk').velocity('transition.fadeIn').velocity('transition.fadeOut');
         SdwCommon.loadStop();
      });
   },

   lightReIndex: function () {
      SdwCommon.loadStart();
      SdwEdit._post('/Edit/ReIndex', { id: $('#editLightId').val() }, function () {
         $('#editLightOk').velocity('transition.fadeIn').velocity('transition.fadeOut');
         SdwCommon.loadStop();
      });
   },

   truthCreate: function() {
      var versions = [];
      var truthLinks = '';
      SdwCommon.loadStart();
      var lights = SdwEdit._getEditLight();
      var hashId = new Hashids('GodisLove');
      $('#addVersionLinks').find('.switch-input').each(function (i, o) {
         var input = $(o);
         if (input.prop('checked')) {
            versions.push(parseInt(input.data('l')));
         }
      });
      $('#addTruthLinks').find('.switch-input').each(function (i, o) {
         var input = $(o);
         if (input.prop('checked')) {
            if (truthLinks != '') {
               truthLinks += ',';
            }
            truthLinks+= input.data('h');
         }
      });
      SdwEdit._post('/Edit/Love', {
         light: hashId.encode(lights),
         truth: $('#addTruthText').val(),
         versions: hashId.encode(versions),
         truthLinks: truthLinks
      }, function() {
         $('#addTruthText').val('');
         SdwCommon.loadStop();
         SdwEdit._getLinks();
      });
   },

   truthAppend: function() {
      var text = $('#addTruthText').val();
      if (text != '') { text += '\n'; }
      text += $('#addTruthAppendOrder').val() + '|' + $('#addTruthAppendNumber').val() + '|' + $('#addTruthAppendText').val();
      $('#addTruthText').val(text);
   },

   truthEdit: function (id) {
      SdwCommon.loadStart();
      SdwCommon.get('/Edit/TruthEdit', { id: id }, function (html) {
         $('#editArea').html(html).velocity('transition.fadeIn').velocity('scroll', { duration: 300 });
         $('#editLightOk').velocity('transition.fadeOut');
         $('#editTruthOk').velocity('transition.fadeOut');
         SdwCommon.loadStop();
      });
   },

   truthRemove: function (id) {
      SdwCommon.loadStart();
      SdwEdit._post('/Edit/TruthRemove', { id: id }, function() {
         $('#currentTruth' + id).velocity('transition.fadeOut').remove();
         SdwCommon.loadStop();
      });
   },

   truthSave: function () {
      SdwCommon.loadStart();
      SdwEdit._post('/Edit/TruthEdit', {
         id: $('#editTruthId').val(),
         order: $('#editTruthOrder').val(),
         number: $('#editTruthNumber').val()
      }, function () {
         $('#editTruthOk').velocity('transition.fadeIn').velocity('transition.fadeOut');
         SdwCommon.loadStop();
      });
   },

   truthIndex: function() {
      $('#footerIndex').text(-parseInt($('#editLightText').get_selection().start));
   },

   truthAddAsTruth: function (id) {
      var lights = SdwEdit._getEditLight();
      var hashId = new Hashids('GodisLove');
      SdwEdit._post('/Edit/TruthAddTruth', {
         id:id,
         light: hashId.encode(lights)
      }, function () {
         $('#editTruthOk').velocity('transition.fadeIn').velocity('transition.fadeOut');
         SdwCommon.loadStop();
      });
   },

   loveAddAsTruth: function (toTruth) {
      var links = SdwEdit._getLinkLight();
      var lights = SdwEdit._getEditLight();
      var hashId = new Hashids('GodisLove');
      SdwEdit._post('/Edit/TruthAddLove', {
         link: hashId.encode(links),
         light: hashId.encode(lights),
         toTruth: toTruth == 1
      }, function () {
         $('#editTruthOk').velocity('transition.fadeIn').velocity('transition.fadeOut');
         SdwCommon.loadStop();
      });
   },

   truthFormat: function () {
      //var uuid = _uuid();
      SdwCommon.loadStart();
      var regex = $('#addTruthFormatRegex').val();
      SdwEdit._post('/Edit/Format', {
         regex: encodeURIComponent(regex),
         text: encodeURIComponent($('#addTruthFormatText').val()),
         startOrder: $('#addTruthFormatOrder').val(),
         startNumber: $('#addTruthFormatNumber').val()
      }, function (d) {
         if ($("#addTruthCurrentRegex option[value='" + d.regexId + "']").length <= 0) {
            $('#addTruthCurrentRegex').append($("<option></option>").attr("value", d.regexId).text(decodeURIComponent(d.regexText)));
         }
         $('#addTruthText').val(d.items);
         $('#addTruthText').velocity('scroll', { duration: 300 });
         SdwCommon.loadStop();
      });
   },

   getIndexes: function () {
      var sel = $('#editLightText').get_selection();
      $('#editStyleStartIndex').val(sel.start);
      $('#editStyleEndIndex').val(sel.end);
   },

   styleAdd: function () {
      SdwCommon.loadStart();
      SdwEdit._post('/Edit/TruthAddStyle', {
         id: $('#editTruthId').val(),
         startIndex: $('#editStyleStartIndex').val(),
         endIndex: $('#editStyleEndIndex').val(),
         start: encodeURIComponent($('#editStyleStart').val()),
         end: encodeURIComponent($('#editStyleEnd').val())
      }, function () {
         SdwEdit.truthEdit($('#editTruthId').val());
         SdwCommon.loadStop();
      });
   },

   styleRemove: function (id) {
      SdwCommon.loadStart();
      SdwEdit._post('/Edit/TruthRemoveStyle', { id: $('#editTruthId').val(), sId: id }, function () {
         $('#style' + id).remove();
         SdwCommon.loadStop();
      });
   },

   _getLinkLight: function () {
      var lights = [];
      $('#linkTruths').html('Loading...');
      $('#linkArea').find('.column').each(function (i, o) {
         var light = $(o);
         lights.push(parseInt(light.attr('id').substr(9)));
      });
      return lights;
   },

   _getEditLight: function () {
      var lights = [];
      $('#editLights').find('.column').each(function (i, o) {
         var light = $(o);
         lights.push(parseInt(light.attr('id').substr(9)));
      });
      return lights;
   },

   _getLinks: function () {
      var lights = SdwEdit._getEditLight();
      var hashId = new Hashids('GodisLove');
      $('#currentTruth').html('Loading...');
      $('#addTruthLinks').html('Loading...');
      $('#addVersionLinks').html('Loading...');
      if (lights.length > 0) {
         var hash = hashId.encode(lights);
         SdwCommon.get('/Edit/TruthLinks', { lights: hash }, function(d) {
            $('#addTruthLinks').html(d).velocity('transition.fadeIn');
         });
         SdwCommon.get('/Edit/VersionLinks', { lights: hash }, function(d) {
            $('#addVersionLinks').html(d).velocity('transition.fadeIn');
         });
         SdwCommon.get('/Edit/Truths', { lights: hash }, function(d) {
            $('#currentTruth').html(d).velocity('transition.fadeIn');
         });
      } else {
         $('#currentTruth').html('');
         $('#addTruthLinks').html('');
         $('#addVersionLinks').html('');
      }
   },

   _getLinkTruths: function () {
      var lights = SdwEdit._getLinkLight();
      var hashId = new Hashids('GodisLove');
      if (lights.length > 0) {
         $('#addLove').show();
         $('#addLoveTruth').show();
         var hash = hashId.encode(lights);
         SdwCommon.get('/Edit/Truths', { lights: hash, link: true }, function(d) {
            $('#linkTruths').html(d).velocity('transition.fadeIn');
         });
      } else {
         $('#editArea').html();
         $('#addLove').hide();
         $('#addLoveTruth').hide();
         $('#linkTruths').html('');
      }
   },

   _uuid:function() {
      return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
         var r = Math.random() * 16 | 0, v = c == 'x' ? r : (r & 0x3 | 0x8);
         return v.toString(16);
      });
   },

   _post: function (url, data, success) {
      $.ajax({ type: 'POST', url: "http://" + location.host + url, data: data }).done(function (d) {
         var ok = true;
         if (d.status) {
            if (d.status == 'fail') {
               SdwCommon.loadStop();
               alert(d.message);
               ok = false;
            }
         }
         if (ok && success) {
            success(d);
         }
      }).fail(function (d) {
         SdwCommon.loadStop();
         if (d.message) {
            alert(d.message);
         }
      });
   },

};
$(document).ready(function () {
   SdwCommon.loadStop();
   SdwEdit.init();
});