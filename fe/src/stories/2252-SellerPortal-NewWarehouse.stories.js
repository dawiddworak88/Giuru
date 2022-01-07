import React from "react";
import WarehouseAddPage from "../project/Seller.Portal/areas/Warehouse/pages/WarehouseAddPage/WarehouseAddPage";
import { header, menuTiles, footer } from "./Shared/Props";
import "../project/Seller.Portal/areas/Warehouse/pages/WarehouseAddPage/WarehouseAddPage.scss";

const formData = {
  title: "Warehouse",
  nameLabel: "Name",
  locationLabel: "Location",
  saveText: "Save",
  warehouseUrl: "#",
  generalErrorMessage: "An Error Occurred",
  navigateToWarehouseListText: "Back to warehouse",
  nameRequiredErrorMessage: "Enter warehouse name",
  locationRequiredErrorMessage: "Enter warehouse location",
  saveUrl: "#",
};

export const WarehouseAddPageStory = () => <WarehouseAddPage header={header} menuTiles={menuTiles} footer={footer} warehouseForm={formData}/>;

WarehouseAddPageStory.story = {
  name: "New Warehouse Page",
};

const SellerWarehouseAddStories = {
  title: "SellerPortal.Warehouse",
  component: WarehouseAddPageStory,
};

export default SellerWarehouseAddStories;
