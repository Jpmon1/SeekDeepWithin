define([
   'jquery',
   'underscore',
   'backbone',
   'common',
   'collections/lights',
   'views/light/list',
   'views/edit/index',
   'views/about/index'
], function($, _, Backbone, Common, LightCollection, LightListView, EditView, AboutView){
   var AppRouter = Backbone.Router.extend({
      initialize: function(){
         Backbone.history.start();
      },
      routes: {
         '':'home',
         'edit':'edit',
         'about':'about'
      },
      home: function(){
         Common.loadStart();
         var lightListView = new LightListView({collection: LightCollection});
         lightListView.render();
      },
      about: function(){
         Common.loadStart();
         var aboutView = new AboutView();
         aboutView.render();
      },
      edit: function(){
         Common.loadStart();
         var editView = new EditView();
         editView.render();
      }
   });
   return AppRouter;
});