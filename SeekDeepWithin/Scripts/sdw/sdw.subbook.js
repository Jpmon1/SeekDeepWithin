$(document).ready(function() {
   $('#abbrevSaved').hide();
   $('#writerSaved').hide();
});

function assignWriter() {
   var form = $('#__AjaxAntiForgeryForm');
   var token = $('input[name="__RequestVerificationToken"]', form).val();
   $.ajax({
      type: 'POST',
      url: '/SubBook/AssignWriter/',
      data: {
         __RequestVerificationToken: token,
         writerId: $('#writerId').val(),
         subBookId: $('#subBookId').val(),
         isTranslator: $('#isTranslator').prop("checked")
      }
   }).done(function (d) {
      if ($('#noWriters').length > 0)
         $('#noWriters').remove();
      $('#writers').append('<div class="row" id="abbrev_' + d.id + '">' +
         '<div class="small-2 large-1 columns text-right">' +
         '<a href="javascript:void(0)" onclick="removeWriter(' + d.subBookId + ', ' + d.writerId + ')" class="button alert tiny" title="Remove">' +
         '<i class="icon-remove"></i></a></div><div class="small-10 large-11 columns">' + d.writer + '</div></div>');
      $('#writerSaved').show(200, function () {
         setTimeout(function () { $('#writerSaved').hide(100); }, 2000);
      });
   }).fail(function (d) {
      alert(d.responseText);
   });
}

function removeWriter(subBookId, writerId) {
   var form = $('#__AjaxAntiForgeryForm');
   var token = $('input[name="__RequestVerificationToken"]', form).val();
   $.ajax({
      type: 'POST',
      url: '/SubBook/RemoveWriter/',
      data: {
         __RequestVerificationToken: token,
         subBookId: subBookId,
         writerId: writerId
      }
   }).done(function () {
      $('#writer_' + writerId).remove();
   }).fail(function (d) {
      alert(d.responseText);
   });
}

function addAbbreviation() {
   var text = $('#abbreviation').val();
   var abbreviations = text.split(';');
   var form = $('#__AjaxAntiForgeryForm');
   var token = $('input[name="__RequestVerificationToken"]', form).val();
   postAbbreviations(abbreviations, token);
}

function postAbbreviations(abbreviations, token) {
   if (abbreviations.length > 0) {
      var abbreviation = abbreviations[0];
      $.ajax({
         type: 'POST',
         url: '/SubBook/AddAbbreviation/',
         data: {
            __RequestVerificationToken: token,
            text: abbreviation,
            subBookId: $('#subBookId').val()
         }
      }).done(function (d) {
         $('#abbreviation').val('');
         if ($('#noAbbreviations').length > 0)
            $('#noAbbreviations').remove();
         $('#abbreviations').append('<div class="row" id="abbrev_' + d.id + '">' +
            '<div class="small-2 large-1 columns text-right">' +
            '<a href="javascript:void(0)" onclick="removeAbbreviation(' + d.id + ')" class="button alert tiny" title="Remove">' +
            '<i class="icon-remove"></i></a></div><div class="small-10 large-11 columns">' + d.text + '</div></div>');
         $('#abbrevSaved').show(200, function () {
            setTimeout(function () { $('#abbrevSaved').hide(100); }, 2000);
         });
         abbreviations.splice(0, 1);
         postAbbreviations(abbreviations, token);
      }).fail(function (d) {
         alert(d.responseText);
      });
   }
}

function removeAbbreviation(id) {
   var form = $('#__AjaxAntiForgeryForm');
   var token = $('input[name="__RequestVerificationToken"]', form).val();
   $.ajax({
      type: 'POST',
      url: '/SubBook/RemoveAbbreviation/',
      data: {
         __RequestVerificationToken: token,
         id: id
      }
   }).done(function () {
      $('#abbrev_' + id).remove();
   }).fail(function (d) {
      alert(d.responseText);
   });
}
