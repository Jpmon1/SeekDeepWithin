$(document).ready(function () {
   $('#termList').autocomplete({
      serviceUrl: '/Term/AutoComplete',
      paramName: 'term',
      onSelect: function (suggestion) {
         $('#selTermId').val(suggestion.data);
      }
   });
});

function version_create() {
   sdw_post('/Version/Create/', {
      bookId: $('#bookId').val(),
      title: $('#versionTitle').val(),
      date: $('#versionDate').val(),
      termId: $('#selTermId').val()
   }, 'Creating Version, please wait...', function() {
      $('#versionDate').val('');
      $('#versionTitle').val('');
      $('#selTermId').val('');
      $('#termList').val('');
   });
}

function version_edit() {
   sdw_post('/Version/Edit/', {
      id: $('#versionId').val(),
      title: $('#versionTitle').val(),
      date: $('#versionDate').val(),
      termId: $('#selTermId').val(),
      sourceName: $('#versionSourceName').val(),
      sourceUrl: $('#versionSourceUrl').val()
   }, 'Editing Version, please wait...');
}

function version_create_subbooks() {
   sdw_post('/Version/CreateSubBooks/', {
      id: $('#versionId').val(),
      list: $('#subBookList').val()
   }, 'Creating Sub Books, please wait...', function () {
      window.location.reload();
   });
}

function version_delete_subbook(id) {
   sdw_post('/SubBook/Delete/', { id: id }, 'Deleting Sub Book, please wait...', function () {
      $('#subBook_' + id).remove();
   }, 'Deleting Data');
}
