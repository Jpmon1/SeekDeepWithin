$(document).ready(function() {
   $('#saveOk').velocity('transition.fadeOut');
   $('#btnSave').click(function(e) {
      e.preventDefault();
      $.ajax({
         type: 'POST',
         url: "http://" + location.host + "/Account/SaveSettings",
         data: {
            LoadOnScroll: $('#LoadOnScroll').prop('checked')
         }
      }).done(function() {
         $('#saveOk').velocity('transition.fadeIn').velocity('transition.fadeOut');
      }).fail(function(d) {
         alert(d.responseText);
      });
   });
   $('#btnLogout').click(function(e) {
      e.preventDefault();
      document.getElementById('logoutForm').submit();
   });
});