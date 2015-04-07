
function footer_create() {
   $('#modalClose').hide();
   $('#modalText').text('Saving Footer, please wait...');
   $('#modal').foundation('reveal', 'open');
   footer_post('Create', function (d) {
      $('#modalText').text('Success!');
      setTimeout(function () { $('#modal').foundation('reveal', 'close'); }, 700);
      $('#footerList').append('<li id="item_' + d.id + '" class="bullet-item"><a href="javascript:void(0)" onclick="footer_edit(' + d.id +
         ')">' + d.index + ' - ' + $('#hfText').val() + '</a></li>');
      footer_new();
   });
}

function footer_edit(id) {
   $.ajax({
      url: '/Footer/Get/',
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
      $('#hfIndex').val(data.index);
      $('#hfJustify').val(data.justify);
      $('#hfIsBold').prop('checked', data.isBold);
      $('#hfIsItalic').prop('checked', data.isItalic);
   }).fail(function (data) {
      $('#modalClose').show();
      $('#modalText').text('An error occured - ' + data.responseText);
   });
}

function footer_update() {
   $('#modalClose').hide();
   $('#modalText').text('Updating Footer, please wait...');
   $('#modal').foundation('reveal', 'open');
   footer_post('Update', function () {
      $('#modalText').text('Success!');
      setTimeout(function () { $('#modal').foundation('reveal', 'close'); }, 700);
   });
}

function footer_post(action, done) {
   $.ajax({
      type: 'POST',
      url: '/Footer/' + action + '/',
      data: footer_GetData()
   }).done(done).fail(function (data) {
      $('#modalClose').show();
      $('#modalText').text('An error occured - ' + data.responseText);
   });
}

function footer_delete() {
   $('#modalClose').hide();
   $('#modalText').text('Deleting Footer, please wait...');
   $('#modal').foundation('reveal', 'open');
   $.ajax({
      type: 'POST',
      url: '/Footer/Delete/',
      data: footer_GetData()
   }).done(function () {
      var id = $('#editId').val();
      $('#item_' + id).remove();
      $('#modalText').text('Success!');
      footer_new();
      setTimeout(function () { $('#modal').foundation('reveal', 'close'); }, 700);
   }).fail(function (data) {
      $('#modalClose').show();
      $('#modalText').text('An error occured - ' + data.responseText);
   });
}

function footer_GetData() {
   var form = $('#__AjaxAntiForgeryForm');
   var token = $('input[name="__RequestVerificationToken"]', form).val();
   return {
      __RequestVerificationToken: token,
      id: $('#editId').val(),
      text: $('#hfText').val(),
      itemId: $('#itemId').val(),
      itemType: $('#itemType').val(),
      index: $('#hfIndex').val(),
      isBold: $('#hfIsBold').prop('checked'),
      isItalic: $('#hfIsItalic').prop('checked'),
      justify: $("#hfJustify option:selected").val()
   };
}

function footer_new() {
   $('#rowCreate').show();
   $('#rowUpdate').hide();
   $('#rowEdit').hide();
   $('#hfText').val('');
   $('#editId').val('');
   $('#hfIndex').val(0);
   $('#hfJustify').val(0);
   $('#hfIsBold').prop('checked', false);
   $('#hfIsItalic').prop('checked', false);
}

function footer_style() {
   window.location = '/Style/EditFooter?id=' + $('#editId').val() + '&itemId=' + $('#itemId').val() + '&itemType=' + $('#itemType').val();
}

function footer_link() {
   window.location = '/Link/EditFooter?id=' + $('#editId').val() + '&itemId=' + $('#itemId').val() + '&itemType=' + $('#itemType').val();
}