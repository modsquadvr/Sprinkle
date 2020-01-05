const HtmlWebpackPlugin = require("html-webpack-plugin");
const TerserPlugin = require('terser-webpack-plugin');
const webpack = require('webpack');
const webpackDevServer = require('webpack-dev-server');

const port = process.env.PORT || 3000;

module.exports = {
  entry: './src/index.js',
  module: {
    rules: [
      {
        test: /\.(js|jsx)$/,
        exclude: /node_modules/,
        loaders: ["babel-loader"]
      },
      {
        test: /\.html$/,
        use: [
          {
            loader: "html-loader"
          }
        ]
      },
      {
        test: /\.css$/,
        use: ['style-loader', 'css-loader']
      },
      {
        test: /\.scss$/,
        use: ['style-loader', 'css-loader', 'sass-loader']
      },
      {
        test: /\.svg$/,
        use: "file-loader",
      },
      {
        test: /\.(jpg|png)$/,
        use: {
          loader: "file-loader",
          options: {
            name: "[path][name].[hash].[ext]",
          },
        },
      }
    ]
  },
  devtool: 'inline-source-map',
  output: {
    path: __dirname + '/dist',
    publicPath: '/',
    filename: 'bundle.js'
  },
  plugins: [
    new webpack.HotModuleReplacementPlugin(),
    new HtmlWebpackPlugin({
      template: './public/index.html',
      favicon: './public/favicon.ico'
    })
  ],
  devServer: {
    publicPath: "/",
    contentBase: "./public",
    hot: true,
    host: 'localhost',
    port: port,
    historyApiFallback: {
      index: '/'
    },
    open: true
  }
};
