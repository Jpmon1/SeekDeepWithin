var SdwCommon = {

   getParam: function(name) {
      return decodeURIComponent((new RegExp('[?|&]' + name + '=' + '([^&;]+?)(&|#|;|$)').exec(location.search) || [, ""])[1].replace(/\+/g, '%20')) || null;
   },

   escapeHtml: function(str) {
      var div = document.createElement('div');
      div.appendChild(document.createTextNode(str));
      return div.innerHTML;
   },

   get: function(url, data, success) {
      SdwCommon.loadStart();
      $.ajax({ url: url, data: data }).done(function (d) {
         var ok = true;
         if (d.status) {
            if (d.status == 'fail') {
               SdwCommon.loadStop();
               alert(d.message);
               ok = false;
            }
         }
         if (ok && success) {
            success(d);
         }
      }).fail(function(d) {
         SdwCommon.loadStop();
         if (d.message) {
            alert(d.message);
         }
      });
   },

   loadStart: function() {
      $('.loader').css('left', 0).show();
      $('.loader').velocity({ left: "80%" }, {
         duration: 3000, loop: true,
         easing: [0.750, 0.000, 0.500, 1.000]
      });
   },

   loadStop: function() {
      $('.loader').velocity("stop").velocity('transition.fadeOut');
   },

   toRadian: function(angle) {
       return (angle * Math.PI) / 180;
   }

};
