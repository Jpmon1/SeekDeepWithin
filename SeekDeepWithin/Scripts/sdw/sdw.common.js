function lazyload() {
   var wt = $(window).scrollTop();    //* top of the window
   var wb = wt + $(window).height();  //* bottom of the window

   $(".lazy").each(function () {
      if (!$(this).attr("loaded")) {
         var ot = $(this).offset().top;  //* top of object
         var ob = ot + $(this).height(); //* bottom of object
         if (wt <= ob && wb >= ot) {
            $(this).html("here goes the iframe definition");
            $(this).attr("loaded", true);
         }
      }
   });
}

// Returns a function, that, as long as it continues to be invoked, will not
// be triggered. The function will be called after it stops being called for
// N milliseconds. If `immediate` is passed, trigger the function on the
// leading edge, instead of the trailing.
function debounce(func, wait, immediate) {
   var timeout;
   return function () {
      var context = this, args = arguments;
      var later = function () {
         timeout = null;
         if (!immediate) func.apply(context, args);
      };
      var callNow = immediate && !timeout;
      clearTimeout(timeout);
      timeout = setTimeout(later, wait);
      if (callNow) func.apply(context, args);
   };
};
