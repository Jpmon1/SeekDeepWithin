
function select_word(start,end) {
   if (end === undefined) {
      $("#textRangeSlider").noUiSlider({
         start: start
      }, true);
      select_text(start);
   } else {
      $("#textRangeSlider").noUiSlider({
         start: [start, end]
      }, true);
      $('#startIndex').val(start);
      $('#endIndex').val(end);
      $('#footerIndex').val(end);
      select_text(start, end);
   }
}

function select_text(start, end) {
   if (end === undefined) {
      var text = $('#itemText').html().replace('<span style="background-color:#A0D3E8;">*</span>', '').replace(/^\s+|\s+$/g, '');
      var html = text.substring(0, start);
      html += '<span style="background-color:#A0D3E8;">*</span>';
      html += text.substring(start);
      $('#itemText').html(html);
   } else {
      var len = $('#totalLength').val();
      for (var a = 0; a < len; a++) {
         if (a >= start && a < end) {
            $('#index_' + a).css('background-color', '#A0D3E8');
         } else {
            $('#index_' + a).css('background-color', '');
         }
      }
   }
}

function select_initSlider(end) {
   $("#textRangeSlider").noUiSlider({
      start: 0,
      step: 1,
      behaviour: 'tap',
      range: {
         'min': 0,
         'max': end
      },
      format: wNumb({
         decimals: 0
      })
   }).on({
      slide: function () {
         var val = $('#textRangeSlider').val();
         select_text(val);
      },
      set: function () {
         var val = $('#textRangeSlider').val();
         select_text(val);
      }
   });
   $("#textRangeSlider").Link('lower').to($('#hfIndex'));
}

function select_initRangeSlider(end) {
   $("#textRangeSlider").noUiSlider({
      start: [0, end],
      behaviour: 'drag-tap',
      connect: true,
      step: 1,
      range: {
         'min': 0,
         'max': end
      },
      format: wNumb({
         decimals: 0
      })
   }).on({
      slide: function() {
         var vals = $('#textRangeSlider').val();
         select_text(vals[0], vals[1]);
      },
      set: function () {
         var vals = $('#textRangeSlider').val();
         select_text(vals[0], vals[1]);
      }
   });
   $("#textRangeSlider").Link('lower').to($('#startIndex'));
   $("#textRangeSlider").Link('upper').to($('#endIndex'));
   $("#textRangeSlider").Link('upper').to($('#footerIndex'));
   select_text(0, end);
}