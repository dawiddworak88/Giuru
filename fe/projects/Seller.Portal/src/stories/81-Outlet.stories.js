import React from "react";
import OutletPage from "../areas/Inventory/pages/OutletPage/OutletPage";
import { header, menuTiles, footer } from "./shared/Props";
import "../areas/Inventory/pages/OutletPage/OutletPage.scss";

const outletForm = {
  title: "Outlet",
};

export const OutletFormPageStory = () => <OutletPage header={header} menuTiles={menuTiles} footer={footer} outletForm={outletForm} />;

OutletFormPageStory.story = {
  name: "OutletItem Page",
};

const SellerOutletFormStories = {
  title: "Inventories",
  component: OutletFormPageStory,
};

export default SellerOutletFormStories;
