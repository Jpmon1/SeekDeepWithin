$(document).ready(function() {
   document.modal = $('.remodal').remodal({ closeOnEscape: false, closeOnAnyClick: false });
});

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

function escapeHtml(str) {
   var div = document.createElement('div');
   div.appendChild(document.createTextNode(str));
   return div.innerHTML;
};

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

function sdw_post(url, data, status, done) {
   var form = $('#__AjaxAntiForgeryForm');
   var token = $('input[name="__RequestVerificationToken"]', form).val();
   $('#modal-content-title').text('Saving Data');
   $('#modal-content-text').text(status);
   $('#modal-content-close').hide();
   document.modal.open();
   var postData = { __RequestVerificationToken: token }
   for (var attrname in data) { postData[attrname] = data[attrname]; }
   $.ajax({
      type: 'POST',
      url: url,
      data: postData
   }).done(function (d) {
      if (done !== undefined) {
         done(d);
      }
      $('#modal-content-text').text('Success!');
      setTimeout(function () { document.modal.close(); }, 500);
   }).fail(function (d) {
      $('#modal-content-close').show();
      $('#modal-content-title').text('Failed');
      $('#modal-content-text').text(d.responseText);
   });
}
