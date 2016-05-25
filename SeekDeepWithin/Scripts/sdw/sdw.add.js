var sdwAdd = {
   
   init: function () {
      sdwEditCommon._initAC($('#txtNewLight'));
      $('#addTruthFormatRegex').val($("#addTruthCurrentRegex option:selected").text());
      $('#addTruthCurrentRegex').change(function () {
         $('#addTruthFormatRegex').val($("#addTruthCurrentRegex option:selected").text());
      });
      $('#btnFormat').click(function (e) {
         e.preventDefault();
         sdwAdd.truthFormat();
      });
   },

   truthFormat: function () {
      var regex = $('#addTruthFormatRegex').val();
      sdwEditCommon._post('/Add/Format', {
         regex: encodeURIComponent(regex),
         text: encodeURIComponent($('#addTruthFormatText').val()),
         startOrder: $('#addTruthFormatOrder').val(),
         startNumber: $('#addTruthFormatNumber').val()
      }, function (d) {
         if ($("#addTruthCurrentRegex option[value='" + d.regexId + "']").length <= 0) {
            $('#addTruthCurrentRegex').append($("<option></option>").attr("value", d.regexId).text(decodeURIComponent(d.regexText)));
         }
         $('#addTruthText').val(JSON.stringify(d.items));
      });
   },

};
$(document).ready(function () {
   sdwCommon.loadStop();
   sdwAdd.init();
});
