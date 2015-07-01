$(document).ready(function () {
   $('#termList').autocomplete({
      serviceUrl: '/Term/AutoComplete',
      paramName: 'term',
      onSelect: function (suggestion) {
         $('#selTermId').val(suggestion.data);
      }
   });
});

function book_create() {
   sdw_post('/Book/Create/', {
      title: $('#bookTitle').val(),
      subTitle: $('#bookSubTitle').val(),
      summary: $('#bookSummary').val(),
      termId: $('#selTermId').val()
   }, 'Creating Book, please wait...', function () {
      $('#modal-content-text').text('Success!');
      $('#bookSubTitle').val('');
      $('#bookSummary').val('');
      $('#selTermId').val('');
      $('#bookTitle').val('');
      $('#termList').val('');
   });
}

function book_edit() {
   $('#modal-content-text').text('Editing Book, please wait...');
   var modal = $('.remodal').remodal({ closeOnEscape: false, closeOnAnyClick: false });
   modal.open();
   sdw_post('/Book/Edit/', {
      id: $('#bookId').val(),
      title: $('#bookTitle').val(),
      subTitle: $('#bookSubTitle').val(),
      summary: $('#bookSummary').val(),
      termId: $('#selTermId').val()
   }, 'Editing Book, please wait...');
}


function book_setDefaultVersion(id) {
   $('#modal-content-text').text('Setting default version, please wait...');
   var modal = $('.remodal').remodal({ closeOnEscape: false, closeOnAnyClick: false });
   modal.open();
   sdw_post('/Book/DefaultVersion/', {
      bookId: $('#bookId').val(),
      versionId: id
   }, 'Setting default version, please wait...');
}
