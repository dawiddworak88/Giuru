const CssMinimizerPlugin = require("css-minimizer-webpack-plugin");
const TerserPlugin = require('terser-webpack-plugin');
const MiniCssExtractPlugin = require("mini-css-extract-plugin");
const { CleanWebpackPlugin } = require("clean-webpack-plugin");

exports.defaultWebpackConfiguration = ({

} = {}) => {
    return {
        resolve: {
            extensions: [".js", ".jsx"]
        },
        module: {
            rules: [
                {
                    test: /\.(js|jsx)$/,
                    exclude: /node_modules/,
                    use: [
                        { 
                            loader: "babel-loader" 
                        }
                    ]
                },
                {
                    test: /\.(png|svg|jpg|jpeg|gif|woff|woff2|eot|ttf|otf)$/i,
                    type: 'asset/inline',
                },
                {
                    test: /\.(scss|css)$/,
                    use: [
                        MiniCssExtractPlugin.loader,
                        "css-loader",
                        "postcss-loader",
                        "sass-loader"
                    ]
                }
            ]
        },
        optimization: {
            minimize: true,
            minimizer: [
                new TerserPlugin(),
                new CssMinimizerPlugin()
            ]
        },
        performance: {
            hints: false,
            maxEntrypointSize: 512000,
            maxAssetSize: 512000
        },
        plugins: [
            new MiniCssExtractPlugin({
                filename: "../../dist/css/[name].css",
            }),
            new CleanWebpackPlugin({
                dry: false,
                dangerouslyAllowCleanPatternsOutsideProject: true
            })
        ]
    }
}