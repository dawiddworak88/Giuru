const parts = require("./config/webpack.parts");
const path = require("path");

module.exports = {
    ...parts.defaultWebpackConfiguration(),
    entry: {
        orderitempage: ["./src/project/AspNetCore/areas/Orders/pages/OrderItemPage/index.js", "./src/project/AspNetCore/areas/Orders/pages/OrderItemPage/OrderItemPage.scss"],
        dashboardpage: ["./src/project/AspNetCore/areas/Dashboard/pages/DashboardPage/index.js", "./src/project/AspNetCore/areas/Dashboard/pages/DashboardPage/DashboardPage.scss"],
        downloadcentercategorypage: ["./src/project/AspNetCore/areas/DownloadCenter/pages/DownloadCenterCategoryPage/index.js", "./src/project/AspNetCore/areas/DownloadCenter/pages/DownloadCenterCategoryPage/DownloadCenterCategoryPage.scss"],
        downloadcenterpage: ["./src/project/AspNetCore/areas/DownloadCenter/pages/DownloadCenterPage/index.js", "./src/project/AspNetCore/areas/DownloadCenter/pages/DownloadCenterPage/DownloadCenterPage.scss"],
        applicationpage: ["./src/project/AspNetCore/areas/Home/pages/ApplicationPage/index.js", "./src/project/AspNetCore/areas/Home/pages/ApplicationPage/ApplicationPage.scss"],
        outletpage: ["./src/project/AspNetCore/areas/Products/pages/OutletPage/index.js", "./src/project/AspNetCore/areas/Products/pages/OutletPage/OutletPage.scss"],
        newsItemPage: ["./src/project/AspNetCore/areas/News/pages/NewsItemPage/index.js", "./src/project/AspNetCore/areas/News/pages/NewsItemPage/NewsItemPage.scss"],
        newsPage: ["./src/project/AspNetCore/areas/News/pages/NewsPage/index.js", "./src/project/AspNetCore/areas/News/pages/NewsPage/NewsPage.scss"],
        newOrderPage: ["./src/project/AspNetCore/areas/Orders/pages/NewOrder/index.js", "./src/project/AspNetCore/areas/Orders/pages/NewOrder/NewOrderPage.scss"],
        listOrdersPage: ["./src/project/AspNetCore/areas/Orders/pages/ListOrders/index.js", "./src/project/AspNetCore/areas/Orders/pages/ListOrders/ListOrdersPage.scss"],
        statusOrderPage: ["./src/project/AspNetCore/areas/Orders/pages/StatusOrder/index.js", "./src/project/AspNetCore/areas/Orders/pages/StatusOrder/StatusOrderPage.scss"],
        homepage: ["./src/project/AspNetCore/areas/Home/pages/HomePage/index.js", "./src/project/AspNetCore/areas/Home/pages/HomePage/HomePage.scss"],
        categorypage: ["./src/project/AspNetCore/areas/Products/pages/CategoryPage/index.js", "./src/project/AspNetCore/areas/Products/pages/CategoryPage/CategoryPage.scss"],
        searchproductspage: ["./src/project/AspNetCore/areas/Products/pages/SearchProductsPage/index.js", "./src/project/AspNetCore/areas/Products/pages/SearchProductsPage/SearchProductsPage.scss"],
        availableproductspage: ["./src/project/AspNetCore/areas/Products/pages/AvailableProductsPage/index.js", "./src/project/AspNetCore/areas/Products/pages/AvailableProductsPage/AvailableProductsPage.scss"],
        productpage: ["./src/project/AspNetCore/areas/Products/pages/ProductPage/index.js", "./src/project/AspNetCore/areas/Products/pages/ProductPage/ProductPage.scss"]
    },
    output: {
        publicPath: path.resolve(__dirname, "../be/src/Project/Web/Buyer/Buyer.Web/wwwroot/dist/js"),
        path: path.resolve(__dirname, "../be/src/Project/Web/Buyer/Buyer.Web/wwwroot/dist/js"),
        filename: "[name].js"
    }
}