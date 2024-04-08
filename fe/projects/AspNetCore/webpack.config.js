const path = require("path");
const MiniCssExtractPlugin = require("mini-css-extract-plugin");
const CssMinimizerPlugin = require("css-minimizer-webpack-plugin");
const TerserPlugin = require('terser-webpack-plugin');
const { CleanWebpackPlugin } = require("clean-webpack-plugin");

module.exports = {
    entry: {
        orderitempage: ["./src/areas/Orders/pages/OrderItemPage/index.js", "./src/areas/Orders/pages/OrderItemPage/OrderItemPage.scss"],
        dashboardpage: ["./src/areas/Dashboard/pages/DashboardPage/index.js", "./src/areas/Dashboard/pages/DashboardPage/DashboardPage.scss"],
        downloadcentercategorypage: ["./src/areas/DownloadCenter/pages/DownloadCenterCategoryPage/index.js", "./src/areas/DownloadCenter/pages/DownloadCenterCategoryPage/DownloadCenterCategoryPage.scss"],
        downloadcenterpage: ["./src/areas/DownloadCenter/pages/DownloadCenterPage/index.js", "./src/areas/DownloadCenter/pages/DownloadCenterPage/DownloadCenterPage.scss"],
        applicationpage: ["./src/areas/Home/pages/ApplicationPage/index.js", "./src/areas/Home/pages/ApplicationPage/ApplicationPage.scss"],
        outletpage: ["./src/areas/Products/pages/OutletPage/index.js", "./src/areas/Products/pages/OutletPage/OutletPage.scss"],
        newsItemPage: ["./src/areas/News/pages/NewsItemPage/index.js", "./src/areas/News/pages/NewsItemPage/NewsItemPage.scss"],
        newsPage: ["./src/areas/News/pages/NewsPage/index.js", "./src/areas/News/pages/NewsPage/NewsPage.scss"],
        newOrderPage: ["./src/areas/Orders/pages/NewOrder/index.js", "./src/areas/Orders/pages/NewOrder/NewOrderPage.scss"],
        listOrdersPage: ["./src/areas/Orders/pages/ListOrders/index.js", "./src/areas/Orders/pages/ListOrders/ListOrdersPage.scss"],
        statusOrderPage: ["./src/areas/Orders/pages/StatusOrder/index.js", "./src/areas/Orders/pages/StatusOrder/StatusOrderPage.scss"],
        homepage: ["./src/areas/Home/pages/HomePage/index.js", "./src/areas/Home/pages/HomePage/HomePage.scss"],
        categorypage: ["./src/areas/Products/pages/CategoryPage/index.js", "./src/areas/Products/pages/CategoryPage/CategoryPage.scss"],
        searchproductspage: ["./src/areas/Products/pages/SearchProductsPage/index.js", "./src/areas/Products/pages/SearchProductsPage/SearchProductsPage.scss"],
        availableproductspage: ["./src/areas/Products/pages/AvailableProductsPage/index.js", "./src/areas/Products/pages/AvailableProductsPage/AvailableProductsPage.scss"],
        productpage: ["./src/areas/Products/pages/ProductPage/index.js", "./src/areas/Products/pages/ProductPage/ProductPage.scss"],
        slugPage: ["./src/areas/Content/pages/SlugPage/index.js", "./src/areas/Content/pages/SlugPage/SlugPage.scss"]
    },
    output: {
        publicPath: path.resolve(__dirname, "../../../be/src/Project/Web/Buyer/Buyer.Web/wwwroot/dist/js"),
        path: path.resolve(__dirname, "../../../be/src/Project/Web/Buyer/Buyer.Web/wwwroot/dist/js"),
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