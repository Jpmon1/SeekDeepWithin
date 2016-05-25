define(['underscore', 'backbone'], function(_, Backbone){
   var lightModel = Backbone.Model.extend({
      defaults: { id: 0, text: "unknown", span: 4, tId: 0 }
   });
   return lightModel;
});