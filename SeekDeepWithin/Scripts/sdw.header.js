
function header_Create() {
   var type = $('#hfFor').val();
   $.ajax({
      type: 'POST',
      url: '/Header/Create' + type + '/',
      data: header_GetData()
   }).done(function (data) {
      if ($('#hfFor').val() == 'passage') {
         editEntry($('#editEntryId').text());
      } else if ($('#hfFor').val() == 'chapter') {
         $('#chapterHeaders').append(data);
      }
      $('#modal').foundation('reveal', 'close');
   }).fail(function (data) {
      alert(data.responseText);
   });
}

function header_Edit(id) {
   $.ajax({
      type: 'POST',
      url: '/Header/Edit/',
      data: header_GetData(id)
   }).done(function (data) {
      if (data.type == 'passage') {
         editEntry($('#editEntryId').text());
      } else if (data.type == 'chapter') {
         $('#chHeader_' + data.id).text(data.text);
      }
      $('#modal').foundation('reveal', 'close');
   }).fail(function (data) {
      alert(data.responseText);
   });
}

function header_GetData(id) {
   var form = $('#__AjaxAntiForgeryForm');
   var token = $('input[name="__RequestVerificationToken"]', form).val();
   return {
      __RequestVerificationToken: token,
      id: id,
      for: $('#hfFor').val(),
      text: $('#hfText').val(),
      itemId: $('#hfItemId').val(),
      index: $('#hfEditIndex').val(),
      isBold: $('#hfIsBold').prop('checked'),
      isItalic: $('#hfIsItalic').prop('checked'),
      justify: $("#hfJustify option:selected").val()
   };
}

function header_ShowCreate(id, type) {
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

function header_CreateChapter() {
   var chapterId = $('#chapterId').val();
   if (chapterId != '') {
      header_ShowCreate(chapterId, 'chapter');
   } else {
      alert('Unable to determine the chapter!?!?!');
   }
}

function header_CreatePassage() {
   var entryId = $('#editEntryId').text();
   if (entryId != '') {
      header_ShowCreate(entryId, 'passage');
   } else {
      alert('Please selected a passage to add a header to.');
   }
}

function header_ShowEdit(id, type) {
   $('#modal').foundation('reveal', 'open', {
      url: '/Header/Edit',
      data: { id: id, type: type },
      success: function (data) {
         $('#modal').html(data);
      },
      error: function (data) {
         alert(data.responseText);
      }
   });
}
