var SdwEdit = {

   init: function () {
      $('#editTruthSuccess').velocity('transition.fadeOut');
      $('#addTruthAppendText').autocomplete({
         serviceUrl: '/Light/AutoComplete',
         paramName: 'text',
         onSelect: function(suggestion) {
            $('#addTruthAppendText').val(suggestion.value);
            $('#addTruthAppendText').data('lightId', suggestion.data);
         }
      });
      $('#addTruthFormatRegex').val($("#addTruthCurrentRegex option:selected").text());
      $('#addTruthCurrentRegex').change(function() {
         $('#addTruthFormatRegex').val($("#addTruthCurrentRegex option:selected").text());
         //var str = "";
         //$("addTruthCurrentRegex option:selected").each(function () {
         //   str += $(this).text() + " ";
         //});
      });
   },

   lightCreate: function() {
      SdwEdit._post('/Light/Create',
         { text: $('#textNewLight').val() },
         function() {
            $('#textNewLight').val('');
         });
   },

   truthCreate: function () {
      var lights = [];
      var hashId = new Hashids('GodisLove');
      $('#editSelectedLight').find('.callout').each(function(i, o) {
         lights.push(parseInt($(o).attr('id').substr(9)));
      });
      SdwEdit._post('/Truth/Create', {
         light: hashId.encode(lights),
         truth: $('#addTruthText').val()
      }, function() {
         $('#addTruthText').val('');
      });
   },

   truthAppend: function () {
      var text = $('#addTruthText').val() + '\n';
      text += $('#addTruthAppendType').val() + '|' + $('#addTruthAppendOrder').val() +
              '|' + $('#addTruthAppendNumber').val() + '|' + $('#addTruthAppendText').val();
      $('#addTruthText').val(text);
   },

   truthEdit: function (last) {
      if (last) {
         var tId = last.data('tid');
         if (tId !== undefined) {
            SdwCommon.get('/Truth/Get', { id: tId }, function (d) {
               $('#editTruthId').val(d.id);
               $('#editTruthType').val(d.type);
               $('#editTruthText').val(d.text);
               $('#editTruthOrder').val(d.order);
               $('#editTruthNumber').val(d.number);
            });
         }
         var id = last.attr('id').substr(6);
         if (last.hasClass('selected')) {
            $('#editSelectedLight').append('<div class="small-12 medium-4 large-3 column end"><div class="callout" id="editLight' + id + '">' + last.html() + '</div></div>');
         } else {
            $('#editLight' + id).remove();
         }
      } else {
         SdwEdit._post('/Truth/Edit', {
            id: $('#editTruthId').val(),
            type: $('#editTruthType').val(),
            text: $('#editTruthText').val(),
            order: $('#editTruthOrder').val(),
            number: $('#editTruthNumber').val(),
            all: $('#editTruthAll').prop('checked')
         }, function () {
            $('#editTruthSuccess').velocity('transition.fadeIn').velocity('transition.fadeOut');
         });
      }
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
               ok = false;
               alert(d.message);
            }
         }
         if (ok && success) {
            success(d);
         }
      }).fail(function (d) {
         alert(d.message);
      });
   }

};
$(document).foundation();
$(document).ready(function () {
   SdwEdit.init();
});