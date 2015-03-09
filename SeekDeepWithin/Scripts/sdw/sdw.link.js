
$(document).ready(function () {
   $('#saveLinkCheck').hide();
   $('#linkGlossary').autocomplete({
      serviceUrl: '/Glossary/AutoComplete',
      paramName: 'term'
   });
   $('#linkBook').autocomplete({
      serviceUrl: '/Book/AutoComplete',
      paramName: 'title',
      onSelect: function (suggestion) {
         $('#selBookId').val(suggestion.data);
         $('#linkVersion').prop('disabled', false);
      }
   });
   $('#linkVersion').autocomplete({
      lookup: function (query, done) {
         $.ajax({
            type: 'POST',
            url: '/Version/AutoComplete/',
            data: {
               title: query,
               bookId: $('#selBookId').val()
            }
         }).done(function (data) {
            done(data);
         }).fail(function (data) {
            alert(data.responseText);
         });
      },
      onSelect: function (suggestion) {
         $('#selVersionId').val(suggestion.data);
         $('#linkSubBook').prop('disabled', false);
      }
   });
   $('#linkSubBook').autocomplete({
      lookup: function (query, done) {
         $.ajax({
            type: 'POST',
            url: '/SubBook/AutoComplete/',
            data: {
               name: query,
               versionId: $('#selVersionId').val()
            }
         }).done(function (data) {
            done(data);
         }).fail(function (data) {
            alert(data.responseText);
         });
      },
      onSelect: function (suggestion) {
         $('#selSubBookId').val(suggestion.data);
         $('#linkChapter').prop('disabled', false);
      }
   });
   $('#linkChapter').autocomplete({
      lookup: function (query, done) {
         $.ajax({
            type: 'POST',
            url: '/Chapter/AutoComplete/',
            data: {
               name: query,
               subBookId: $('#selSubBookId').val()
            }
         }).done(function (data) {
            done(data);
         }).fail(function (data) {
            alert(data.responseText);
         });
      },
      onSelect: function (suggestion) {
         $('#selChapterId').val(suggestion.data);
      }
   });
   $('#text').keyup(setSelection);
   $('#text').mouseup(setSelection);
   $('#linkVersion').prop('disabled', true);
   $('#linkSubBook').prop('disabled', true);
   $('#linkChapter').prop('disabled', true);

   $('#linkBook').change(function () {
      if ($('#linkBook').val().replace(/^\s+|\s+$/g, '') === '') {
         $('#linkVersion').prop('disabled', true);
      }
   });
   $('#linkVersion').change(function () {
      if ($('#linkVersion').val().replace(/^\s+|\s+$/g, '') === '') {
         $('#linkSubBook').prop('disabled', true);
      }
   });
   $('#linkSubBook').change(function () {
      if ($('#linkSubBook').val().replace(/^\s+|\s+$/g, '') === '') {
         $('#linkChapter').prop('disabled', true);
      }
   });
});

function link_Create() {
   var startIndex = $('#StartIndex').val();
   var endIndex = $('#EndIndex').val();
   if (startIndex != '' && endIndex != '') {
      var type = $('#LinkType').val();
      var form = $('#__AjaxAntiForgeryForm');
      var token = $('input[name="__RequestVerificationToken"]', form).val();
      var itemId;
      if (type == 'Entry') {
         itemId = $('#editEntryId').text();
      } else if (type == 'SeeAlso') {
         itemId = $('#TermId').val();
      } else {
         itemId = $('#editPassId').text();
      }
      $.ajax({
         type: 'POST',
         url: '/Link/Create' + type + '/',
         data: {
            __RequestVerificationToken: token,
            startIndex: startIndex,
            endIndex: endIndex,
            linkItemId: itemId,
            glossaryTerm: $('#linkGlossary').val(),
            anchor: $('#linkAnchor').val(),
            book: $('#linkBook').val(),
            version: $('#linkVersion').val(),
            subBook: $('#linkSubBook').val(),
            chapter: $('#linkChapter').val(),
            search: $('#linkSearch').val(),
            link: $('#linkExternal').val(),
            chapterId: $('#selChapterId').val(),
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
