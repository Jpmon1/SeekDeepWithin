
$(document).ready(function () {
   $('#tabGlossary').show();
   $('#tabRead').hide();
   $('#tabSearch').hide();
   $('#tabTag').hide();
   $('#tabExternal').hide();
   $('#rowUpdate').hide();
   $('#rowOpenLink').hide();
   $('#createLinkCheck').hide();
   $('#updateLinkCheck').hide();

   $('#linkGlossary').autocomplete({
      serviceUrl: '/Glossary/AutoComplete',
      paramName: 'term',
      onSelect: function (suggestion) {
         $('#selTermId').val(suggestion.data);
         if ($('#itemType').val() == 'SeeAlso') {
            $('#linkName').val(suggestion.value);
         }
      }
   });
   $('#linkTag').autocomplete({
      serviceUrl: '/Tag/AutoComplete',
      paramName: 'tag',
      onSelect: function (suggestion) {
         $('#selTagId').val(suggestion.data);
         if ($('#itemType').val() == 'SeeAlso') {
            $('#linkName').val(suggestion.value);
         }
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
      $('#tabGlossary').hide();
      $('#tabRead').hide();
      $('#tabSearch').hide();
      $('#tabTag').hide();
      $('#tabExternal').hide();
      $('#rowAnchor').show();
      $('#linkNewWindow').prop('checked', false);
      if (selected == 0) {
         $('#tabGlossary').show();
      } else if (selected == 1) {
         $('#tabRead').show();
      } else if (selected == 2) {
         $('#tabSearch').show();
      } else if (selected == 3) {
         $('#tabTag').show();
      } else {
         $('#tabExternal').show();
         $('#rowAnchor').hide();
         $('#linkNewWindow').prop('checked', true);
      }
   });
});

function link_create() {
   var selected = $("#linkTypeCombo").val();
   if (selected == 0) {
      link_createGlossary();
   } else if (selected == 1) {
      link_createRead();
   } else if (selected == 2) {
      link_createSearch();
   } else if (selected == 3) {
      link_createTag();
   } else {
      link_createExternal();
   }
}

function link_createGlossary() {
   var termId = $('#selTermId').val();
   if (termId == '') {
      alert('Select a term to link to first.');
   } else {
      var linkUrl = window.location.protocol + "//" + window.location.host + "/Glossary/Term/" + termId;
      link_post('create', linkUrl, function (d) {
         link_render();
         $('#linkTerm').val('');
         $('#selTermId').val('');
         $('#createLinkCheck').show(200, function () {
            setTimeout(function () { $('#createLinkCheck').hide(100); }, 2000);
         });
         link_add(d.id, d.startIndex, d.endIndex, d.linkUrl);
      });
   }
}

function link_createTag() {
   var tagId = $('#selTagId').val();
   if (tagId == '') {
      alert('Select a tag to link to first.');
   } else {
      var linkUrl = window.location.protocol + "//" + window.location.host + "/Tag/Details/" + tagId;
      link_post('create', linkUrl, function (d) {
         link_render();
         $('#linkTag').val('');
         $('#selTagId').val('');
         $('#createLinkCheck').show(200, function () {
            setTimeout(function () { $('#createLinkCheck').hide(100); }, 2000);
         });
         link_add(d.id, d.startIndex, d.endIndex, d.linkUrl);
      });
   }
}

function link_createRead() {
   var chapterId = $('#selChapterId').val();
   if (chapterId == '') {
      alert('Select a chapter to link to first.');
   } else {
      var linkUrl = window.location.protocol + "//" + window.location.host + "/Chapter/Read/" + chapterId;
      link_post('create', linkUrl, function (d) {
         link_render();
         $('#linkChapter').val('');
         $('#selChapterId').val('');
         $('#createLinkCheck').show(200, function () {
            setTimeout(function () { $('#createLinkCheck').hide(100); }, 2000);
         });
         link_add(d.id, d.startIndex, d.endIndex, d.linkUrl);
      });
   }
}

function link_createSearch() {
   var searchFor = $('#linkSearch').val();
   if (searchFor == '') {
      alert('Specify the search first.');
   } else {
      var linkUrl = window.location.protocol + "//" + window.location.host +
         "/Search/Results?searchFor=" + encodeURIComponent(searchFor);
      link_post('create', linkUrl, function (d) {
         link_render();
         $('#linkSearch').val('');
         $('#createLinkCheck').show(200, function () {
            setTimeout(function () { $('#createLinkCheck').hide(100); }, 2000);
         });
         link_add(d.id, d.startIndex, d.endIndex, d.linkUrl);
      });
   }

}

function link_createExternal() {
   link_post('create', $('#linkExternal').val(), function (d) {
      link_render();
      $('#createLinkCheck').show(200, function () {
         setTimeout(function () { $('#createLinkCheck').hide(100); }, 2000);
      });
      link_add(d.id, d.startIndex, d.endIndex, d.linkUrl);
   });
}

function link_update() {
   link_post('update', '', function () {
      $('#updateLinkCheck').show(200, function () {
         setTimeout(function () { $('#updateLinkCheck').hide(100); }, 2000);
      });
      link_render();
      link_add($('#editId').val(), $('#startIndex').val(), $('#endIndex').val(), '');
   });
}

function link_add(id, start, end, url) {
   if ($('#listItem_' + id).length > 0) {
      if (url === '') {
         var split = $('#listItem_' + id).text().split(' ');
         url = split[split.length - 1].replace('(', '').replace(')', '');
      }
      $('#listItem_' + id).remove();
   }
   $('#linkList').append('<li id="listItem_' + id + '" class="bullet-item"><a href="javascript:void(0)" onclick="link_edit(' + id +
      ')">Start: ' + start + ' End: ' + end + ' (' + url + ')</a></li>');
}

function link_post(action, linkUrl, done) {
   var startIndex = $('#startIndex').val();
   var endIndex = $('#endIndex').val();
   var anchor = $('#linkAnchor').val();
   if (anchor.length > 0)
      linkUrl = linkUrl + '#' + anchor;
   if (startIndex != '' && endIndex != '') {
      var form = $('#__AjaxAntiForgeryForm');
      var token = $('input[name="__RequestVerificationToken"]', form).val();
      $.ajax({
         type: 'POST',
         url: '/Link/' + action + '/',
         data: {
            __RequestVerificationToken: token,
            startIndex: startIndex,
            endIndex: endIndex,
            linkId: $('#editId').val(),
            itemId: $('#itemId').val(),
            itemType: $('#itemType').val(),
            openInNewWindow: $('#linkNewWindow').prop('checked'),
            linkUrl: linkUrl,
            linkName: $('#linkName').val()
   }
      }).done(done).fail(function (data) {
         alert(data.responseText);
      });
   } else {
      alert('Please select where in the text you would like the link.');
   }
}

function link_delete() {
   var form = $('#__AjaxAntiForgeryForm');
   var token = $('input[name="__RequestVerificationToken"]', form).val();
   $.ajax({
      type: 'POST',
      url: '/Link/Delete/',
      data: {
         __RequestVerificationToken: token,
         id: $('#editId').val(),
         itemId: $('#itemId').val(),
         itemType: $('#itemType').val()
      }
   }).done(function () {
      var id = $('#editId').val();
      $('#listItem_' + id).remove();
      link_new();
      link_render();
   }).fail(function (data) {
      alert(data.responseText);
   });
}

function link_edit(itemId) {
   $.ajax({
      url: '/Link/Get/',
      data: {
         itemId: $('#itemId').val(),
         id: itemId,
         itemType: $('#itemType').val()
      }
   }).done(function (data) {
      $('#rowCreate').hide();
      $('#rowUpdate').show();
      $('#rowOpenLink').show();
      $('#editId').val(itemId);
      $('#linkEditArea').hide();
      $('#openLinkArea').html('<a href="' + data.url + '" target="_blank" class="button small expand">Open Link</a>');
      select_word(data.startIndex, data.endIndex);
   }).fail(function (data) {
      alert(data.responseText);
   });
}

function link_render() {
   $.ajax({
      url: '/Link/Render/',
      data: {
         itemId: $('#itemId').val(),
         itemType: $('#itemType').val()
      }
   }).done(function (data) {
      $('#renderedText').html(data.html);
      $('#generatedHtml').text(data.html);
   });
}

function link_new() {
   $('#rowCreate').show();
   $('#rowUpdate').hide();
   $('#rowOpenLink').hide();
   $('#linkEditArea').show();
   $('#linkTypeCombo').val(0);
}
