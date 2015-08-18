function index_create() {
   var start = parseInt($('#startIndex').val());
   var length = parseInt($('#lengthIndex').val());
   sdw_post('/Search/CreateIndex/', {
      type: $('#selCreateIndex').val(),
      start: start,
      length: length
   }, 'Creating Index, please wait...', function (d) {
      $('#totalCount').text(d.count + ' total records.');
      if (start != -1) {
         $('#startIndex').val(start + length);
      }
   });
}

function index_optimize() {
   sdw_post('/Search/OptimizeIndex/', {
         type: $('#selOptIndex').val()
      }, 'Optimizing Index, please wait...');
}

function index_delete() {
   sdw_post('/Search/DeleteIndex/', {
         type: $('#selDelIndex').val()
      }, 'Deleting Index, please wait...');
}
