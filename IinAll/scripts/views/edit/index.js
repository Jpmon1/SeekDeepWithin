define(['jquery', 'underscore', 'backbone', 'common', 'models/edit'], function($, _, Backbone, Common, EditModel) {
   var editView = Backbone.View.extend({
      el: '#edit-stuff',
      model: new EditModel,
      events: {
         "change #txtMotto": "setMotto",
         "change #txtCreateLight": "setNewLight"
      },
      setMotto: function (){
         this.model.set('motto', $('#txtMotto').val());
      },
      setNewLight: function (){
         this.model.set('newLight', $('#txtCreateLight').val());
      },
      render: function () {
         var view = this;
         view.$el.show();
         $('#love').hide();
         $('.search-stuff').hide();
         view.$el.html('');
         Common.get('/edit/index.php', {}, function (d) {
            view.$el.html(d);
            Common.loadStop();
         });
      }
   });
   return editView;
});