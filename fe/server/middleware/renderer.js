import React from "react";
import ReactDOMServer from "react-dom/server";
import { resetServerContext } from 'react-beautiful-dnd';
import { ServerStyleSheets } from "@material-ui/core/styles";

// AspNetCore
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

// Account
import RegisterPage from "../../src/project/Account/areas/Accounts/pages/Register/RegisterPage";
import ResetPasswordPage from "../../src/project/Account/areas/Accounts/pages/ResetPassword/ResetPasswordPage";
import SignInPage from "../../src/project/Account/areas/Accounts/pages/SignIn/SignInPage";
import SetPasswordPage from "../../src/project/Account/areas/Accounts/pages/SetPassword/SetPasswordPage";
import ContentPage from "../../src/project/Account/areas/Home/pages/Content/ContentPage";

// Seller Portal
import OutletPage from "../../src/project/Seller.Portal/areas/Inventory/pages/OutletPage/OutletPage";
import OutletsPage from "../../src/project/Seller.Portal/areas/Inventory/pages/OutletsPage/OutletsPage";
import ClientGroupPage from "../../src/project/Seller.Portal/areas/Clients/pages/ClientGroupPage/ClientGroupPage";
import ClientGroupsPage from "../../src/project/Seller.Portal/areas/Clients/pages/ClientGroupsPage/ClientGroupsPage";
import NewsPage from "../../src/project/Seller.Portal/areas/News/pages/NewsPage/NewsPage";
import NewsItemPage from "../../src/project/Seller.Portal/areas/News/pages/NewsItemPage/NewsItemPage";
import NewsCategoriesPage from "../../src/project/Seller.Portal/areas/News/pages/CategoriesPage/CategoriesPage";
import NewsCategoryPage from "../../src/project/Seller.Portal/areas/News/pages/CategoryPage/CategoryPage";
import InventoryPage from "../../src/project/Seller.Portal/areas/Inventory/pages/InventoryPage/InventoryPage";
import InventoriesPage from "../../src/project/Seller.Portal/areas/Inventory/pages/InventoriesPage/InventoriesPage";
import WarehousePage from "../../src/project/Seller.Portal/areas/Inventory/pages/WarehousePage/WarehousePage";
import WarehousesPage from "../../src/project/Seller.Portal/areas/Inventory/pages/WarehousesPage/WarehousesPage";
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

	RegisterPage,
	ResetPasswordPage,
	SignInPage,
	SetPasswordPage,
	ContentPage,

	OutletPage,
	OutletsPage,
	ClientGroupPage,
	ClientGroupsPage,
	NewsPage,
	NewsItemPage,
	NewsCategoriesPage,
	NewsCategoryPage,
	InventoryPage,
	InventoriesPage,
	WarehousePage,
	WarehousesPage,
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
