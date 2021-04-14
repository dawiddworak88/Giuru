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
import OrdersPage from "../../src/project/Seller.Portal/areas/Orders/pages/OrdersPage/OrdersPage";
import OrderPage from "../../src/project/Seller.Portal/areas/Orders/pages/OrderPage/OrderPage";
import EditOrderPage from "../../src/project/Seller.Portal/areas/Orders/pages/EditOrderPage/EditOrderPage";
import ClientsPage from "../../src/project/Seller.Portal/areas/Clients/pages/ClientsPage/ClientsPage";
import ClientPage from "../../src/project/Seller.Portal/areas/Clients/pages/ClientPage/ClientPage";
import ProductsPage from "../../src/project/Seller.Portal/areas/Products/pages/ProductsPage/ProductsPage";
import ProductPage from "../../src/project/Seller.Portal/areas/Products/pages/ProductPage/ProductPage";
import ProductAttributesPage from "../../src/project/Seller.Portal/areas/Products/pages/ProductAttributesPage/ProductAttributesPage";
import ProductAttributePage from "../../src/project/Seller.Portal/areas/Products/pages/ProductAttributePage/ProductAttributePage";
import ProductAttributeItemPage from "../../src/project/Seller.Portal/areas/Products/pages/ProductAttributeItemPage/ProductAttributeItemPage";
import CategoriesPage from "../../src/project/Seller.Portal/areas/Products/pages/CategoriesPage/CategoriesPage";
import SellerCategoryPage from "../../src/project/Seller.Portal/areas/Products/pages/CategoryPage/CategoryPage";
import SettingsPage from "../../src/project/Seller.Portal/areas/Settings/pages/SettingsPage/SettingsPage";

const Components = {

	HomePage,
	CategoryPage,
	SearchProductsPage,
	BuyerProductPage,
	BrandPage,

	SignInPage,
	ContentPage,

	OrdersPage,
	OrderPage,
	EditOrderPage,
	ClientsPage,
	ClientPage,
	ProductsPage,
	ProductPage,
	ProductAttributesPage,
	ProductAttributePage,
	ProductAttributeItemPage,
	CategoriesPage,
	SellerCategoryPage,
	SettingsPage
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
