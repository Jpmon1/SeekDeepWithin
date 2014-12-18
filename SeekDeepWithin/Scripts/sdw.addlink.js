$(document).ready(function() {
   $('#GlossaryTerm').autocomplete({
      source: '/Glossary/AutoComplete',
      data: $('#GlossaryTerm').val(),
      success: function(data) {
         alert(data);
      }
   });
   $('#passageText').keyup(setSelection);
   $('#passageText').mouseup(setSelection);
});

function setSelection(event) {
   var jItem = $(event.target);
   var selData = jItem.get_selection();
   var start = selData.start;
   var end = selData.end;
   var selection = jItem.val().substring(start, end);
   if (selection.charAt(0) === ' ')
      start++;
   if (selection.charAt(selection.length - 1) === ' ')
      end--;
   selection = selection.replace(/^\s+|\s+$/g, '');//.trim (); - Trim not supported by IE
   $('#StartIndex').val(start);
   $('#EndIndex').val(end);
   /*var link = selection.toLowerCase().replace(/\b[a-z]/g, function (letter) {
      return letter.toUpperCase();
   });
   if (link === 'Me') {
      link = 'God';
   }*/
   //$('#inAddNewGlossaryLink').val(link);
}