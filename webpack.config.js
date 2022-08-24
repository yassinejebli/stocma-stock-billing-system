"use strict";
var path = require("path");
var WebpackNotifierPlugin = require("webpack-notifier");
var BrowserSyncPlugin = require("browser-sync-webpack-plugin");
var MinifyPlugin = require("babel-minify-webpack-plugin");
var BundleAnalyzerPlugin =
  require("webpack-bundle-analyzer").BundleAnalyzerPlugin;
var CompressionPlugin = require("compression-webpack-plugin");

module.exports = {
  entry: "./Scripts/Home/react/index.js",
  output: {
    path: path.resolve(__dirname, "./Scripts/dist/Home/react"),
    filename: "bundle.js",
  },
  devServer: { contentBase: ".", host: "localhost", port: 8080 },
  module: {
    rules: [
      {
        test: /\.js$/,
        exclude: /node_modules/,
        loader: require.resolve("babel-loader"),
      },
      {
        test: /\.(png|jp(e*)g|svg|gif)$/,
        use: [
          {
            loader: "file-loader",
            options: {
              name: "[name].[ext]",
            },
          },
        ],
      },
    ],
  },
  devtool: "inline-source-map",
  plugins: [
    new WebpackNotifierPlugin(),
    new BrowserSyncPlugin(),
    // new MinifyPlugin(),
    // new BundleAnalyzerPlugin(),
    // new CompressionPlugin()
  ],
};
