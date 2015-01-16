
$(document).ready(function() {
   $('#saveStyleCheck').hide();
});

function style_Create() {
   var startIndex = $('#StartIndex').val();
   var endIndex = $('#EndIndex').val();
   if (startIndex != '' && endIndex != '') {
      var form = $('#__AjaxAntiForgeryForm');
      var token = $('input[name="__RequestVerificationToken"]', form).val();
      $.ajax({
         type: 'POST',
         url: '/Style/Create/',
         data: {
            __RequestVerificationToken: token,
            startIndex: startIndex,
            endIndex: endIndex,
            startStyle: $('#styleStart').val(),
            endStyle: $('#styleEnd').val(),
            parentId: $('#editEntryId').text()
         }
      }).done(function () {
         $('#styleStart').val('');
         $('#styleEnd').val('');
         $('#saveStyleCheck').show(200, function () {
            setTimeout(function () { $('#saveStyleCheck').hide(100); }, 2000);
         });
      }).fail(function (data) {
         alert(data.responseText);
      });
   } else {
      alert('Please select where in the text you would like the style.');
   }
}

function style_Bold() {
   $('#styleStart').val('<strong>');
   $('#styleEnd').val('</strong>');
}

function style_Italic() {
   $('#styleStart').val('<em>');
   $('#styleEnd').val('</em>');
}

function style_BlockQuote() {
   $('#styleStart').val('<blockquote>');
   $('#styleEnd').val('</blockquote>');
}
