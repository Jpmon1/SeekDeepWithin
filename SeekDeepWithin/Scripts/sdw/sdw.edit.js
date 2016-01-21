var SdwEdit = {

   init: function () {
      $('#editTruthOk').velocity('transition.fadeOut');
      $('#addTruthAppendText').autocomplete({
         serviceUrl: '/Seek/AutoComplete',
         paramName: 'text',
         noCache: true,
         onSelect: function(suggestion) {
            $('#addTruthAppendText').val(suggestion.value);
            $('#addTruthAppendText').data('lightId', suggestion.data);
         }
      });
      $('#addTruthFormatRegex').val($("#addTruthCurrentRegex option:selected").text());
      $('#addTruthCurrentRegex').change(function() {
         $('#addTruthFormatRegex').val($("#addTruthCurrentRegex option:selected").text());
      });
      $('#addTruthAppendText').autocomplete('clearCache');
   },

   lightCreate: function() {
      SdwEdit._post('/Edit/Illuminate', { text: $('#textNewLight').val() }, function (d) {
         $('#textNewLight').val('');
         var container = $('#lightList');
         container.append(d);
         container.masonry('reloadItems');
         container.masonry('layout');
      });
   },

   truthCreate: function() {
      var links = [];
      var lights = [];
      var hashId = new Hashids('GodisLove');
      $('#editLights').find('.callout').each(function (i, o) {
         var callout = $(o);
         lights.push(parseInt(callout.attr('id').substr(9)));
      });
      $('#addTruthLinks').find('.truthLink').each(function (i, o) {
         var input = $(o);
         if (input.prop('checked')) {
            links.push(parseInt(input.data('l')));
         }
      });
      SdwEdit._post('/Edit/Love', {
         links: hashId.encode(links),
         light: hashId.encode(lights),
         truth: $('#addTruthText').val(),
         invert: $('#addTruthInvert').prop('checked')
      }, function() {
         $('#addTruthText').val('');
      });
   },

   truthAppend: function() {
      var text = $('#addTruthText').val();
      if (text != '') { text += '\n'; }
      text += $('#addTruthAppendOrder').val() + '|' + $('#addTruthAppendNumber').val() + '|' + $('#addTruthAppendText').val();
      $('#addTruthText').val(text);
   },

   lightAdd: function (id) {
      var edit = $('#editLight' + id);
      if (edit.length <= 0) {
         SdwCommon.get('/Edit/Light', { id: id }, function(d) {
            $('#editLights').append(d).velocity('transition.fadeIn');
            SdwEdit.getLinks();
         });
      } else {
         edit.remove();
         SdwEdit.getLinks();
      }
   },

   getLinks: function () {
      var lights = [];
      $('#addTruthLinks').html('Loading...');
      var hashId = new Hashids('GodisLove');
      $('#editLights').find('.callout').each(function (i, o) {
         var callout = $(o);
         lights.push(parseInt(callout.attr('id').substr(9)));
      });
      SdwCommon.get('/Edit/Links', { lights: hashId.encode(lights) }, function (d) {
         $('#addTruthLinks').html(d).velocity('transition.fadeIn');
      });
   },

   truthEdit: function (tId) {
      if (tId) {
         SdwCommon.get('/Seek/Truth', { id: tId }, function(d) {
            $('#editTruthId').val(d.id);
            $('#editTruthText').val(d.text);
            $('#editTruthOrder').val(d.order);
            $('#editTruthNumber').val(d.number);
         });
      } else {
         $('#editTruthId').val('');
         $('#editTruthText').val('');
         $('#editTruthOrder').val('');
         $('#editTruthNumber').val('');
      }
   },

   truthSave: function () {
      SdwEdit._post('/Edit/Truth', {
         id: $('#editTruthId').val(),
         text: $('#editTruthText').val(),
         order: $('#editTruthOrder').val(),
         number: $('#editTruthNumber').val(),
         all: $('#editTruthAll').prop('checked')
      }, function () {
         $('#editTruthOk').velocity('transition.fadeIn').velocity('transition.fadeOut');
      });
   },

   truthFormat: function () {
      //var uuid = _uuid();
      var regex = $('#addTruthFormatRegex').val();
      SdwEdit._post('/Edit/Format', {
         regex: encodeURIComponent(regex),
         text: encodeURIComponent($('#addTruthFormatText').val()),
         startOrder: $('#addTruthFormatOrder').val(),
         startNumber: $('#addTruthFormatNumber').val()
      }, function (d) {
         if ($("#addTruthCurrentRegex option[value='" + d.regexId + "']").length <= 0) {
            $('#addTruthCurrentRegex')
            .append($("<option></option>")
            .attr("value", d.regexId)
            .text(decodeURIComponent(d.regexText)));
         }
         $('#addTruthText').val(d.items);
      });
   },

   _uuid:function() {
      return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
         var r = Math.random() * 16 | 0, v = c == 'x' ? r : (r & 0x3 | 0x8);
         return v.toString(16);
      });
   },

   _post: function (url, data, success) {
      $.ajax({ type: 'POST', url: url, data: data }).done(function (d) {
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
   SdwEdit.init();
});