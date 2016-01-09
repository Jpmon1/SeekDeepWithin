var SdwEdit = {

   init: function () {
      $('#editTruthSuccess').velocity('transition.fadeOut');
      /*$('#textNewLove').autocomplete({
         serviceUrl: '/Light/AutoComplete',
         paramName: 'text',
         onSelect: function(suggestion) {
            $('#textNewLove').val(suggestion.value);
            $('#textNewLove').data('lightId', suggestion.data);
         }
      });*/
   },

   lightCreate: function() {
      SdwEdit._post('/Light/Create',
         { text: $('#textNewLight').val() },
         function() {
            $('#textNewLight').val('');
         });
   },

   truthCreate: function() {
      SdwEdit._post('/Truth/Create',
      {
         light: $('#selLight').val(),
         truth: $('#loveType').val() + '|' + $('#truthOrder').val() + '|' + $('#truthNumber').val() + '|' + $('#textNewLove').val()
         },
         function() {
            $('#textNewLove').val('');
            $('#textNewLove').data('lightId', '');
         });
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

   truthNew: function() {
      var uuid = _uuid();
      $.ajax({
         url: "/Truth/New",
         data: { id: uuid }
      }).done(function (d) {
         $('#formattedTruth').append(d);
         var truth = $('#' + uuid);
         truth.velocity("transition.slideRightIn");
         var text = truth.find('#text');
         text.autocomplete({
            serviceUrl: '/Light/AutoComplete',
            paramName: 'text',
            onSelect: function (suggestion) {
               text.val(suggestion.value);
            }
         });
      }).fail(function (d) {
         alert(d.message);
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
         if (d.status == 'fail') {
            alert(d.message);
         } else {
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