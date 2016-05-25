define([
   'jquery',
   'underscore',
   'backbone',
   'router',
   'autocomplete'
], function($, _, Backbone, Router, Autocomplete){
   'use strict';
   var app = Backbone.View.extend({
      el: 'body',
      masonry: null,
      appRouter: null,
      initialize: function () {
         this.appRouter = new Router ();
         Autocomplete.Autocomplete ($('txtSearch'), {
         //$('txtSearch').autocomplete ({
            serviceUrl: '/api/suggest',
            paramName: 'token',
            noCache: true,
            deferRequestBy: 500,
            triggerSelectOnValidInput: false
         });
      }
   });
   return app;
});
/*
$(document).ready(function(){
   iinallCommon.get('/api/randomLight', {}, function(d){
      for (var i = 0; i < d.light.length; i++){
         var light = d.light[i];
         $('#divLove').append (iinall.buildLight (light));
      }
      $('#divLove').masonry({ itemSelector: '.column' });
      iinallCommon.loadStop();
   });
   $('#txtSearch').autocomplete({
      serviceUrl: '/api/suggest',
      paramName: 'token',
      noCache: true,
      deferRequestBy: 500,
      triggerSelectOnValidInput: false
   });
});*/