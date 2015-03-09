$(document).ready(function () {
   $('#singleAddCheck').hide();
});

function addSingle() {
   postNewEntry($('#Text').val(), $('#Order').val(), function () {
      $('#singleAddCheck').show(200, function () {
         setTimeout(function () { $('#singleAddCheck').hide(100); }, 2000);
      });
      $('#Text').val('');
      $('#Order').val(parseInt($('#Order').val()) + 1);
   });
}

function addMultiple(i) {
   var itemOrder = $('#addOrder' + i);
   if (itemOrder.length > 0) {
      var itemText = $('#addText' + i);
      postNewEntry(itemText.val(), itemOrder.val(), function () {
         $('#multiAddCheck' + i + '').show();
         addMultiple(parseInt(i) + 1);
      });
   } else {
      window.location = '/Glossary/Term/' + $('#TermId').val();
   }
}

function postNewEntry(text, order, complete) {
   var form = $('#__AjaxAntiForgeryForm');
   var token = $('input[name="__RequestVerificationToken"]', form).val();
   $.ajax({
      type: 'POST',
      url: '/Glossary/Add/',
      data: {
         __RequestVerificationToken: token,
         text: text,
         parentId: $('#ParentId').val(),
         Order: order
      }
   }).done(complete).fail(function (data) {
      alert(data.responseText);
   });
}