$(document).ready(function() {
   iinallCommon.loadStop();
   window.gettingTruth = false;
   window.reloadTruth = false;
   
   $('#txtRegex').val($("#cmbFormatRegex option:selected").text());
   $('#cmbFormatRegex').change(function() {
      $('#txtRegex').val($("#cmbFormatRegex option:selected").text());
   });
      
   $('#btnCreateLight').click(function() {
      var light = $('#txtCreateLight').val();
      var motto = $('#txtMotto').val();
      if (light != '') {
         $('#btnCreateLight').hide();
         _post('/api/createLight', {l: light, m: motto}, function(d) {
            $('#txtCreateLight').val('');
            $('#btnCreateLight').show();
         });
      }
   });
   
   $('#btnAddLightToLove').click(function() {
      var lightId = $('#txtSearch').data('lightId');
      if ($('#' + lightId).length <= 0) {
         var html = '<div class="small-12 medium-3 large-3 columns end" id="' + lightId + '"><div class="callout secondary small">' + $('#txtSearch').val() + '<a id="btnRemove" class="sdw-button err expand tiny">Remove</a></div></div>';
         var jObj = $(html);
         jObj.find('#btnRemove').each(function(i,o) {
            $(o).click(function(e) {
               $('#' + lightId).remove();
            });
         });;
         $('#divLoveLight').append(jObj);
         getTruth();
      }
   });
   
   $('#btnFormat').click(function() {
      var love = getLove();
      if (love == '') {
      } else {
         var regex = $('#txtRegex').val();
         if (regex.indexOf('/') != 0){
            regex = '/' + regex;
         }
         if (regex.lastIndexOf('/') != regex.length - 1){
            regex = regex + '/';
         }
         var order = $('#txtStartOrder').val();
         var number = $('#txtStartNumber').val();
         _post('/api/format', { l: love, d: $('#txtToFormat').val(), r: regex, o: order, n: number }, function(d) {
            var text = $('#txtFormatted').val();
            if (text != ''){
               text += ',';
            }
            text += JSON.stringify(d.data);
            $('#txtFormatted').val(text);
         });
      }
   });
   
   $('#btnCreateTruth').click(function() {
      var motto = $('#txtMotto').val();
      _post('/api/createTruth', {d: '[' + $('#txtFormatted').val() + ']', m: motto }, function(d) {
         var a = 0;
      });
   });
   
   $('#txtSearch').autocomplete({
      serviceUrl: '/api/suggest',
      paramName: 't',
      noCache: true,
      deferRequestBy: 500,
      triggerSelectOnValidInput: false,
      onSelect: function(suggestion) {
         $('#txtSearch').val(suggestion.value);
         $('#txtSearch').data('lightId', suggestion.data);
      }
   });
});

function getTruth(){
   var love = getLove();
   if (love!= '') {
      if (!window.gettingTruth){
         window.gettingTruth = true;
         iinallCommon.get('/api/truth', {l: love}, function(d){
            window.gettingTruth = false;
            if (window.reloadTruth){
               window.reloadTruth = false;
               getTruth();
            }
         });
      } else {
         window.reloadTruth = true;
      }
   }
   iinallCommon.loadStop();
}

function getLove(){
   var lights = [];
   $('#divLoveLight').find('.columns').each(function (i, o) {
      var light = $(o);
      lights.push(parseInt(light.attr('id')));
   });
   return lights.join();
}

function _post(url, data, success) {
   iinallCommon.loadStart();
   setTimeout(function() {
      $.ajax({ type: 'POST', url: 'http://' + location.host + url, data: data }).done(function(d) {
         var ok = true;
         if (d.status) {
            if (d.status == 'error') {
               iinallCommon.loadStop(true);
               console.log(url + ': ' + JSON.stringify(d));
               alert(d.message);
               ok = false;
            }
         }
         if (ok && success) {
            success(d);
         }
         iinallCommon.loadStop();
      }).fail(function (d) {
         iinallCommon.loadStop(true);
         console.log(url + ': ' + JSON.stringify(d));
      });
   }, 300);
}