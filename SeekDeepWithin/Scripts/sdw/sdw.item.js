$(document).ready(function () {
   $('#termList').autocomplete({
      serviceUrl: '/Term/AutoComplete',
      paramName: 'term',
      onSelect: function (suggestion) {
         $('#selTermId').val(suggestion.data);
      }
   });
   $('#sourceList').autocomplete({
      serviceUrl: '/TermItem/AutoComplete',
      paramName: 'source',
      onSelect: function (suggestion) {
         $('#selSourceId').val(suggestion.data);
      }
   });
});

function item_create() {
   sdw_post('/TermItem/Create/', {
      termId: $('#termId').val(),
      sourceId: $('#selSourceId').val()
   }, 'Creating Item, please wait...', function () {
      $('#selSourceId').val('');
      $('#sourceList').val('');
   });
}

function item_source_create() {
   sdw_post('/TermItem/CreateSource/', {
      name: $('#sourceName').val(),
      url: $('#sourceUrl').val(),
      termId: $('#selTermId').val()
   }, 'Creating Source, please wait...', function() {
      $('#sourceName').val('');
      $('#sourceUrl').val('');
      $('#selTermId').val('');
      $('#termList').val('');
   });
}
