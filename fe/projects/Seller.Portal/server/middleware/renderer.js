import React from "react";
import ReactDOMServer from "react-dom/server";
import { resetServerContext } from 'react-beautiful-dnd';
import { ServerStyleSheets } from "@mui/styles";

import ProductCardPage from "../../src/areas/Products/pages/ProductCardPage/ProductCardPage";
import ProductCardsPage from "../../src/areas/Products/pages/ProductCardsPage/ProductCardsPage";
import DashboardPage from "../../src/areas/Dashboard/pages/DashboardPage/DashboardPage";
import CountryPage from "../../src/areas/Global/pages/CountryPage/CountryPage";
import CountriesPage from "../../src/areas/Global/pages/CountriesPage/CountriesPage";
import TeamMemberPage from "../../src/areas/TeamMembers/pages/TeamMember/TeamMember";
import TeamMembersPage from "../../src/areas/TeamMembers/pages/TeamMembers/TeamMembers";
import DownloadCenterItemPage from "../../src/areas/DownloadCenter/pages/DownloadCenterItemPage/DownloadCenterItemPage";
import DownloadCenterPage from "../../src/areas/DownloadCenter/pages/DownloadCenterPage/DownloadCenterPage";
import DownloadCenterCategoryPage from "../../src/areas/DownloadCenter/pages/DownloadCenterCategoryPage/DownloadCenterCategoryPage";
import DownloadCenterCategoriesPage from "../../src/areas/DownloadCenter/pages/DownloadCenterCategoriesPage/DownloadCenterCategoriesPage";
import ClientRolePage from "../../src/areas/Clients/pages/ClientRolePage/ClientRolePage";
import ClientRolesPage from "../../src/areas/Clients/pages/ClientRolesPage/ClientRolesPage";
import ClientApplicationPage from "../../src/areas/Clients/pages/ClientApplicationPage/ClientApplicationPage";
import ClientsApplicationsPage from "../../src/areas/Clients/pages/ClientsApplicationsPage/ClientsApplicationsPage";
import MediaPage from "../../src/areas/MediaItems/pages/MediaPage/MediaPage";
import MediaItemPage from "../../src/areas/MediaItems/pages/MediaItemPage/MediaItemPage";
import ClientAccountManagerPage from "../../src/areas/Clients/pages/ClientAccountManagerPage/ClientAccountManagerPage";
import ClientAccountManagersPage from "../../src/areas/Clients/pages/ClientAccountManagersPage/ClientAccountManagersPage";
import MediaItemsPage from "../../src/areas/MediaItems/pages/MediaItemsPage/MediaItemsPage"
import OutletPage from "../../src/areas/Inventory/pages/OutletPage/OutletPage";
import OutletsPage from "../../src/areas/Inventory/pages/OutletsPage/OutletsPage";
import ClientGroupPage from "../../src/areas/Clients/pages/ClientGroupPage/ClientGroupPage";
import ClientGroupsPage from "../../src/areas/Clients/pages/ClientGroupsPage/ClientGroupsPage";
import NewsPage from "../../src/areas/News/pages/NewsPage/NewsPage";
import NewsItemPage from "../../src/areas/News/pages/NewsItemPage/NewsItemPage";
import NewsCategoriesPage from "../../src/areas/News/pages/CategoriesPage/CategoriesPage";
import NewsCategoryPage from "../../src/areas/News/pages/CategoryPage/CategoryPage";
import InventoryPage from "../../src/areas/Inventory/pages/InventoryPage/InventoryPage";
import InventoriesPage from "../../src/areas/Inventory/pages/InventoriesPage/InventoriesPage";
import WarehousePage from "../../src/areas/Inventory/pages/WarehousePage/WarehousePage";
import WarehousesPage from "../../src/areas/Inventory/pages/WarehousesPage/WarehousesPage";
import OrdersPage from "../../src/areas/Orders/pages/OrdersPage/OrdersPage";
import OrderPage from "../../src/areas/Orders/pages/OrderPage/OrderPage";
import OrderItemPage from "../../src/areas/Orders/pages/OrderItemPage/OrderItemPage";
import EditOrderPage from "../../src/areas/Orders/pages/EditOrderPage/EditOrderPage";
import ClientsPage from "../../src/areas/Clients/pages/ClientsPage/ClientsPage";
import ClientPage from "../../src/areas/Clients/pages/ClientPage/ClientPage";
import ProductsPage from "../../src/areas/Products/pages/ProductsPage/ProductsPage";
import ProductPage from "../../src/areas/Products/pages/ProductPage/ProductPage";
import ProductAttributesPage from "../../src/areas/Products/pages/ProductAttributesPage/ProductAttributesPage";
import ProductAttributePage from "../../src/areas/Products/pages/ProductAttributePage/ProductAttributePage";
import ProductAttributeItemPage from "../../src/areas/Products/pages/ProductAttributeItemPage/ProductAttributeItemPage";
import CategoriesPage from "../../src/areas/Products/pages/CategoriesPage/CategoriesPage";
import SellerCategoryPage from "../../src/areas/Products/pages/CategoryPage/CategoryPage";
import SettingsPage from "../../src/areas/Settings/pages/SettingsPage/SettingsPage";
import ClientAddressPage from "../../src/areas/Clients/pages/ClientAddressPage/ClientAddressPage";
import ClientAddressesPage from "../../src/areas/Clients/pages/ClientAddressesPage/ClientAddressesPage";
import ClientApprovalsPage from "../../src/areas/Clients/pages/ClientApprovalsPage/ClientApprovalsPage";
import ClientApprovalPage from "../../src/areas/Clients/pages/ClientApprovalPage/ClientApprovalPage";
import ClientFieldsPage from "../../src/areas/Clients/pages/ClientFieldsPage/ClientFieldsPage";
import ClientFieldPage from "../../src/areas/Clients/pages/ClientFieldPage/ClientFieldPage";
import ClientFieldOptionPage from "../../src/areas/Clients/pages/ClientFieldOptionPage/ClientFieldOptionPage";
import CurrenciesPage from "../../src/areas/Global/pages/CurrenciesPage/CurrenciesPage";
import CurrencyPage from "../../src/areas/Global/pages/CurrencyPage/CurrencyPage";

const Components = {
	ClientFieldOptionPage,
	ClientFieldPage,
	ClientFieldsPage,
	ClientAddressPage,
	ClientAddressesPage,
	ProductCardPage,
	ProductCardsPage,
	DashboardPage,
	CountriesPage,
	CountryPage,
	TeamMemberPage,
	TeamMembersPage,
	DownloadCenterItemPage,
	DownloadCenterPage,
	DownloadCenterCategoryPage,
	DownloadCenterCategoriesPage,
	ClientRolePage,
	ClientRolesPage,
	ClientApplicationPage,
	ClientsApplicationsPage,
	MediaPage,
	MediaItemPage,
	ClientAccountManagerPage,
	ClientAccountManagersPage,
	MediaItemsPage,
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
	OrderItemPage,
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
	SettingsPage,
	ClientApprovalsPage,
	ClientApprovalPage,
	CurrenciesPage,
	CurrencyPage
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
