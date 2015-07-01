function link_setup() {
   $('#linkTerm').autocomplete({
      serviceUrl: '/Term/AutoComplete',
      paramName: 'term',
      onSelect: function (suggestion) {
         $('#selTermId').val(suggestion.data);
      }
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
         if ($('#itemType').val() == 'SeeAlso') {
            $('#linkName').val(suggestion.value);
         }
      }
   });
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

   $('#linkTypeCombo').change(function () {
      var selected = $("#linkTypeCombo").val();
      $('#termArea').hide();
      $('#readArea').hide();
      $('#otherArea').hide();
      $('#anchorArea').show();
      $('#linkNewWindow').prop('checked', false);
      if (selected == 1) {
         $('#termArea').show();
      } else if (selected == 2) {
         $('#readArea').show();
      } else {
         $('#otherArea').show();
         $('#anchorArea').hide();
         $('#linkNewWindow').prop('checked', true);
      }
   });
}

function link_create() {
   var selected = $("#linkTypeCombo").val();
   if (selected == 1) {
      link_createGlossary();
   } else if (selected == 2) {
      link_createRead();
   } else if (selected == 3) {
      link_createSearch();
   } else {
      link_post($('#linkOther').val());
   }
}

function link_createGlossary() {
   var termId = $('#selTermId').val();
   if (termId == '') {
      alert('Select a term to link to first.');
   } else {
      var linkUrl = window.location.protocol + "//" + window.location.host + "/Term/" + termId;
      link_post(linkUrl, function () {
         $('#linkTerm').val('');
         $('#selTermId').val('');
      });
   }
}

function link_createRead() {
   var chapterId = $('#selChapterId').val();
   if (chapterId == '') {
      alert('Select a chapter to link to first.');
   } else {
      var linkUrl = window.location.protocol + "//" + window.location.host + "/Read/" + chapterId;
      link_post(linkUrl, function () {
         $('#linkChapter').val('');
         $('#selChapterId').val('');
      });
   }
}

function link_createSearch() {
   var searchFor = $('#linkOther').val();
   if (searchFor == '') {
      alert('Specify the search first.');
   } else {
      var linkUrl = window.location.protocol + "//" + window.location.host +
         "/Search/Results?searchFor=" + encodeURIComponent(searchFor);
      link_post(linkUrl, function () {
         $('#linkOther').val('');
      });
   }
}

function link_post(linkUrl, done) {
   var anchor = $('#linkAnchor').val();
   if (anchor.length > 0) {
      linkUrl += '#' + anchor;
   }
   sdw_post('/Link/Create' + $('#editItemType').val(), {
      footerId: $('#editItemFooterId').val(),
      itemId: $('#editItemId').val(),
      startIndex: $('#startIndex').val(),
      endIndex: $('#endIndex').val(),
      linkUrl: linkUrl,
      openInNewWindow: $('#linkNewWindow').prop('checked')
   }, 'Creating Link, please wait...', function (d) {
      $('#currentLinks').append('<a href="javascript:void(0)" class="sdw-button white small expand" onclick="link_edit(' + d.id +
         ')" id="link_' + d.id + '">Start: ' + d.startIndex + ' End: ' + d.endIndex + ' (' + d.linkUrl + ')</a>');
      link_new();
      if (done !== undefined) {
         done(d);
      }
   });
}

function link_edit(id) {
   $.ajax({
      url: '/Link/Get' + $('#editItemType').val(),
      data: {
         id: id,
         itemId: $('#editItemId').val(),
         footerId: $('#editItemFooterId').val()
      }
   }).done(function(data) {
      $('#linkCreate').hide();
      $('#linkUpdate').show();
      $('#linkEditId').val(id);
      $('#linkNewWindow').prop('checked', data.openInNewWindow);
      $('#linkOpen').attr('href', data.url);
      select_word(data.startIndex, data.endIndex);
   });
}

function link_update() {
   sdw_post('/Link/Update' + $('#editItemType').val(), {
      id: $('#linkEditId').val(),
      footerId: $('#editItemFooterId').val(),
      itemId: $('#editItemId').val(),
      startIndex: $('#startIndex').val(),
      endIndex: $('#endIndex').val(),
      openInNewWindow: $('#linkNewWindow').prop('checked')
   }, 'Updating Link, please wait...', function (d) {
      $('#link_' + d.id).text('Start: ' + d.startIndex + ' End: ' + d.endIndex +
         ' (' + d.linkUrl + ')');
      link_new();
   });
}

function link_delete() {
   sdw_post('/Link/Delete' + $('#editItemType').val(), {
      id: $('#linkEditId').val(),
      footerId: $('#editItemFooterId').val(),
      itemId: $('#editItemId').val(),
   }, 'Deleting Link, please wait...', function () {
      var id = $('#linkEditId').val();
      $('#link_' + id).remove();
      link_new();
   });
}

function link_new() {
   $('#linkCreate').show();
   $('#linkUpdate').hide();
   $('#linkEditId').val('');
}
