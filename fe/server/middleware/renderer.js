import React from "react";
import ReactDOMServer from "react-dom/server";
import { ServerStyleSheets } from "@material-ui/core/styles";

// AspNetCore
import HomePage from "../../src/project/AspNetCore/areas/Home/pages/HomePage/HomePage";
import CategoryPage from "../../src/project/AspNetCore/areas/Products/pages/CategoryPage/CategoryPage";
import SearchProductsPage from "../../src/project/AspNetCore/areas/Products/pages/SearchProductsPage/SearchProductsPage";
import BuyerProductPage from "../../src/project/AspNetCore/areas/Products/pages/ProductPage/ProductPage";
import BrandPage from "../../src/project/AspNetCore/areas/Products/pages/BrandPage/BrandPage";

// Account
import SignInPage from "../../src/project/Account/areas/Accounts/pages/SignIn/SignInPage";
import ContentPage from "../../src/project/Account/areas/Home/pages/Content/ContentPage";

// Seller Portal
import OrderPage from "../../src/project/Seller.Portal/areas/Orders/pages/OrderPage/OrderPage";
import OrderDetailPage from "../../src/project/Seller.Portal/areas/Orders/pages/OrderDetailPage/OrderDetailPage";
import ImportOrderPage from "../../src/project/Seller.Portal/areas/Orders/pages/ImportOrderPage/ImportOrderPage";
import ClientsPage from "../../src/project/Seller.Portal/areas/Clients/pages/ClientsPage/ClientsPage";
import ClientPage from "../../src/project/Seller.Portal/areas/Clients/pages/ClientPage/ClientPage";
import ProductsPage from "../../src/project/Seller.Portal/areas/Products/pages/ProductsPage/ProductsPage";
import ProductPage from "../../src/project/Seller.Portal/areas/Products/pages/ProductPage/ProductPage";
import CategoriesPage from "../../src/project/Seller.Portal/areas/Products/pages/CategoriesPage/CategoriesPage";
import SellerCategoryPage from "../../src/project/Seller.Portal/areas/Products/pages/CategoryPage/CategoryPage";

const Components = {

	HomePage,
	CategoryPage,
	SearchProductsPage,
	BuyerProductPage,
	BrandPage,

	SignInPage,
	ContentPage,

	OrderPage,
	OrderDetailPage,
	ImportOrderPage,
	ClientsPage,
	ClientPage,
	ProductsPage,
	ProductPage,
	CategoriesPage,
	SellerCategoryPage
};

export default (req, res, next) => {

	let Component = Components[req.body.moduleName];

	if (Component) {

		const sheets = new ServerStyleSheets();

		ReactDOMServer.renderToString(
			sheets.collect(
				<Component {...req.body.parameters} />
			)
		);

		const css = sheets.toString();

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
