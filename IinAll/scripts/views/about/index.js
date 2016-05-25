define(['jquery', 'underscore', 'backbone', 'common'], function($, _, Backbone, Common) {
   var aboutView = Backbone.View.extend({
      el: $('#love'),
      render: function () {
         var view = this;
         $('#edit-stuff').hide();
         $('.search-stuff').hide();
         view.$el.html('');
         Common.get('/about.php', {}, function (d) {
            view.$el.html(d);
            Common.loadStop();
         });
      }
   });
   return aboutView;
});