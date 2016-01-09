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
         alert(d.message);
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

if (!Array.prototype.diff) {
   //var diff = $(old_array).not(new_array).get();
   //diff now contains what was in old_array that is not in new_array
   Array.prototype.diff = function(a) {
      return this.filter(function(i) { return a.indexOf(i) < 0; });
   };
}

if (!String.format) {
   String.format = function (format) {
      var args = Array.prototype.slice.call(arguments, 1);
      return format.replace(/{(\d+)}/g, function (match, number) {
         return typeof args[number] != 'undefined'
           ? args[number]
           : match
         ;
      });
   };
}
