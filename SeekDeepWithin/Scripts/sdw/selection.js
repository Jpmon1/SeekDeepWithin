(function ($) {

   $.fn.get_selection = function () {
      var e = this.get(0);
      //Mozilla and DOM 3.0
      if ('selectionStart' in e) {
         var l = e.selectionEnd - e.selectionStart;
         return { start: e.selectionStart, end: e.selectionEnd, length: l, text: e.value.substr(e.selectionStart, l) };
      }
      else if (document.selection) {    //IE
         e.focus();
         var r = document.selection.createRange();
         var tr = e.createTextRange();
         var tr2 = tr.duplicate();
         tr2.moveToBookmark(r.getBookmark());
         tr.setEndPoint('EndToStart', tr2);
         var textPart = r.text.replace(/[\r\n]/g, '.'); //for some reason IE doesn't always count the \n and \r in length
         var textWhole = e.value.replace(/[\r\n]/g, '.');
         var theStart = textWhole.indexOf(textPart, tr.text.length);
         return { start: theStart, end: theStart + textPart.length, length: textPart.length, text: r.text };
      }
         //Browser not supported
      else
         return { start: e.value.length, end: e.value.length, length: 0, text: '' };
   };

   $.fn.set_selection = function (startPos, endPos) {
      var e = this.get(0);
      //Mozilla and DOM 3.0
      if ('selectionStart' in e) {
         e.focus();
         e.selectionStart = startPos;
         e.selectionEnd = endPos;
      }
      else if (document.selection) { //IE
         e.focus();
         var tr = e.createTextRange();

         //Fix IE from counting the newline characters as two seperate characters
         var stopIt = startPos;
         for (var i = 0; i < stopIt; i++)
            if (e.value[i].search(/[\r\n]/) != -1)
               startPos = startPos - .5;
         stopIt = endPos;
         for (i = 0; i < stopIt; i++)
            if (e.value[i].search(/[\r\n]/) != -1)
               endPos = endPos - .5;

         tr.moveEnd('textedit', -1);
         tr.moveStart('character', startPos);
         tr.moveEnd('character', endPos - startPos);
         tr.select();
      }
      return this.get_selection();
   };

})(jQuery);

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
   $('#FooterIndex').val(start);
   $('#EndIndex').val(end);
   var link = selection.toLowerCase().replace(/\b[a-z]/g, function (letter) {
      return letter.toUpperCase();
   });
   if (link === 'Me') {
      link = 'God';
   }
   //$('#linkGlossary').val(link);
}
