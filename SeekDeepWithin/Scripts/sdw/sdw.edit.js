var SdwEdit = {

   init: function () {
      $('#editTruthOk').velocity('transition.fadeOut');
      $('#addTruthAppendText').autocomplete({
         serviceUrl: '/Light/AutoComplete',
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
      SdwEdit._post('/Light/Create',
         { text: $('#textNewLight').val() },
         function() {
            $('#textNewLight').val('');
         });
   },

   truthCreate: function() {
      var lights = [];
      var hashId = new Hashids('GodisLove');
      $('#editLights').find('.callout').each(function (i, o) {
         var callout = $(o);
         lights.push(parseInt(callout.attr('id').substr(9)));
         lights.push(parseInt(callout.find('select').first().val()));
      });
      SdwEdit._post('/Truth/Create', {
         light: hashId.encode(lights),
         truth: $('#addTruthText').val()
      }, function() {
         $('#addTruthText').val('');
      });
   },

   truthAppend: function() {
      var text = $('#addTruthText').val();
      if (text != '') { text += '\n'; }
      text += $('#addTruthAppendType').val() + '|' + $('#addTruthAppendOrder').val() +
              '|' + $('#addTruthAppendNumber').val() + '|' + $('#addTruthAppendText').val();
      $('#addTruthText').val(text);
   },

   truthAdd: function (id) {
      var edit = $('#editLight' + id);
      if (edit.length <= 0) {
         SdwCommon.get('/Light/Edit', { id: id }, function (d) { $('#editLights').append(d).velocity('transition.fadeIn'); });
      } else {
         edit.remove();
      }
   },

   truthEdit: function (tId) {
      if (tId) {
         SdwCommon.get('/Truth/Get', { id: tId }, function(d) {
            $('#editTruthId').val(d.id);
            $('#editTruthType').val(d.type);
            $('#editTruthText').val(d.text);
            $('#editTruthOrder').val(d.order);
            $('#editTruthNumber').val(d.number);
         });
      } else {
         $('#editTruthId').val('');
         $('#editTruthType').val('');
         $('#editTruthText').val('');
         $('#editTruthOrder').val('');
         $('#editTruthNumber').val('');
      }
   },

   truthSave: function () {
      SdwEdit._post('/Truth/Edit', {
         id: $('#editTruthId').val(),
         type: $('#editTruthType').val(),
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
      SdwEdit._post('/Truth/Format', {
         regex: encodeURIComponent(regex),
         text: encodeURIComponent($('#addTruthFormatText').val()),
         type: $('#addTruthFormatType').val(),
         startOrder: $('#addTruthFormatOrder').val()
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