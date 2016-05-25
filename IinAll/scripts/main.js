/**
 * Created by Jonathan Montiverdi on 5/21/2016.
 */
'use strict';

require.config({
    paths: {
        jquery: 'lib/jquery/jquery-1.11.2',
        autocomplete: 'lib/jquery/jquery.autocomplete',
        underscore: 'lib/backbone/underscore-min',
        backbone: 'lib/backbone/backbone-min',
        masonry: 'lib/masonry/masonry.pkgd',
        handlebars: 'lib/handlebars/handlebars-v4.0.5'
    }
});

require(['app'], function(App){
    new App;
});