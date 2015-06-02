$(document).ready(function () {
   document.passFilters = [];
   document.glossFilters = [];
   /* $(window).scroll(lazyload);
   lazyload();*/
   $('#doPassages').change(function () { $('#passageFilter').toggle(); });
   $('#doGlossary').change(function () { $('#glossaryFilter').toggle(); });
   $('#txtSearch').keypress(function(e) {
      if (e.which == 13) {
         search_Parse(true);
      }
   });
   $('#txtSearch').focus();
});

function search_Parse(keepOn, page) {
   var doPassages = $('#doPassages').prop('checked');
   if (doPassages) {
      $('#searchLoader').show();
      $.ajax({
         type: 'POST',
         url: '/Search/Parse/',
         data: search_getData(page)
      }).done(function(data) {
         $('#parseResults').html(data);
         $(document).foundation('equalizer', 'reflow');
         if (keepOn) {
            search_Book(keepOn);
         } else {
            $('#searchLoader').hide();
         }
      }).fail(function(data) {
         $('#searchLoader').hide();
         $('#parseResults').html('<div class="text-center"><h3><i class="icon-erroralt" style="color:darkred;"></i><small>' + data.responseText + '</small></h3></div>');
      });
   } else if (keepOn) {
      search_Term(keepOn);
   }
}

function search_Book(keepOn, page) {
   var doPassages = $('#doPassages').prop('checked');
   if (doPassages) {
      $('#searchLoader').show();
      $.ajax({
         type: 'POST',
         url: '/Search/Books/',
         data: search_getData(page)
      }).done(function (data) {
         $('#bookResults').html(data);
         $(document).foundation('equalizer', 'reflow');
         if (keepOn) {
            search_Passage(keepOn);
         } else {
            $('#searchLoader').hide();
         }
      }).fail(function (data) {
         $('#searchLoader').hide();
         $('#parseResults').html('<div class="text-center"><h3><i class="icon-erroralt" style="color:darkred;"></i><small>' + data.responseText + '</small></h3></div>');
      });
   } else if (keepOn) {
      search_Term(keepOn);
   }
}

function search_Passage(keepOn, page) {
   var doPassages = $('#doPassages').prop('checked');
   if (doPassages) {
      $('#searchLoader').show();
      $.ajax({
         type: 'POST',
         url: '/Search/Passages/',
         data: search_getData(page)
      }).done(function (data) {
         $('#passageResults').html(data);
         $(document).foundation('equalizer', 'reflow');
         if (keepOn) {
            search_Term(keepOn);
         } else {
            $('#searchLoader').hide();
         }
      }).fail(function (data) {
         $('#searchLoader').hide();
         $('#parseResults').html('<div class="text-center"><h3><i class="icon-erroralt" style="color:darkred;"></i><small>' + data.responseText + '</small></h3></div>');
      });
   } else if (keepOn) {
      search_Term(keepOn);
   }
}

function search_Term(keepOn, page) {
   var doGlossary = $('#doGlossary').prop('checked');
   if (doGlossary) {
      $('#searchLoader').show();
      $.ajax({
         type: 'POST',
         url: '/Search/Terms/',
         data: search_getData(page)
      }).done(function (data) {
         $('#termResults').html(data);
         $(document).foundation('equalizer', 'reflow');
         if (keepOn) {
            search_Glossary();
         } else {
            $('#searchLoader').hide();
         }
      }).fail(function (data) {
         $('#searchLoader').hide();
         $('#parseResults').html('<div class="text-center"><h3><i class="icon-erroralt" style="color:darkred;"></i><small>' + data.responseText + '</small></h3></div>');
      });
   }
}

function search_Glossary(page) {
   var doGlossary = $('#doGlossary').prop('checked');
   if (doGlossary) {
      $('#searchLoader').show();
      $.ajax({
         type: 'POST',
         url: '/Search/Glossary/',
         data: search_getData(page)
      }).done(function (data) {
         $('#glossaryResults').html(data);
         $(document).foundation('equalizer', 'reflow');
         $('#searchLoader').hide();
      }).fail(function (data) {
         $('#searchLoader').hide();
         $('#parseResults').html('<div class="text-center"><h3><i class="icon-erroralt" style="color:darkred;"></i><small>' + data.responseText + '</small></h3></div>');
      });
   }
}

function search_getData(page) {
   if (page == null) {
      page = 1;
   }
   return {
      query: encodeURIComponent($('#txtSearch').val()),
      searchType: $('#searchType').val(),
      page: page,
      exact: $('#chkExact').prop('checked'),
      doHeaders: $('#doHeaders').prop('checked'),
      doFooters: $('#doFooters').prop('checked')
   }
}

function search_adv() {
   $('#advSearch').toggle();
}

function search_addPassFilter() {
   var id = generateUUID();
   document.passFilters.push(id);
   $('#passFilters').append('<div class="row" id="' + id + '">' +
      '<div class="small-12 medium-4 large-4 columns">' +
         '<select><option>Tagged with</option><option>Not Tagged with</option></select>' +
      '</div>' +
      '<div class="small-8 medium-6 large-6 columns">' +
         '<input type="text" readonly="readonly" />' +
      '</div>' +
      '<div class="small-4 medium-2 large-2 columns">' +
         '<a href="javascript:void(0);" class="button alert small" title="Delete this filter" onclick="search_removeFilter(\'' + id + '\', \'pass\')">' +
            '<i class="icon-remove"></i>' +
         '</a>' +
      '</div>' +
      '</div>');
}

function search_addGlossFilter() {
   var id = generateUUID();
   document.glossFilters.push(id);
   $('#glossFilters').append('<div class="row" id="' + id + '">' +
      '<div class="small-12 medium-4 large-4 columns">' +
         '<select><option>Tagged with</option><option>Not Tagged with</option></select>' +
      '</div>' +
      '<div class="small-8 medium-6 large-6 columns">' +
         '<input type="text" readonly="readonly" />' +
      '</div>' +
      '<div class="small-4 medium-2 large-2 columns">' +
         '<a href="javascript:void(0);" class="button alert small" title="Delete this filter" onclick="search_removeFilter(\'' + id + '\', \'gloss\')">' +
            '<i class="icon-remove"></i>' +
         '</a>' +
      '</div>' +
      '</div>');
}

function search_removeFilter(id, type) {
   if (type === 'gloss') {
      var index = document.glossFilters.indexOf(id);
      document.glossFilters.splice(index);
   } else if (type === 'pass') {
      
   }
   $('#' + id).remove();
}

function generateUUID() {
   var d = performance.now();
   var uuid = 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
      var r = (d + Math.random() * 16) % 16 | 0;
      d = Math.floor(d / 16);
      return (c == 'x' ? r : (r & 0x3 | 0x8)).toString(16);
   });
   return uuid;
};
