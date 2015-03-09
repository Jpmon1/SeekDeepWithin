
function footer_Create() {
   var type = $('#hfFor').val();
   $.ajax({
      type: 'POST',
      url: '/Footer/Create' + type + '/',
      data: footer_GetData()
   }).done(function (data) {
      if ($('#hfFor').val() == 'passage') {
         editEntry($('#editEntryId').text());
      } else if ($('#hfFor').val() == 'chapter') {
         $('#chapterFooters').append(data);
      }
      $('#modal').foundation('reveal', 'close');
   }).fail(function (data) {
      alert(data.responseText);
   });
}

function footer_Edit(id) {
   $.ajax({
      type: 'POST',
      url: '/Footer/Edit/',
      data: footer_GetData(id)
   }).done(function (data) {
      if (data.type == 'passage') {
         editEntry($('#editEntryId').text());
      } else if (data.type == 'chapter') {
         $('#chFooter_' + data.id).text(data.text);
      }
      $('#modal').foundation('reveal', 'close');
   }).fail(function (data) {
      alert(data.responseText);
   });
}

function footer_GetData() {
   var form = $('#__AjaxAntiForgeryForm');
   var token = $('input[name="__RequestVerificationToken"]', form).val();
   return {
      __RequestVerificationToken: token,
      id: $('#hfId').val(),
      for: $('#hfFor').val(),
      text: $('#hfText').val(),
      itemId: $('#hfItemId').val(),
      index: $('#hfEditIndex').val(),
      isBold: $('#hfIsBold').prop('checked'),
      isItalic: $('#hfIsItalic').prop('checked'),
      justify: $("#hfJustify option:selected").val()
   };
}

function footer_ShowCreate(id, index, type) {
   $('#modal').foundation('reveal', 'open', {
      url: '/Footer/Create',
      data: { itemId: id, index: index, type: type },
      success: function (data) {
         $('#modal').html(data);
      },
      error: function (data) {
         alert(data.responseText);
      }
   });
}

function footer_CreateChapter() {
   var chapterId = $('#chapterId').val();
   if (chapterId != '') {
      footer_ShowCreate(chapterId, -1, 'chapter');
   } else {
      alert('Unable to determine the chapter!?!?!');
   }
}

function footer_CreatePassage() {
   var entryId = $('#editEntryId').text();
   if (entryId != '') {
      var index = $('#FooterIndex').val();
      if (index != '' && index != -1) {
         footer_ShowCreate(entryId, index, 'passage');
      } else {
         alert('Please specify an index for the footer.');
      }
   } else {
      alert('Please selected a passage to add a footer to.');
   }
}

function footer_CreateEntry() {
   var entryId = $('#editEntryId').text();
   if (entryId != '') {
      var index = $('#FooterIndex').val();
      if (index != '' && index != -1) {
         footer_ShowCreate(entryId, index, 'entry');
      } else {
         alert('Please specify an index for the footer.');
      }
   } else {
      alert('Please selected an entry to add a footer to.');
   }
}

function footer_ShowEdit(id, type) {
   $('#modal').foundation('reveal', 'open', {
      url: '/Footer/Edit',
      data: { id: id, type: type },
      success: function (data) {
         $('#modal').html(data);
      },
      error: function (data) {
         alert(data.responseText);
      }
   });
}
