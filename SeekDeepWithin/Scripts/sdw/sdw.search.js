$(document).ready(function () {
   document.passFilters = [];
   document.glossFilters = [];
   /* $(window).scroll(lazyload);
   lazyload();*/
   $('#doPassages').change(function () { $('#passageFilter').toggle(); });
   $('#doGlossary').change(function () { $('#glossaryFilter').toggle(); });
   $('#txtSearch').keypress(function(e) {
      if (e.which == 13) {
         search_search();
      }
   });
   $('#txtSearch').focus();
});

function search_search() {
   /*$('#modalClose').hide();
   $('#modalText').text('Searching, please wait...');
   $('#modal').foundation('reveal', 'open');*/
   $('#searchResults').html('<div class="text-center"><img src="Content/ajax-loader_2.gif"/></div>');
   $.ajax({
      type: 'POST',
      url: '/Search/Query/',
      data: {
         query: encodeURIComponent($('#txtSearch').val()),
         doPassages: $('#doPassages').prop('checked'),
         doGlossary: $('#doGlossary').prop('checked'),
         doTags: $('#doTags').prop('checked'),
         doWriters: $('#doWriters').prop('checked'),
         searchType: $('#searchType').val(),
         exact:$('#chkExact').prop('checked'),
         doGlossHeaders: $('#doGlossHeaders').prop('checked'),
         doGlossFooters: $('#doGlossFooters').prop('checked'),
         doPassHeaders: $('#doPassHeaders').prop('checked'),
         doPassFooters: $('#doPassFooters').prop('checked')
      }
   }).done(function (data) {
      $('#advSearch').hide();
      $('#searchResults').html(data);
      /*$('#modal').foundation('reveal', 'close');*/
      $(document).foundation('equalizer', 'reflow');
   }).fail(function (data) {
      $('#searchResults').html('<div class="text-center"><h3><i class="icon-erroralt" style="color:darkred;"></i><small>' + data.responseText + '</small></h3></div>');
   });
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
