var webpack = require('webpack');

module.exports = {
    entry: "./wwwroot/js/site.js",
    devtool: 'source-map',
    output: {
        path: __dirname,
        filename: "./wwwroot/assets/bundle.js"
    },
    module: {
        loaders: [
            { test: /\.css$/, loader: "style!css" }
        ]
    },
    plugins: [
        new webpack.optimize.UglifyJsPlugin({
            compress: {
                warnings: false,
            },
            output: {
                comments: false,
            }
        }),
    ]
};