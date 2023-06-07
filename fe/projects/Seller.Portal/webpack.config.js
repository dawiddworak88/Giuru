const path = require("path");
const MiniCssExtractPlugin = require("mini-css-extract-plugin");
const CssMinimizerPlugin = require("css-minimizer-webpack-plugin");
const TerserPlugin = require('terser-webpack-plugin');
const { CleanWebpackPlugin } = require("clean-webpack-plugin");

module.exports = {
    entry: {
        productcardpage: ["./src/areas/Products/pages/ProductCardPage/index.js", "./src/areas/Products/pages/ProductCardPage/ProductCardPage.scss"],
        productcardspage: ["./src/areas/Products/pages/ProductCardsPage/index.js", "./src/areas/Products/pages/ProductCardsPage/ProductCardsPage.scss"],
        dashboardpage: ["./src/areas/Dashboard/pages/DashboardPage/index.js", "./src/areas/Dashboard/pages/DashboardPage/DashboardPage.scss"],
        countrypage: ["./src/areas/Global/pages/CountryPage/index.js", "./src/areas/Global/pages/CountryPage/CountryPage.scss"],
        countriespage: ["./src/areas/Global/pages/CountriesPage/index.js", "./src/areas/Global/pages/CountriesPage/CountriesPage.scss"],
        teammember: ["./src/areas/TeamMembers/pages/TeamMember/index.js", "./src/areas/TeamMembers/pages/TeamMember/TeamMember.scss"],
        teammembers: ["./src/areas/TeamMembers/pages/TeamMembers/index.js", "./src/areas/TeamMembers/pages/TeamMembers/TeamMembers.scss"],
        downloadcenteritempage: ["./src/areas/DownloadCenter/pages/DownloadCenterItemPage/index.js", "./src/areas/DownloadCenter/pages/DownloadCenterItemPage/DownloadCenterItemPage.scss"],
        downloadcenterpage: ["./src/areas/DownloadCenter/pages/DownloadCenterPage/index.js", "./src/areas/DownloadCenter/pages/DownloadCenterPage/DownloadCenterPage.scss"],
        downloadcentercategorypage: ["./src/areas/DownloadCenter/pages/DownloadCenterCategoryPage/index.js", "./src/areas/DownloadCenter/pages/DownloadCenterCategoryPage/DownloadCenterCategoryPage.scss"],
        downloadcentercategoriespage: ["./src/areas/DownloadCenter/pages/DownloadCenterCategoriesPage/index.js", "./src/areas/DownloadCenter/pages/DownloadCenterCategoriesPage/DownloadCenterCategoriesPage.scss"],
        clientrolepage: ["./src/areas/Clients/pages/ClientRolePage/index.js", "./src/areas/Clients/pages/ClientRolePage/ClientRolePage.scss"],
        clientrolespage: ["./src/areas/Clients/pages/ClientRolesPage/index.js", "./src/areas/Clients/pages/ClientRolesPage/ClientRolesPage.scss"],
        clientapplicationpage: ["./src/areas/Clients/pages/ClientApplicationPage/index.js", "./src/areas/Clients/pages/ClientApplicationPage/ClientApplicationPage.scss"],
        clientsapplicationspage: ["./src/areas/Clients/pages/ClientsApplicationsPage/index.js", "./src/areas/Clients/pages/ClientsApplicationsPage/ClientsApplicationsPage.scss"],
        mediapage: ["./src/areas/MediaItems/pages/MediaPage/index.js", "./src/areas/MediaItems/pages/MediaPage/MediaPage.scss"],
        mediaitempage: ["./src/areas/MediaItems/pages/MediaItemPage/index.js", "./src/areas/MediaItems/pages/MediaItemPage/MediaItemPage.scss"],
        clientaccountmanagerpage: ["./src/areas/Clients/pages/ClientAccountManagerPage/index.js", "./src/areas/Clients/pages/ClientAccountManagerPage/ClientAccountManagerPage.scss"],
        clientaccountmanagerspage: ["./src/areas/Clients/pages/ClientAccountManagersPage/index.js", "./src/areas/Clients/pages/ClientAccountManagersPage/ClientAccountManagersPage.scss"],
        mediaitemspage: ["./src/areas/MediaItems/pages/MediaItemsPage/index.js", "./src/areas/MediaItems/pages/MediaItemsPage/MediaItemsPage.scss"],
        outletpage: ["./src/areas/Inventory/pages/OutletPage/index.js", "./src/areas/Inventory/pages/OutletPage/OutletPage.scss"],
        outletspage: ["./src/areas/Inventory/pages/OutletsPage/index.js", "./src/areas/Inventory/pages/OutletsPage/OutletsPage.scss"],
        clientgrouppage: ["./src/areas/Clients/pages/ClientGroupPage/index.js", "./src/areas/Clients/pages/ClientGroupPage/ClientGroupPage.scss"],
        clientgroupspage: ["./src/areas/Clients/pages/ClientGroupsPage/index.js", "./src/areas/Clients/pages/ClientGroupsPage/ClientGroupsPage.scss"],
        newsitempage: ["./src/areas/News/pages/NewsItemPage/index.js", "./src/areas/News/pages/NewsItemPage/NewsItemPage.scss"],
        newspage: ["./src/areas/News/pages/NewsPage/index.js", "./src/areas/News/pages/NewsPage/NewsPage.scss"],
        newscategoriespage: ["./src/areas/News/pages/CategoriesPage/index.js", "./src/areas/News/pages/CategoriesPage/CategoriesPage.scss"],
        newscategorypage: ["./src/areas/News/pages/CategoryPage/index.js", "./src/areas/News/pages/CategoryPage/CategoryPage.scss"],
        inventorypage: ["./src/areas/Inventory/pages/InventoryPage/index.js", "./src/areas/Inventory/pages/InventoryPage/InventoryPage.scss"],
        inventoriespage: ["./src/areas/Inventory/pages/InventoriesPage/index.js", "./src/areas/Inventory/pages/InventoriesPage/InventoriesPage.scss"],
        warehousespage: ["./src/areas/Inventory/pages/WarehousesPage/index.js", "./src/areas/Inventory/pages/WarehousesPage/WarehousesPage.scss"],
        warehousepage: ["./src/areas/Inventory/pages/WarehousePage/index.js", "./src/areas/Inventory/pages/WarehousePage/WarehousePage.scss"],
        orderitempage: ["./src/areas/Orders/pages/OrderItemPage/index.js", "./src/areas/Orders/pages/OrderItemPage/OrderItemPage.scss"],
        orderspage: ["./src/areas/Orders/pages/OrdersPage/index.js", "./src/areas/Orders/pages/OrdersPage/OrdersPage.scss"],
        orderpage: ["./src/areas/Orders/pages/OrderPage/index.js", "./src/areas/Orders/pages/OrderPage/OrderPage.scss"],
        editorderpage: ["./src/areas/Orders/pages/EditOrderPage/index.js", "./src/areas/Orders/pages/EditOrderPage/EditOrderPage.scss"],
        clientspage: ["./src/areas/Clients/pages/ClientsPage/index.js", "./src/areas/Clients/pages/ClientsPage/ClientsPage.scss"],
        clientpage: ["./src/areas/Clients/pages/ClientPage/index.js", "./src/areas/Clients/pages/ClientPage/ClientPage.scss"],
        productspage: ["./src/areas/Products/pages/ProductsPage/index.js", "./src/areas/Products/pages/ProductsPage/ProductsPage.scss"],
        productpage: ["./src/areas/Products/pages/ProductPage/index.js", "./src/areas/Products/pages/ProductPage/ProductPage.scss"],
        productattributespage: ["./src/areas/Products/pages/ProductAttributesPage/index.js", "./src/areas/Products/pages/ProductAttributesPage/ProductAttributesPage.scss"],
        productattributepage: ["./src/areas/Products/pages/ProductAttributePage/index.js", "./src/areas/Products/pages/ProductAttributePage/ProductAttributePage.scss"],
        productattributeitempage: ["./src/areas/Products/pages/ProductAttributeItemPage/index.js", "./src/areas/Products/pages/ProductAttributeItemPage/ProductAttributeItemPage.scss"],
        categoriespage: ["./src/areas/Products/pages/CategoriesPage/index.js", "./src/areas/Products/pages/CategoriesPage/CategoriesPage.scss"],
        categorypage: ["./src/areas/Products/pages/CategoryPage/index.js", "./src/areas/Products/pages/CategoryPage/CategoryPage.scss"],
        settingspage: ["./src/areas/Settings/pages/SettingsPage/index.js", "./src/areas/Settings/pages/SettingsPage/SettingsPage.scss"]
    },
    output: {
        publicPath: path.resolve(__dirname, "../../../be/src/Project/Web/Seller/Seller.Web/wwwroot/dist/js"),
        path: path.resolve(__dirname, "../../../be/src/Project/Web/Seller/Seller.Web/wwwroot/dist/js"),
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
                    },
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