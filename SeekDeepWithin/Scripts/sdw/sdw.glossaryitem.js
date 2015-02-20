
$(document).ready(function () {
   $('#saveCheck').hide();
   $('#entryText').keyup(setSelection);
   $('#entryText').mouseup(setSelection);
});

function editEntry(id) {
   $('#rightMenu').children().not('#headersLabel, #addHeaderBtn, #footersLabel, #addFooterBtn, #endLabel').remove();
   $.ajax({
      type: 'GET',
      url: '/Glossary/GetEntry/' + id
   }).done(function (data) {
      $('#entryText').val(data.text);
      $('#LinkItemId').val(data.entryId);
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
         ', \'entry\');"><i class="icon-notificationbottom"></i> ' + text.substr(0, len) + '</a><li>');
   } else if (type == 'footer') {
      $('#endLabel').before('<li><a href="#" onclick="footer_ShowEdit(' + id +
         ', \'entry\');"><i class="icon-notificationtop"></i> ' + text.substr(0, len) + '</a><li>');
   }
}

function saveEntry() {
   var form = $('#__AjaxAntiForgeryForm');
   var token = $('input[name="__RequestVerificationToken"]', form).val();
   $.ajax({
      type: 'POST',
      url: '/Glossary/Edit/',
      data: {
         __RequestVerificationToken: token,
         text: $('#entryText').val(),
         Id: $('#editEntryId').text()
      }
   }).done(function () {
      $('#saveCheck').show(200, function () {
         setTimeout(function () { $('#saveCheck').hide(100); }, 2000);
      });
   }).fail(function (data) {
      alert(data.responseText);
   });
}