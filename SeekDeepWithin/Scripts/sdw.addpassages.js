$(document).ready(function () {
   $('#singleAddCheck').hide();
});

function regexRemove() {
   $.ajax({
      type: 'POST',
      url: '/Convert/RemoveRegex/',
      data: {
         text: $('#Text').val(),
         regex: $('#regex').val()
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

function addSingle() {
   postNewPassage($('#Text').val(), $('#Order').val(), $('#Number').val(), function() {
      $('#singleAddCheck').show(200, function() {
         setTimeout(function() { $('#singleAddCheck').hide(100); }, 2000);
      });
      $('#Text').val('');
      $('#Order').val(parseInt($('#Order').val()) + 1);
      $('#Number').val(parseInt($('#Number').val()) + 1);
   });
}

function addMultiple(i) {
   var itemOrder = $('#addOrder' + i);
   if (itemOrder.length > 0) {
      var itemText = $('#addText' + i);
      var itemNumber = $('#addNumber' + i);
      postNewPassage(itemText.val(), itemOrder.val(), itemNumber.val(), function() {
         $('#multiAddCheck' + i + '').show();
         addMultiple(parseInt(i) + 1);
      });
   } else {
      window.location = '/Chapter/Read/' + $('#ChapterId').val();
   }
}

function postNewPassage(text, order, number, complete) {
   var form = $('#__AjaxAntiForgeryForm');
   var token = $('input[name="__RequestVerificationToken"]', form).val();
   $.ajax({
      type: 'POST',
      url: '/Passage/Create/',
      data: {
         __RequestVerificationToken: token,
         text: text,
         Order: order,
         Number: number,
         chapterId: $('#ChapterId').val()
      }
   }).done(complete).fail(function (data) {
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
      $('#singleAdd').hide();
      $('#multiAdd').show();
      var titleHtml = '<div class="row">';
      titleHtml += '<div class="small-6 large-1 columns"><strong>Order</strong></div>';
      titleHtml += '<div class="small-6 large-1 columns"><strong>Number</strong></div>';
      titleHtml += '<div class="small-12 large-10 columns"><strong>Text</strong></div>';
      titleHtml += '</div>';
      $('#multiAdd').append(titleHtml);
      $.each(data.passages, function(i, val) {
         var html = '<div class="row">';
         html += '<div class="small-6 large-1 columns">';
         html += '<input type="number" value="' + val.order + '" id="addOrder' + i + '">';
         html += '</div>';
         html += '<div class="small-6 large-1 columns">';
         html += '<input type="number" value="' + val.number + '" id="addNumber' + i + '">';
         html += '</div>';
         html += '<div class="small-10 large-9 columns">';
         html += '<input type="text" value="' + val.text + '" id="addText' + i + '">';
         html += '</div>';
         html += '<div class="small-2 large-1 columns">';
         html += '<span id="multiAddCheck' + i + '" style="display:none;"><i class="icon-ok-sign" style="color: green"></i></span>';
         html += '</div>';
         html += '</div>';
         $('#multiAdd').append(html);
      });
      titleHtml = '<div class="row">';
      titleHtml += '<div class="small-12 large-12 columns"><a href="#" onclick="addMultiple(0);" class="button">Add All</a></div>';
      titleHtml += '</div>';
      $('#multiAdd').append(titleHtml);
   }).fail(function (data) {
      alert(data.responseText);
   });
}