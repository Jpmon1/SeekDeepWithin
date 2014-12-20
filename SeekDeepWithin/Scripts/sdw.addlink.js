$(document).ready(function() {
   $('#GlossaryTerm').autocomplete({
      serviceUrl: '/Glossary/AutoComplete',
      paramName: 'term'
   });
   $('#text').keyup(setSelection);
   $('#text').mouseup(setSelection);
});
