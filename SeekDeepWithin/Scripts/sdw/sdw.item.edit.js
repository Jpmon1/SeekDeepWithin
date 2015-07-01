function sdw_add_remove() {
   $('#regexList').append('<div class="row"><div class="small-9 medium-10 large-11 columns">' +
      '<input type="text" id="removeRegex" />' +
      '</div><div class="small-3 medium-2 large-1 columns">' +
      '<a href="javascript:void(0)" onclick="sdw_del_remove(this);" class="sdw-button err tiny expand">' +
      '<i class="icon-trash"></i></a>' +
      '</div></div>');
}

function sdw_del_remove(b) {
   $(b).closest(".row").remove();
}

function sdw_get_edit(url) {
   $('#editItem').html('<div class="tCenter"><img src="../../Content/ajax-loader_2.gif" alt="Loading..."/></div>');
   $.ajax({
      type: 'GET',
      url: url
   }).done(function (d) {
      $('#editItem').html(d);
      select_initRangeSlider(parseInt($('#totalLength').val()));
      style_setup();
      if ($('#linkArea').length > 0) {
         link_setup();
      }
   });
}

function sdw_item_add() {
   var chapter = $('#chapterOrder').length > 0 ? $('#chapterOrder').val() : undefined;
   if (chapter === undefined) {
      sdw_post('/ItemEntry/Create/', { entryList: $('#formatted').val(), itemId: $('#itemId').val() },
         'Adding Entries, please wait...', function () {

         });
   } else {
      sdw_post('/Passage/Create/', { passageList: $('#formatted').val(), subBookId: $('#subBookId').val() },
         'Adding Passages, please wait...', function() {

         });
   }
}

function sdw_format_item() {
   var order = 1;
   var number = 1;
   var chapter = $('#chapterOrder').length > 0 ? $('#chapterOrder').val() : undefined;
   var text = $('#unformatted').val();
   $('#removeRegex').each(function () {
      var regex = $(this).val();
      if (regex !== '') {
         text = text.replace(new RegExp(regex, "gi"), '');
      }
   });
   var formattedText = '';
   /*[c]Chapter|[n]Number|[o]Order|[t]Text|[h]Header|[f@index]footer|[f@index]footer|...*/
   if ($('#convertItems').prop('checked')) {
      var passages = text.split(/\n/g);
      for (var i = 0; i < passages.length; i++) {
         var passage = passages[i].replace(/^\s+|\s+$/g, '');
         if (passage === '') { continue; }
         if (formattedText !== '') { formattedText += '\n'; }
         if (chapter === undefined) {
            formattedText += String.format("[o]{0}|[t]{1}", order, passage);
         } else {
            formattedText += String.format("[c]{0}|[n]{1}|[o]{2}|[t]{3}", chapter, number, order, passage);
         }
         order++;
         number++;
      }
   } else {
      if ($('#convertSpaces').prop('checked')) {
         text = text.replace(/\n/g, ' ');
         text = text.replace(/\s+/g, ' ');
      }
      if (chapter === undefined) {
         formattedText = String.format("[o]{0}|[t]{1}", order, text);
      } else {
         formattedText = String.format("[c]{0}|[n]{1}|[o]{2}|[t]{3}", chapter, number, order, text);
      }
   }
   $('#formatted').val(formattedText);
   $('#formatted').show();
   $('#formattedAddBtn').show();
}
