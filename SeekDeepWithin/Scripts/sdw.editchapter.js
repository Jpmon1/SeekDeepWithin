$(document).ready(function () {
   $('#saveCheck').hide();
   $('#passText').keyup(setSelection);
   $('#passText').mouseup(setSelection);
});

function editEntry(id) {
   $('#rightPassageMenu').children().not('#headersLabel, #addHeaderBtn, #footersLabel, #addFooterBtn, #endLabel').remove();
   $.ajax({
      type: 'GET',
      url: '/Passage/GetEntry/' + id
   }).done(function (data) {
      $('#passText').val(data.passageText);
      $('#editPassNum').text(data.passageNumber);
      $('#LinkItemId').val(data.passageId);
      $('#editPassId').text(data.passageId);
      $('#editEntryId').text(data.entryId);
      $.each(data.headers, function (i, val) {
         addDisplay('header', val.text);
      });
      $.each(data.footers, function (i, val) {
         addDisplay('footer', val.text);
      });
   }).fail(function (data) {
      alert(data.responseText);
   });
}

function addDisplay(type, text) {
   if (type == 'header') {
      $('#footersLabel').before('<li><a href="#"><i class="icon-notificationbottom"></i> ' + text + '</a><li>');
   } else if (type == 'footer') {
      $('#endLabel').before('<li><a href="#"><i class="icon-notificationtop"></i> ' + text + '</a><li>');
   }
}

function ajaxFail(data) {
   alert(data.responseText);
}

function successCreateLink() {
   $('#GlossaryTerm').val('');
   $('#Book').val('');
   $('#Version').val('');
   $('#SubBook').val('');
   $('#Chapter').val('');
   $('#Link').val('');
   $('#Anchor').val('');
   $('#StartIndex').val('');
   $('#EndIndex').val('');
}

function successCreateHeader() {
   editEntry($('#editEntryId').text());
   $('#addHeaderModal').foundation('reveal', 'close');
}

function successCreateFooter() {
   editEntry($('#editEntryId').text());
   $('#addFooterModal').foundation('reveal', 'close');
}

function addChapterHeader() {
   var chapterId = $('#chapterId').val();
   if (chapterId != '') {
      addHeader(chapterId, 'chapter');
   } else {
      alert('Unable to determine the chapter!?!?!');
   }
}

function addPassageHeader() {
   var entryId = $('#editEntryId').text();
   if (entryId != '') {
      addHeader(entryId, 'passage');
   } else {
      alert('Please selected a passage to add a header to.');
   }
}

function addHeader(id, type) {
   $('#modal').foundation('reveal', 'open', {
      url: '/Header/Create',
      data: { itemId: id, type: type },
      success: function (data) {
         $('#modal').html(data);
      },
      error: function (data) {
         alert(data.responseText);
      }
   });
}

function addChapterFooter() {
   var chapterId = $('#chapterId').val();
   if (chapterId != '') {
      addFooter(chapterId, -1, 'chapter');
   } else {
      alert('Unable to determine the chapter!?!?!');
   }
}

function addPassageFooter() {
   var entryId = $('#editEntryId').text();
   if (entryId != '') {
      var index = $('#FooterIndex').val();
      if (index != '') {
         addFooter(entryId, index, 'passage');
      } else {
         alert('Please specify an index for the footer.');
      }
   } else {
      alert('Please selected a passage to add a footer to.');
   }
}

function addFooter(id, index, type) {
   $('#modal').foundation('reveal', 'open', {
      url: '/Footer/Create',
      data: { itemId: id, index: index, type: type },
      success: function(data) {
         $('#modal').html(data);
      },
      error: function(data) {
         alert(data.responseText);
      }
   });
}

function savePassage() {
   var form = $('#__AjaxAntiForgeryForm');
   var token = $('input[name="__RequestVerificationToken"]', form).val();
   $.ajax({
      type: 'POST',
      url: '/Passage/Edit/',
      data: {
         __RequestVerificationToken: token,
         text: $('#passText').val(),
         Id: $('#editPassId').text()
      }
   }).done(function() {
      $('#saveCheck').show(200, function () {
         setTimeout(function () { $('#saveCheck').hide(100); }, 2000);
      });
   }).fail(function (data) {
      alert(data.responseText);
   });
}