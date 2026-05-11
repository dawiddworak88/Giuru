import React from "react";
import ReactDOMServer from "react-dom/server";
import { CacheProvider } from "@emotion/react";
import createCache from "@emotion/cache";
import createEmotionServer from "@emotion/server/create-instance";

import BuyerOrderItemPage from "../../src/areas/Orders/pages/OrderItemPage/OrderItemPage";
import BuyerDashboardPage from "../../src/areas/Dashboard/pages/DashboardPage/DashboardPage";
import DownloadCenterBuyerCategoryPage from "../../src/areas/DownloadCenter/pages/DownloadCenterCategoryPage/DownloadCenterCategoryPage";
import DownloadCenterBuyerPage from "../../src/areas/DownloadCenter/pages/DownloadCenterPage/DownloadCenterPage";
import ApplicationPage from "../../src/areas/Home/pages/ApplicationPage/ApplicationPage";
import OutletCatalogPage from "../../src/areas/Products/pages/OutletPage/OutletPage";
import NewsItemDetails from "../../src/areas/News/pages/NewsItemPage/NewsItemPage";
import NewsBuyerPage from "../../src/areas/News/pages/NewsPage/NewsPage";
import NewOrderPage from "../../src/areas/Orders/pages/NewOrder/NewOrderPage";
import ListOrdersPage from "../../src/areas/Orders/pages/ListOrders/ListOrdersPage";
import StatusOrderPage from "../../src/areas/Orders/pages/StatusOrder/StatusOrderPage";
import HomePage from "../../src/areas/Home/pages/HomePage/HomePage";
import CategoryPage from "../../src/areas/Products/pages/CategoryPage/CategoryPage";
import SearchProductsPage from "../../src/areas/Products/pages/SearchProductsPage/SearchProductsPage";
import AvailableProductsPage from "../../src/areas/Products/pages/AvailableProductsPage/AvailableProductsPage";
import BuyerProductPage from "../../src/areas/Products/pages/ProductPage/ProductPage";
import SlugPage from "../../src/areas/Content/pages/SlugPage/SlugPage";

const Components = {
	BuyerOrderItemPage,
	BuyerDashboardPage,
	DownloadCenterBuyerCategoryPage,
	DownloadCenterBuyerPage,
	ApplicationPage,
	OutletCatalogPage,
	NewsItemDetails,
	NewsBuyerPage,
	ListOrdersPage,
	NewOrderPage,
	StatusOrderPage,
	HomePage,
	CategoryPage,
	SearchProductsPage,
	AvailableProductsPage,
	BuyerProductPage,
	SlugPage
};

const serverRenderer = (req, res, next) => {

	let Component = Components[req.body.moduleName];

	if (Component) {

		const cache = createCache({ key: "css" });
		const { extractCriticalToChunks, constructStyleTagsFromChunks } = createEmotionServer(cache);

		const html = ReactDOMServer.renderToString(
			<CacheProvider value={cache}>
				<Component {...req.body.parameters} />
			</CacheProvider>
		);

		const emotionChunks = extractCriticalToChunks(html);
		const emotionCss = constructStyleTagsFromChunks(emotionChunks);

		return res.send(emotionCss + html);
	}

	res.status(400).end();
};

export default serverRenderer;