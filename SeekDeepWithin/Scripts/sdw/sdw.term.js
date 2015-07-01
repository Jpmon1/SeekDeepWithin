$(document).ready(function () {
   $('#termTagList').autocomplete({
      serviceUrl: '/Term/AutoComplete',
      paramName: 'term',
      onSelect: function (suggestion) {
         $('#selTermTagId').val(suggestion.data);
      }
   });
   $('#termSeeAlsoList').autocomplete({
      serviceUrl: '/Term/AutoComplete',
      paramName: 'term',
      onSelect: function (suggestion) {
         $('#selTermSeeAlsoId').val(suggestion.data);
      }
   });
   $('#termWriterList').autocomplete({
      serviceUrl: '/Term/AutoComplete',
      paramName: 'term',
      onSelect: function (suggestion) {
         $('#selTermWriterId').val(suggestion.data);
      }
   });
});

function term_create() {
   sdw_post('/Term/Create/', {
      name: $('#termName').val()
   }, 'Creating Term, please wait...', function () {
      $('#termName').val('');
   });
}

function term_edit() {
   sdw_post('/Term/Edit/', {
      id: $('#termId').val(),
      name: $('#termName').val()
   }, 'Editing Term, please wait...');
}

function term_link(action, id) {
   sdw_post('/Term/' + action + '/', {
      id: $('#termId').val(),
      linkId: id
   }, 'Adding Link, please wait...', function (d) {
      if (action.indexOf('Remove') === 0) {
         $('#' + action + '_' + id).remove();
      } else {
         var type = action.substring(3);
         $('#term' + type + 'List').val('');
         $('#selTerm' + type + 'Id').val('');
         $('#current' + type).append('<div class="row" id="Remove' + type + '_' + d.id + '">' +
                     '<div class="small-8 medium-9 large-10 columns">' +
                        '<a href="/Term/' + d.refId + '/" class="sdw-button white expand">' +
                           d.name +
                        '</a>' +
                     '</div>' +
                     '<div class="small-4 medium-3 large-2 columns">' +
                        '<a href="javascript:void(0);" onclick="term_link(\'Remove' + type +
                           '\', ' + d.id + ');" title="Remove ' + type + '" class="sdw-button err small expand">' +
                           '<i class="icon-trash"></i>' +
                        '</a>' +
                     '</div>' +
                  '</div>');
      }
   });
}

function term_addTag() {
   term_link('AddTag', $('#selTermTagId').val());
}

function term_addSeeAlso() {
   term_link('AddSeeAlso', $('#selTermSeeAlsoId').val());
}

function term_addWriter() {
   term_link('AddWriter', $('#selTermWriterId').val());
}
