$(document).ready(function () {
   $('#termList').autocomplete({
      serviceUrl: '/Term/AutoComplete',
      paramName: 'term',
      onSelect: function (suggestion) {
         $('#selTermId').val(suggestion.data);
      }
   });
});

function subbook_edit() {
   sdw_post('/SubBook/Edit/', {
      id: $('#subBookId').val(),
      visible: $('#subBookVisi').prop('checked'),
      alias: $('#subBookAlias').val(),
      termId: $('#selTermId').val()
   }, 'Editing Sub book, please wait...');
}

function subbook_create_chapters() {
   sdw_post('/SubBook/CreateChapters/', {
      id: $('#subBookId').val(),
      list: $('#chapterList').val()
   }, 'Creating Chapters, please wait...', function() {
      location.reload();
   });
}

function subbook_delete_chapter(id) {
   sdw_post('/Chapter/Delete/', { id: id }, 'Deleting Chapter, please wait...', function() {
      $('#chapter_' + id).remove();
   }, 'Deleting Data');
}

function subbook_range() {
   var text = '';
   var title = $('#chapterRangeTitle').val();
   var start = parseInt($('#chapterRangeStart').val());
   var end = parseInt($('#chapterRangeEnd').val());
   for (var a = start; a <= end; a++) {
      text += title + a;
      if (a != end) {
         text += '\n';
      }
   }
   $('#chapterList').val(text);
}

function subbook_create_passages() {
   sdw_post('/SubBook/AddPassages/', {
      id: $('#vSubBookId').val(),
      text: $('#passText').val(),
      regex: encodeURIComponent($('#regexToPass').val())
   }, 'Adding Passages, please wait...', function() {
      $('#passText').val('');
   });
}

function subbook_add_abbrev() {
   var text = $('#subBookAbbrev').val();
   var abbreviations = text.split(';');
   subbook_post_abbrev(abbreviations);
}

function subbook_post_abbrev(abbrevs) {
   if (abbrevs.length > 0) {
      var abbrev = abbrevs[0];
      sdw_post('/SubBook/AddAbbreviation/', {
         abbrev: abbrev,
         id: $('#subBookId').val()
      }, 'Adding Abbreviations, please wait...', function() {
         abbrevs.splice(0, 1);
         subbook_post_abbrev(abbrevs);
      });
   } else {
      window.location.reload();
   }
}

function subbook_del_abbrev(abbrev) {
   sdw_post('/SubBook/RemoveAbbreviation', { abbrev: abbrev },
      'Removing the Abbreviation, please wait...', function() {
      $('#abbrev_' + abbrev).remove();
   });
}
