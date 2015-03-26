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
      postNewPassage(itemText.val(), itemOrder.val(), itemNumber.val(), function () {
         itemText.hide();
         $('#multiAddCheck' + i + '').show();
         if (!isElementInViewport(itemOrder)) {
            $('html, body').animate({
               scrollTop: itemOrder.offset().top
            }, 100);
         }
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
         IsInsert: $('#isInsert').prop('checked'),
         parentId: $('#ParentId').val()
      }
   }).done(complete).fail(function (data) {
      alert(data.responseText);
   });
}

function isElementInViewport(el) {
   //special bonus for those using jQuery
   if (typeof jQuery === "function" && el instanceof jQuery) {
      el = el[0];
   }
   var rect = el.getBoundingClientRect();
   return (
       rect.top >= 0 &&
       rect.left >= 0 &&
       rect.bottom <= (window.innerHeight || document.documentElement.clientHeight) && /*or $(window).height() */
       rect.right <= (window.innerWidth || document.documentElement.clientWidth) /*or $(window).width() */
   );
}