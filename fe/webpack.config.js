const path = require("path");
const MiniCssExtractPlugin = require("mini-css-extract-plugin");
const OptimizeCSSAssetsPlugin = require("optimize-css-assets-webpack-plugin");
const CopyWebpackPlugin = require("copy-webpack-plugin");
const TerserPlugin = require('terser-webpack-plugin');
const { CleanWebpackPlugin } = require("clean-webpack-plugin");

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
            new OptimizeCSSAssetsPlugin({}),
            new TerserPlugin({})
        ]
    },
    resolve: {
        extensions: [".js", ".jsx"]
    },
    entry: {
        homepage: ["./src/project/AspNetCore/areas/Home/pages/HomePage/index.js", "./src/project/AspNetCore/areas/Home/pages/HomePage/HomePage.scss"],
        categorypage: ["./src/project/AspNetCore/areas/Products/pages/CategoryPage/index.js", "./src/project/AspNetCore/areas/Products/pages/CategoryPage/CategoryPage.scss"],
        searchproductspage: ["./src/project/AspNetCore/areas/Products/pages/SearchProductsPage/index.js", "./src/project/AspNetCore/areas/Products/pages/SearchProductsPage/SearchProductsPage.scss"],
        productpage: ["./src/project/AspNetCore/areas/Products/pages/ProductPage/index.js", "./src/project/AspNetCore/areas/Products/pages/ProductPage/ProductPage.scss"],
        brandpage: ["./src/project/AspNetCore/areas/Products/pages/BrandPage/index.js", "./src/project/AspNetCore/areas/Products/pages/BrandPage/BrandPage.scss"]
    },
    output: {
        publicPath: path.resolve(__dirname, "../be/src/Project/Web/Buyer/Buyer.Web/wwwroot/dist/js"),
        path: path.resolve(__dirname, "../be/src/Project/Web/Buyer/Buyer.Web/wwwroot/dist/js"),
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
            new OptimizeCSSAssetsPlugin({}),
            new TerserPlugin({})
        ]
    },
    resolve: {
        extensions: [".js", ".jsx"]
    },
    entry: {
        signinpage: ["./src/project/Account/areas/Accounts/pages/SignIn/index.js", "./src/project/Account/areas/Accounts/pages/SignIn/SignInPage.scss"],
        contentpage: ["./src/project/Account/areas/Home/pages/Content/index.js", "./src/project/Account/areas/Home/pages/Content/ContentPage.scss"]
    },
    output: {
        publicPath: path.resolve(__dirname, "../be/src/Project/Services/Identity/Identity.Api/wwwroot/dist/js"),
        path: path.resolve(__dirname, "../be/src/Project/Services/Identity/Identity.Api/wwwroot/dist/js"),
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
            new OptimizeCSSAssetsPlugin({}),
            new TerserPlugin({})
        ]
    },
    resolve: {
        extensions: [".js", ".jsx"]
    },
    entry: {
        orderspage: ["./src/project/Seller.Portal/areas/Orders/pages/OrdersPage/index.js", "./src/project/Seller.Portal/areas/Orders/pages/OrdersPage/OrdersPage.scss"],
        orderpage: ["./src/project/Seller.Portal/areas/Orders/pages/OrderPage/index.js", "./src/project/Seller.Portal/areas/Orders/pages/OrderPage/OrderPage.scss"],
        editorderpage: ["./src/project/Seller.Portal/areas/Orders/pages/EditOrderPage/index.js", "./src/project/Seller.Portal/areas/Orders/pages/EditOrderPage/EditOrderPage.scss"],
        clientspage: ["./src/project/Seller.Portal/areas/Clients/pages/ClientsPage/index.js", "./src/project/Seller.Portal/areas/Clients/pages/ClientsPage/ClientsPage.scss"],
        clientpage: ["./src/project/Seller.Portal/areas/Clients/pages/ClientPage/index.js", "./src/project/Seller.Portal/areas/Clients/pages/ClientPage/ClientPage.scss"],
        productspage: ["./src/project/Seller.Portal/areas/Products/pages/ProductsPage/index.js", "./src/project/Seller.Portal/areas/Products/pages/ProductsPage/ProductsPage.scss"],
        productpage: ["./src/project/Seller.Portal/areas/Products/pages/ProductPage/index.js", "./src/project/Seller.Portal/areas/Products/pages/ProductPage/ProductPage.scss"],
        productattributespage: ["./src/project/Seller.Portal/areas/Products/pages/ProductAttributesPage/index.js", "./src/project/Seller.Portal/areas/Products/pages/ProductAttributesPage/ProductAttributesPage.scss"],
        productattributepage: ["./src/project/Seller.Portal/areas/Products/pages/ProductAttributePage/index.js", "./src/project/Seller.Portal/areas/Products/pages/ProductAttributePage/ProductAttributePage.scss"],
        productattributeitempage: ["./src/project/Seller.Portal/areas/Products/pages/ProductAttributeItemPage/index.js", "./src/project/Seller.Portal/areas/Products/pages/ProductAttributeItemPage/ProductAttributeItemPage.scss"],
        categoriespage: ["./src/project/Seller.Portal/areas/Products/pages/CategoriesPage/index.js", "./src/project/Seller.Portal/areas/Products/pages/CategoriesPage/CategoriesPage.scss"],
        categorypage: ["./src/project/Seller.Portal/areas/Products/pages/CategoryPage/index.js", "./src/project/Seller.Portal/areas/Products/pages/CategoryPage/CategoryPage.scss"],
        settingspage: ["./src/project/Seller.Portal/areas/Settings/pages/SettingsPage/index.js", "./src/project/Seller.Portal/areas/Settings/pages/SettingsPage/SettingsPage.scss"]
    },
    output: {
        publicPath: path.resolve(__dirname, "../be/src/Project/Web/Seller/Seller.Web/wwwroot/dist/js"),
        path: path.resolve(__dirname, "../be/src/Project/Web/Seller/Seller.Web/wwwroot/dist/js"),
        filename: "[name].js"
    }
};

module.exports = [browserConfig, accountBrowserConfig, sellerPortalBrowserConfig];
