function regexRemove() {
   $.ajax({
      type: 'POST',
      url: '/Convert/RemoveRegex/',
      data: {
         text: $('#Text').val(),
         regex: encodeURIComponent($('#regex').val())
      }
   }).done(function (data) {
      $('#Text').val(data.text);
   }).fail(function (data) {
      alert(data.responseText);
   });
}

function convert(path) {
   $('#multiAdd').empty();
   $.ajax({
      type: 'POST',
      url: '/Convert/' + path + '/',
      data: {
         text: $('#Text').val()
      }
   }).done(function (data) {
      $('#singleAdd').show();
      $('#multiAdd').hide();
      $('#Text').val(data.text);
   }).fail(function (data) {
      alert(data.responseText);
   });
}

function convertToPassages() {
   $('#multiAdd').empty();
   $.ajax({
      type: 'POST',
      url: '/Convert/LinesToPassages/',
      data: {
         text: $('#Text').val(),
         startOrder: $('#Order').val(),
         startNumber: $('#Number').val()
      }
   }).done(function (data) {
      addPassages(data);
   }).fail(function (data) {
      alert(data.responseText);
   });
}

function regexToPassages() {
   $('#multiAdd').empty();
   $.ajax({
      type: 'POST',
      url: '/Convert/RegexToPassages/',
      data: {
         text: $('#Text').val(),
         regex: encodeURIComponent($('#regexToPass').val()),
         startOrder: $('#Order').val(),
         startNumber: $('#Number').val()
      }
   }).done(function (data) {
      addPassages(data);
   }).fail(function (data) {
      alert(data.responseText);
   });
}

function addPassages(data) {
   $('#singleAdd').hide();
   $('#multiAdd').show();
   var titleHtml = '<div class="row">';
   titleHtml += '<div class="small-6 large-1 columns"><strong>Order</strong></div>';
   titleHtml += '<div class="small-6 large-1 columns"><strong>Number</strong></div>';
   titleHtml += '<div class="small-12 large-10 columns"><strong>Text</strong></div>';
   titleHtml += '</div>';
   $('#multiAdd').append(titleHtml);
   $.each(data.passages, function (i, val) {
      var html = '<div class="row">';
      html += '<div class="small-6 large-1 columns">';
      html += '<input type="number" value="' + val.order + '" id="addOrder' + i + '">';
      html += '</div>';
      html += '<div class="small-6 large-1 columns">';
      html += '<input type="number" value="' + val.number + '" id="addNumber' + i + '">';
      html += '</div>';
      html += '<div class="small-12 large-10 columns">';
      html += '<input type="text" value="' + val.text + '" id="addText' + i + '">';
      html += '<span id="multiAddCheck' + i + '" style="display:none;"><i class="icon-ok-sign" style="color: green"></i></span>';
      html += '</div>';
      html += '</div>';
      $('#multiAdd').append(html);
   });
   titleHtml = '<div class="row">';
   titleHtml += '<div class="small-12 large-12 columns">' +
      '<a href="javascript:void(0)" onclick="addMultiple(0);" class="button expand" id="addAllButton">Add All</a>' +
      '</div>';
   titleHtml += '</div>';
   $('#multiAdd').append(titleHtml);

   $('html, body').animate({
      scrollTop: $('#addAllButton').offset().top
   }, 100);
}
