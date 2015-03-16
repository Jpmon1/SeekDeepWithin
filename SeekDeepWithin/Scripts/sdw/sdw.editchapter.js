
$(document).ready(function () {
   $('#saveCheck').hide();
   $('#saveReadStyleCheck').hide();
   $('#passText').keyup(setSelection);
   $('#passText').mouseup(setSelection);
   $('#verseRadio').change(verseParaChanged);
   $('#paraRadio').change(verseParaChanged);
   $("#textRangeSlider").noUiSlider({
      start: [0, 1],
      behaviour: 'drag-tap',
      connect: true,
      step: 1,
      range: {
         'min': 0,
         'max': 100
      },
      format: wNumb({
         decimals: 0
      })
   }).on({
      slide: function () {
         var text = $('#textSelectArea').text();
         var vals = $('#textRangeSlider').val();
         var html = text.substring(0, vals[0]);
         html += '<span style="background-color:#A0D3E8">';
         html += text.substring(vals[0], vals[1]);
         html += '</span>';
         html += text.substring(vals[1]);
         $('#textSelectArea').html(html);
      }
   });;
   $("#textRangeSlider").Link('lower').to($('#StartIndex'));
   $("#textRangeSlider").Link('upper').to($('#FooterIndex'));
   $("#textRangeSlider").Link('upper').to($('#EndIndex'));
});

function verseParaChanged() {
   var para = $('#paraRadio').prop("checked");
   var form = $('#__AjaxAntiForgeryForm');
   var token = $('input[name="__RequestVerificationToken"]', form).val();
   $.ajax({
      type: 'POST',
      url: '/Chapter/ReadStyle/',
      data: {
         __RequestVerificationToken: token,
         paragraph: para,
         id: $('#chapterId').val()
      }
   }).done(function () {
      $('#saveReadStyleCheck').show(200, function () {
         setTimeout(function () { $('#saveReadStyleCheck').hide(100); }, 2000);
      });
   }).fail(function (data) {
      alert(data.responseText);
   });
}

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
      var len = data.passageText.length;
      $("#textRangeSlider").noUiSlider({
         start: [0, len > 5 ? 5 : len],
         range: {
            'min': 0,
            'max': len
         }
      }, true);
      /*var textSplit = data.passageText.split('/\s+/');
      var html = '';
      for (var a = 0; a < textSplit.length; a++) {
         html += '<span onclick="wordClick()">' + textSplit[a] + '</span> ';
      }*/
      $('#textSelectArea').text(data.passageText);
   }).fail(function (data) {
      alert(data.responseText);
   });
}

function wordClick() {
   alert('Click!');
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