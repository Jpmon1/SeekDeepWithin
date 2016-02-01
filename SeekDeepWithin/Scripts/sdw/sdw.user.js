$(document).ready(function() {
   $('#saveOk').hide();
   $('#btnSave').click(function(e) {
      e.preventDefault();
      $.ajax({
         type: 'POST',
         url: "http://" + location.host + "/Account/SaveSettings",
         data: {
            LoadOnScroll: $('#LoadOnScroll').prop('checked')
         }
      }).done(function () {
         $('#saveOk').show();
         setTimeout(function() {$('#saveOk').hide();}, 700);
      }).fail(function(d) {
         alert(d.responseText);
      });
   });
   $('#btnLogout').click(function(e) {
      e.preventDefault();
      document.getElementById('logoutForm').submit();
   });
});