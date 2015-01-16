﻿
$(document).ready(function () {
   $('#saveLinkCheck').hide();
   $('#linkGlossary').autocomplete({
      serviceUrl: '/Glossary/AutoComplete',
      paramName: 'term'
   });
});

function link_Create() {
   var startIndex = $('#StartIndex').val();
   var endIndex = $('#EndIndex').val();
   if (startIndex != '' && endIndex != '') {
      var form = $('#__AjaxAntiForgeryForm');
      var token = $('input[name="__RequestVerificationToken"]', form).val();
      $.ajax({
         type: 'POST',
         url: '/Link/CreatePassage/',
         data: {
            __RequestVerificationToken: token,
            startIndex: startIndex,
            endIndex: endIndex,
            linkItemId: $('#editPassId').text(),
            glossaryTerm: $('#linkGlossary').val(),
            anchor: $('#linkAnchor').val(),
            book: $('#linkBook').val(),
            version: $('#linkVersion').val(),
            subBook: $('#linkSubBook').val(),
            chapter: $('#linkChapter').val(),
            search: $('#linkSearch').val(),
            link: $('#linkExternal').val(),
            openInNewWindow: $('#linkNewWindow').prop('checked')
         }
      }).done(function () {
         $('#linkGlossary').val('');
         $('#linkAnchor').val('');
         $('#linkBook').val('');
         $('#linkVersion').val('');
         $('#linkSubBook').val('');
         $('#linkChapter').val('');
         $('#linkSearch').val('');
         $('#linkExternal').val('');
         $('#saveLinkCheck').show(200, function () {
            setTimeout(function () { $('#saveLinkCheck').hide(100); }, 2000);
         });
      }).fail(function (data) {
         alert(data.responseText);
      });
   } else {
      alert('Please select where in the text you would like the link.');
   }
}
