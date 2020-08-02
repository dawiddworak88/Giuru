const path = require("path");
const MiniCssExtractPlugin = require("mini-css-extract-plugin");
const OptimizeCSSAssetsPlugin = require("optimize-css-assets-webpack-plugin");
const CopyWebpackPlugin = require("copy-webpack-plugin");
const { CleanWebpackPlugin } = require('clean-webpack-plugin');

var browserConfig = {
    module: {
        rules: [
            {
                test: /\.(scss|css)$/,
                use: [
                    MiniCssExtractPlugin.loader,
                    "css-loader",
                    "postcss-loader",
                    "sass-loader"
                ]
            },
            {
                test: /\.(png|svg|jpg|gif|woff(2)?|ttf|eot)$/,
                use: [{
                    loader: "file-loader",
                    options: {
                        name: "[name].[ext]",
                        outputPath: "../images",
                        publicPath: "/dist/images"
                    }
                }]
            },
            {
                test: /\.(js|jsx)$/,
                exclude: /node_modules/,
                use: [
                    { loader: "babel-loader" }
                ]
            }
        ]
    },
    plugins: [
        new MiniCssExtractPlugin({
            filename: "../../dist/css/[name].css",
        }),
        new CleanWebpackPlugin({
            dry: false,
            dangerouslyAllowCleanPatternsOutsideProject: true
        }),
        new CopyWebpackPlugin([{
            from: path.resolve(__dirname, "wwwroot/src/*.png"),
            to: path.resolve(__dirname, "wwwroot/dist/images") + "/[name].[ext]"
        }])
    ],
    optimization: {
        minimizer: [
            new OptimizeCSSAssetsPlugin({})
        ]
    },
    resolve: {
        extensions: [".js", ".jsx"]
    },
    entry: {
        homepage: ["./src/project/AspNetCore/areas/Home/pages/HomePage/index.js", "./src/project/AspNetCore/areas/Home/pages/HomePage/HomePage.scss"],
        categorypage: ["./src/project/AspNetCore/areas/Products/pages/CategoryPage/index.js", "./src/project/AspNetCore/areas/Products/pages/CategoryPage/CategoryPage.scss"],
        productpage: ["./src/project/AspNetCore/areas/Products/pages/ProductPage/index.js", "./src/project/AspNetCore/areas/Products/pages/ProductPage/ProductPage.scss"],
        brandpage: ["./src/project/AspNetCore/areas/Brands/pages/BrandPage/index.js", "./src/project/AspNetCore/areas/Brands/pages/BrandPage/BrandPage.scss"]
    },
    output: {
        publicPath: path.resolve(__dirname, "../be/src/Project/Web/AspNetCore/wwwroot/dist/js"),
        path: path.resolve(__dirname, "../be/src/Project/Web/AspNetCore/wwwroot/dist/js"),
        filename: "[name].js"
    }
};

var accountBrowserConfig = {
    module: {
        rules: [
            {
                test: /\.(scss|css)$/,
                use: [
                    MiniCssExtractPlugin.loader,
                    "css-loader",
                    "postcss-loader",
                    "sass-loader"
                ]
            },
            {
                test: /\.(png|svg|jpg|gif|woff(2)?|ttf|eot)$/,
                use: [{
                    loader: "file-loader",
                    options: {
                        name: "[name].[ext]",
                        outputPath: "../images",
                        publicPath: "/dist/images"
                    }
                }]
            },
            {
                test: /\.(js|jsx)$/,
                exclude: /node_modules/,
                use: [
                    { loader: "babel-loader" }
                ]
            }
        ]
    },
    plugins: [
        new MiniCssExtractPlugin({
            filename: "../../dist/css/[name].css",
        }),
        new CleanWebpackPlugin({
            dry: false,
            dangerouslyAllowCleanPatternsOutsideProject: true
        }),
        new CopyWebpackPlugin([{
            from: path.resolve(__dirname, "wwwroot/src/*.png"),
            to: path.resolve(__dirname, "wwwroot/dist/images") + "/[name].[ext]"
        }])
    ],
    optimization: {
        minimizer: [
            new OptimizeCSSAssetsPlugin({})
        ]
    },
    resolve: {
        extensions: [".js", ".jsx"]
    },
    entry: {
        signinpage: ["./src/project/Account/areas/Accounts/pages/SignIn/index.js", "./src/project/Account/areas/Accounts/pages/SignIn/SignInPage.scss"]
    },
    output: {
        publicPath: path.resolve(__dirname, "../be/src/Project/Web/Account/wwwroot/dist/js"),
        path: path.resolve(__dirname, "../be/src/Project/Web/Account/wwwroot/dist/js"),
        filename: "[name].js"
    }
};

var sellerPortalBrowserConfig = {
    module: {
        rules: [
            {
                test: /\.(scss|css)$/,
                use: [
                    MiniCssExtractPlugin.loader,
                    "css-loader",
                    "postcss-loader",
                    "sass-loader"
                ]
            },
            {
                test: /\.(png|svg|jpg|gif|woff(2)?|ttf|eot)$/,
                use: [{
                    loader: "file-loader",
                    options: {
                        name: "[name].[ext]",
                        outputPath: "../images",
                        publicPath: "/dist/images"
                    }
                }]
            },
            {
                test: /\.(js|jsx)$/,
                exclude: /node_modules/,
                use: [
                    { loader: "babel-loader" }
                ]
            }
        ]
    },
    plugins: [
        new MiniCssExtractPlugin({
            filename: "../../dist/css/[name].css",
        }),
        new CleanWebpackPlugin({
            dry: false,
            dangerouslyAllowCleanPatternsOutsideProject: true
        }),
        new CopyWebpackPlugin([{
            from: path.resolve(__dirname, "wwwroot/src/*.png"),
            to: path.resolve(__dirname, "wwwroot/dist/images") + "/[name].[ext]"
        }])
    ],
    optimization: {
        minimizer: [
            new OptimizeCSSAssetsPlugin({})
        ]
    },
    resolve: {
        extensions: [".js", ".jsx"]
    },
    entry: {
        orderpage: ["./src/project/Seller.Portal/areas/Orders/pages/OrderPage/index.js", "./src/project/Seller.Portal/areas/Orders/pages/OrderPage/OrderPage.scss"],
        orderdetailpage: ["./src/project/Seller.Portal/areas/Orders/pages/OrderDetailPage/index.js", "./src/project/Seller.Portal/areas/Orders/pages/OrderDetailPage/OrderDetailPage.scss"],
        importorderpage: ["./src/project/Seller.Portal/areas/Orders/pages/ImportOrderPage/index.js", "./src/project/Seller.Portal/areas/Orders/pages/ImportOrderPage/ImportOrderPage.scss"],
        clientpage: ["./src/project/Seller.Portal/areas/Clients/pages/ClientPage/index.js", "./src/project/Seller.Portal/areas/Clients/pages/ClientPage/ClientPage.scss"],
        clientdetailpage: ["./src/project/Seller.Portal/areas/Clients/pages/ClientDetailPage/index.js", "./src/project/Seller.Portal/areas/Clients/pages/ClientDetailPage/ClientDetailPage.scss"],
        productpage: ["./src/project/Seller.Portal/areas/Products/pages/ProductPage/index.js", "./src/project/Seller.Portal/areas/Products/pages/ProductPage/ProductPage.scss"],
        productdetailpage: ["./src/project/Seller.Portal/areas/Products/pages/ProductDetailPage/index.js", "./src/project/Seller.Portal/areas/Products/pages/ProductDetailPage/ProductDetailPage.scss"]
    },
    output: {
        publicPath: path.resolve(__dirname, "../be/src/Project/Web/Seller.Portal/wwwroot/dist/js"),
        path: path.resolve(__dirname, "../be/src/Project/Web/Seller.Portal/wwwroot/dist/js"),
        filename: "[name].js"
    }
};

module.exports = [browserConfig, accountBrowserConfig, sellerPortalBrowserConfig];