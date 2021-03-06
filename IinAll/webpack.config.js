var path = require('path');
var webpack = require('webpack');

module.exports = {
  /*devtool: 'cheap-module-source-map',
  plugins: [
    new webpack.DefinePlugin({
      'process.env': {
        'NODE_ENV': JSON.stringify('production')
      }
    })
  ],*/
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