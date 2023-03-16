import React from "react";
import ReactDOMServer from "react-dom/server";
import { resetServerContext } from 'react-beautiful-dnd';
import { ServerStyleSheets } from "@mui/styles";

import BuyerOrderItemPage from "../../src/project/AspNetCore/areas/Orders/pages/OrderItemPage/OrderItemPage";
import BuyerDashboardPage from "../../src/project/AspNetCore/areas/Dashboard/pages/DashboardPage/DashboardPage";
import DownloadCenterBuyerCategoryPage from "../../src/project/AspNetCore/areas/DownloadCenter/pages/DownloadCenterCategoryPage/DownloadCenterCategoryPage";
import DownloadCenterBuyerPage from "../../src/project/AspNetCore/areas/DownloadCenter/pages/DownloadCenterPage/DownloadCenterPage";
import ApplicationPage from "../../src/project/AspNetCore/areas/Home/pages/ApplicationPage/ApplicationPage";
import OutletCatalogPage from "../../src/project/AspNetCore/areas/Products/pages/OutletPage/OutletPage";
import NewsItemDetails from "../../src/project/AspNetCore/areas/News/pages/NewsItemPage/NewsItemPage";
import NewsBuyerPage from "../../src/project/AspNetCore/areas/News/pages/NewsPage/NewsPage";
import NewOrderPage from "../../src/project/AspNetCore/areas/Orders/pages/NewOrder/NewOrderPage";
import ListOrdersPage from "../../src/project/AspNetCore/areas/Orders/pages/ListOrders/ListOrdersPage";
import StatusOrderPage from "../../src/project/AspNetCore/areas/Orders/pages/StatusOrder/StatusOrderPage";
import HomePage from "../../src/project/AspNetCore/areas/Home/pages/HomePage/HomePage";
import CategoryPage from "../../src/project/AspNetCore/areas/Products/pages/CategoryPage/CategoryPage";
import SearchProductsPage from "../../src/project/AspNetCore/areas/Products/pages/SearchProductsPage/SearchProductsPage";
import AvailableProductsPage from "../../src/project/AspNetCore/areas/Products/pages/AvailableProductsPage/AvailableProductsPage";
import BuyerProductPage from "../../src/project/AspNetCore/areas/Products/pages/ProductPage/ProductPage";

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
	BuyerProductPage
};

const serverRenderer = (req, res, next) => {

	let Component = Components[req.body.moduleName];

	if (Component) {

		const sheets = new ServerStyleSheets();

		ReactDOMServer.renderToString(
			sheets.collect(
				<Component {...req.body.parameters} />
			)
		);

		const css = sheets.toString();

		resetServerContext();

		return res.send(
			ReactDOMServer.renderToString(
				<React.Fragment>
					{css &&
						<style id="jss-server-side">
							{css}
						</style>
					}
					<Component {...req.body.parameters} />
				</React.Fragment>
			));
	}

	res.status(400).end();
};

export default serverRenderer;
