var path = require('path');
var webpack = require('webpack');

module.exports = {
   entry: [
      './js/app.js'
   ],
   output: {
      filename: 'bundle.js',
      path: __dirname + '/dist'
   },
   module: {
      loaders: [
         {
            test: /\.jsx?$/,
            loader: 'babel-loader',
            exclude: /node_modules/,
            query: {
               presets: ['react', 'es2015']
            }
         },
         {
            test: /\.css$/,
            loader: 'style!css?modules',
            include: /flexboxgrid/,
         }
      ]
   }
}