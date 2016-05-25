define(['jquery', 'underscore', 'backbone', 'models/light'], function($, _, Backbone, LightModel){
   'use strict';
   var LightCollection = Backbone.Collection.extend({
      model: LightModel,
      url: '/api/Light',
      initialize: function() {
         this.fetch();
      }
   });
   return new LightCollection ();
});