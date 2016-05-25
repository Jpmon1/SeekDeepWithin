define([
   'jquery',
   'underscore',
   'backbone'
], function($, _, Backbone){
   var lightListView = Backbone.View.extend({
      //tagName: 'div',
      template:_.template($("#light-template").html()),
      /*initialize: function(data) {
      },*/
      render: function(){
         var html = this.template(this.model.toJSON());
         this.el = html;
         return this;
      }
   });
   return lightListView;
});
