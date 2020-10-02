import React from "react";
import ReactDOMServer from "react-dom/server";
import { ServerStyleSheets } from "@material-ui/core/styles";

// AspNetCore
import HomePage from "../../src/project/AspNetCore/areas/Home/pages/HomePage/HomePage";
import CategoryPage from "../../src/project/AspNetCore/areas/Products/pages/CategoryPage/CategoryPage";
import BuyerProductPage from "../../src/project/AspNetCore/areas/Products/pages/ProductPage/ProductPage";
import BrandPage from "../../src/project/AspNetCore/areas/Brands/pages/BrandPage/BrandPage";

// Account
import SignInPage from "../../src/project/Account/areas/Accounts/pages/SignIn/SignInPage";

// Seller Portal
import OrderPage from "../../src/project/Seller.Portal/areas/Orders/pages/OrderPage/OrderPage";
import OrderDetailPage from "../../src/project/Seller.Portal/areas/Orders/pages/OrderDetailPage/OrderDetailPage";
import ImportOrderPage from "../../src/project/Seller.Portal/areas/Orders/pages/ImportOrderPage/ImportOrderPage";
import ClientPage from "../../src/project/Seller.Portal/areas/Clients/pages/ClientPage/ClientPage";
import ClientDetailPage from "../../src/project/Seller.Portal/areas/Clients/pages/ClientDetailPage/ClientDetailPage";
import ProductPage from "../../src/project/Seller.Portal/areas/Products/pages/ProductPage/ProductPage";
import ProductDetailPage from "../../src/project/Seller.Portal/areas/Products/pages/ProductDetailPage/ProductDetailPage";

const Components = {
	HomePage,
	CategoryPage,
	BuyerProductPage,
	BrandPage,
	SignInPage,
	OrderPage,
	OrderDetailPage,
	ImportOrderPage,
	ClientPage,
	ClientDetailPage,
	ProductPage,
	ProductDetailPage
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
