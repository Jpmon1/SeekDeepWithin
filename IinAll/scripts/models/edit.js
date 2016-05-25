define (['jquery', 'underscore', 'backbone'],function ($, _, Backbone) {
   var editModel = Backbone.Model.extend({
      defaults: { motto: '', newLight: '', addLove: '' }
   });
   return editModel;
});