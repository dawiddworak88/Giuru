import React from "react";
import OutletPage from "../project/Seller.Portal/areas/Inventory/pages/OutletPage/OutletPage";
import { header, menuTiles, footer } from "./Shared/Props";
import "../project/Seller.Portal/areas/Inventory/pages/OutletPage/OutletPage.scss";

const outletForm = {
  title: "Outlet",
};

export const OutletFormPageStory = () => <OutletPage header={header} menuTiles={menuTiles} footer={footer} outletForm={outletForm} />;

OutletFormPageStory.story = {
  name: "Outlet Form Page",
};

const SellerOutletFormStories = {
  title: "SellerPortal.Outlet",
  component: OutletFormPageStory,
};

export default SellerOutletFormStories;
