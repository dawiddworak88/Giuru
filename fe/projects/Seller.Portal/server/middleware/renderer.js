import React from "react";
import ReactDOMServer from "react-dom/server";
import { resetServerContext } from 'react-beautiful-dnd';
import { ServerStyleSheets } from "@mui/styles";

import DashboardPage from "../../src/project/Seller.Portal/areas/Dashboard/pages/DashboardPage/DashboardPage";
import CountryPage from "../../src/project/Seller.Portal/areas/Global/pages/CountryPage/CountryPage";
import CountriesPage from "../../src/project/Seller.Portal/areas/Global/pages/CountriesPage/CountriesPage";
import TeamMemberPage from "../../src/project/Seller.Portal/areas/TeamMembers/pages/TeamMember/TeamMember";
import TeamMembersPage from "../../src/project/Seller.Portal/areas/TeamMembers/pages/TeamMembers/TeamMembers";
import DownloadCenterItemPage from "../../src/project/Seller.Portal/areas/DownloadCenter/pages/DownloadCenterItemPage/DownloadCenterItemPage";
import DownloadCenterPage from "../../src/project/Seller.Portal/areas/DownloadCenter/pages/DownloadCenterPage/DownloadCenterPage";
import DownloadCenterCategoryPage from "../../src/project/Seller.Portal/areas/DownloadCenter/pages/DownloadCenterCategoryPage/DownloadCenterCategoryPage";
import DownloadCenterCategoriesPage from "../../src/project/Seller.Portal/areas/DownloadCenter/pages/DownloadCenterCategoriesPage/DownloadCenterCategoriesPage";
import ClientRolePage from "../../src/project/Seller.Portal/areas/Clients/pages/ClientRolePage/ClientRolePage";
import ClientRolesPage from "../../src/project/Seller.Portal/areas/Clients/pages/ClientRolesPage/ClientRolesPage";
import ClientApplicationPage from "../../src/project/Seller.Portal/areas/Clients/pages/ClientApplicationPage/ClientApplicationPage";
import ClientsApplicationsPage from "../../src/project/Seller.Portal/areas/Clients/pages/ClientsApplicationsPage/ClientsApplicationsPage";
import MediaPage from "../../src/project/Seller.Portal/areas/MediaItems/pages/MediaPage/MediaPage";
import MediaItemPage from "../../src/project/Seller.Portal/areas/MediaItems/pages/MediaItemPage/MediaItemPage";
import ClientAccountManagerPage from "../../src/project/Seller.Portal/areas/Clients/pages/ClientAccountManagerPage/ClientAccountManagerPage";
import ClientAccountManagersPage from "../../src/project/Seller.Portal/areas/Clients/pages/ClientAccountManagersPage/ClientAccountManagersPage";
import MediaItemsPage from "../../src/project/Seller.Portal/areas/MediaItems/pages/MediaItemsPage/MediaItemsPage"
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
import OrderItemPage from "../../src/project/Seller.Portal/areas/Orders/pages/OrderItemPage/OrderItemPage";
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
