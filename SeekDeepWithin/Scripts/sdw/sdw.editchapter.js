
$(document).ready(function () {
   $('#saveCheck').hide();
   $('#passText').keyup(setSelection);
   $('#passText').mouseup(setSelection);
});

function editEntry(id) {
   $('#rightMenu').children().not('#headersLabel, #addHeaderBtn, #footersLabel, #addFooterBtn, #endLabel').remove();
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
         addDisplay('header', val.text, val.id);
      });
      $.each(data.footers, function (i, val) {
         addDisplay('footer', val.text, val.id);
      });
   }).fail(function (data) {
      alert(data.responseText);
   });
}

function addDisplay(type, text, id) {
   var len = text.length > 25 ? 25 : text.length;
   if (type == 'header') {
      $('#footersLabel').before('<li><a href="#" onclick="header_ShowEdit(' + id +
         ', \'passage\');"><i class="icon-notificationbottom"></i> ' + text.substr(0, len) + '</a><li>');
   } else if (type == 'footer') {
      $('#endLabel').before('<li><a href="#" onclick="footer_ShowEdit(' + id +
         ', \'passage\');"><i class="icon-notificationtop"></i> ' + text.substr(0, len) + '</a><li>');
   }
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