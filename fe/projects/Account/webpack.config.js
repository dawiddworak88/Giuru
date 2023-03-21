const path = require("path");
const MiniCssExtractPlugin = require("mini-css-extract-plugin");
const CssMinimizerPlugin = require("css-minimizer-webpack-plugin");
const TerserPlugin = require('terser-webpack-plugin');
const { CleanWebpackPlugin } = require("clean-webpack-plugin");

module.exports = {
    entry: {
        registerpage: ["./src/areas/Accounts/pages/Register/index.js", "./src/areas/Accounts/pages/Register/RegisterPage.scss"],
        resetpasswordpage: ["./src/areas/Accounts/pages/ResetPassword/index.js", "./src/areas/Accounts/pages/ResetPassword/ResetPasswordPage.scss"],
        signinpage: ["./src/areas/Accounts/pages/SignIn/index.js", "./src/areas/Accounts/pages/SignIn/SignInPage.scss"],
        setpasswordpage: ["./src/areas/Accounts/pages/SetPassword/index.js", "./src/areas/Accounts/pages/SetPassword/SetPasswordPage.scss"],
        contentpage: ["./src/areas/Home/pages/Content/index.js", "./src/areas/Home/pages/Content/ContentPage.scss"]
    },
    output: {
        publicPath: path.resolve(__dirname, "../../../be/src/Project/Services/Identity/Identity.Api/wwwroot/dist/js"),
        path: path.resolve(__dirname, "../../../be/src/Project/Services/Identity/Identity.Api/wwwroot/dist/js"),
        filename: "[name].js"
    },
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