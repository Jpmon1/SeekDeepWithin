sdwEditCommon = {

   _initAC: function (item) {
      item.autocomplete({
         serviceUrl: '/Seek/AutoComplete',
         paramName: 'text',
         noCache: true,
         deferRequestBy: 500,
         triggerSelectOnValidInput: false,
         onSelect: function (suggestion) {
            item.val(suggestion.value);
            item.data('lightId', suggestion.data);
         }
      });
   },

   _post: function (url, data, success) {
      sdwCommon.loadStart();
      setTimeout(function () {
         $.ajax({ type: 'POST', url: 'http://' + location.host + url, data: data }).done(function (d) {
            var ok = true;
            if (d.status) {
               if (d.status == 'fail') {
                  sdwCommon.loadStop(true);
                  console.log(url + ': ' + JSON.stringify(d));
                  alert(d.message);
                  ok = false;
               }
            }
            if (ok && success) {
               success(d);
            }
            sdwCommon.loadStop();
         }).fail(function (d) {
            sdwCommon.loadStop(true);
            console.log(url + ': ' + JSON.stringify(d));
         });
      }, 300);
   },

};
