define([
   'jquery',
   'underscore',
   'backbone',
   'views/light/item',
   'masonry',
   'common'
], function($, _, Backbone, LightView, Masonry, Common){
   var lightListView = Backbone.View.extend({
      el: $('#love'),
      masonry: null,
      initialize: function(data) {
         this.collection = data.collection;
         this.collection.on('add', this.render, this);
         this.collection.on('reset', this.render, this);
         this.masonry = new Masonry ('#love', { itemSelector: '.column' });
         $(document).on('click', '.truth', function(){
            alert(this.innerHTML);
         });
      },
      render: function(){
         this.$el.show();
         this.$el.html('');
         $('#edit-stuff').hide();
         $('.search-stuff').show();
         this.collection.each(function(light){
            var lightView = new LightView({ model: light });
            this.$el.append(lightView.render().el);
         }, this);
         this.masonry.reloadItems();
         this.masonry.layout();
         Common.loadStop();
      }
   });
   return lightListView;
});