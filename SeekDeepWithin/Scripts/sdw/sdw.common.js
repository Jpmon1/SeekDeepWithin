var SdwCommon = {

   getParam: function(name) {
      return decodeURIComponent((new RegExp('[?|&]' + name + '=' + '([^&;]+?)(&|#|;|$)').exec(location.search) || [, ""])[1].replace(/\+/g, '%20')) || null;
   },

   get: function (url, data, success) {
      SdwCommon.loadStart();
      setTimeout(function() {
         $.ajax({ url: "http://" + location.host + url, data: data }).done(function(d) {
            var ok = true;
            if (d.status) {
               if (d.status == 'fail') {
                  SdwCommon.loadStop(true);
                  console.log(url + ': ' + JSON.stringify(d));
                  ok = false;
               }
            }
            if (ok && success) {
               success(d);
            }
            SdwCommon.loadStop();
         }).fail(function(d) {
            SdwCommon.loadStop(true);
            console.log(url + ': ' + JSON.stringify(d));
         });
      }, 300);
   },

   loadStart: function () {
      $('.loader').text('LOADING...').css({ 'background-color': '#78b823' })
         .show().velocity({left: 0},{duration:0}).velocity({ left: "80%" }, {
         duration: 2000, loop: true,
         easing: [0.750, 0.000, 0.500, 1.000]
      });
   },

   loadStop: function (err) {
      if (err) {
         $('.loader').css({ 'background-color': 'darkred' }).velocity("stop").text('ERROR!');
         setTimeout(function () { $('.loader').hide(); }, 1000);
      } else {
         $('.loader').velocity("stop").text('COMPLETE!');
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
