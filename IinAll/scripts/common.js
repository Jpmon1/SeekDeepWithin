define(['jquery'], function($){

   var iInAllCommon = {

      getParam: function(name) {
         return decodeURIComponent((new RegExp('[?|&]' + name + '=' + '([^&;]+?)(&|#|;|$)').exec(location.search) || [, ""])[1].replace(/\+/g, '%20')) || '';
      },

      get: function (url, data, success) {
         this.loadStart();
         setTimeout(function() {
            $.ajax({ url: "http://" + location.host + url, data: data }).done(function(d) {
               var ok = true;
               if (d.status) {
                  if (d.status == 'fail') {
                     this.loadStop(true);
                     console.log(url + ': ' + JSON.stringify(d));
                     ok = false;
                  }
               }
               if (ok && success) {
                  success(d);
               }
            }).fail(function(d) {
               this.loadStop(true);
               console.log(url + ': ' + JSON.stringify(d));
            });
         }, 300);
      },

      loadStart: function () {
         $('.loader').text('LOADING...').css({ 'background-color': '#78b823' }).show();
      },

      loadStop: function (err) {
         if (err) {
            $('.loader').css({ 'background-color': 'darkred' }).text('ERROR!');
            setTimeout(function () { $('.loader').hide(); }, 1000);
         } else {
            $('.loader').text('COMPLETE!');
            setTimeout(function () { $('.loader').hide(); }, 500);
         }
      },

      throttle: function (func, delay) {
         var timer = null;
         return function () {
            var context = this, args = arguments;
            if (timer === null) {
               timer = setTimeout(function () {
                  func.apply(context, args);
                  timer = null;
               }, delay);
            }
         };
      }

   };
   return iInAllCommon;
});
