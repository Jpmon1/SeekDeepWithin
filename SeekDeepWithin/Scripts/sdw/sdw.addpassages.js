$(document).ready(function () {
   $('#singleAddCheck').hide();
});

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
      window.location = '/Chapter/Read/' + $('#ParentId').val();
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
         parentId: $('#ParentId').val()
      }
   }).done(complete).fail(function (data) {
      alert(data.responseText);
   });
}