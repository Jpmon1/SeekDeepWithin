
function header_create() {
   $('#modalClose').hide();
   $('#modalText').text('Saving Header, please wait...');
   $('#modal').foundation('reveal', 'open');
   header_post('Create', function (d) {
      $('#modalText').text('Success!');
      setTimeout(function () { $('#modal').foundation('reveal', 'close'); }, 700);
      $('#headerList').append('<li id="item_' + d.id + '" class="bullet-item"><a href="javascript:void(0)" onclick="header_edit(' + d.id +
         ')">' + $('#hfText').val() + '</a></li>');
      header_new();
   });
}

function header_edit(id) {
   $.ajax({
      url: '/Header/Get/',
      data: {
         id: id,
         itemId: $('#itemId').val(),
         itemType: $('#itemType').val()
      }
   }).done(function (data) {
      $('#editId').val(id);
      $('#rowCreate').hide();
      $('#rowUpdate').show();
      $('#rowEdit').show();
      $('#hfText').val(data.text);
      $('#hfJustify').val(data.justify);
      $('#hfIsBold').prop('checked', data.isBold);
      $('#hfIsItalic').prop('checked', data.isItalic);
   }).fail(function (data) {
      $('#modalClose').show();
      $('#modalText').text(data.responseText);
      $('#modal').foundation('reveal', 'open');
   });
}

function header_update() {
   $('#modalClose').hide();
   $('#modalText').text('Updating Header, please wait...');
   $('#modal').foundation('reveal', 'open');
   header_post('Update', function () {
      $('#modalText').text('Success!');
      setTimeout(function () { $('#modal').foundation('reveal', 'close'); }, 700);
   });
}

function header_post(action, done) {
   $.ajax({
      type: 'POST',
      url: '/Header/' + action + '/',
      data: header_GetData()
   }).done(done).fail(function (data) {
      $('#modalClose').show();
      $('#modalText').text('An error occured - ' + data.responseText);
   });
}

function header_delete() {
   $('#modalClose').hide();
   $('#modalText').text('Deleting Header, please wait...');
   $('#modal').foundation('reveal', 'open');
   $.ajax({
      type: 'POST',
      url: '/Header/Delete/',
      data: header_GetData()
   }).done(function () {
      var id = $('#editId').val();
      $('#item_' + id).remove();
      $('#modalText').text('Success!');
      header_new();
      setTimeout(function () { $('#modal').foundation('reveal', 'close'); }, 700);
   }).fail(function (data) {
      $('#modalClose').show();
      $('#modalText').text('An error occured - ' + data.responseText);
   });
}

function header_style() {
   window.location = '/Style/EditHeader?id=' + $('#editId').val() + '&itemId=' + $('#itemId').val() + '&itemType=' + $('#itemType').val();
}

function header_GetData() {
   var form = $('#__AjaxAntiForgeryForm');
   var token = $('input[name="__RequestVerificationToken"]', form).val();
   return {
      __RequestVerificationToken: token,
      index: 0,
      id: $('#editId').val(),
      text: $('#hfText').val(),
      itemId: $('#itemId').val(),
      itemType: $('#itemType').val(),
      isBold: $('#hfIsBold').prop('checked'),
      isItalic: $('#hfIsItalic').prop('checked'),
      justify: $("#hfJustify option:selected").val()
   };
}

function header_new() {
   $('#rowCreate').show();
   $('#rowUpdate').hide();
   $('#rowHide').hide();
   $('#hfText').val('');
   $('#editId').val('');
   $('#hfJustify').val(0);
   $('#styleArea').html('');
   $('#hfIsBold').prop('checked', false);
   $('#hfIsItalic').prop('checked', false);
}
