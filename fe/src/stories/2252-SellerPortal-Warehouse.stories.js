import React from "react";
import WarehousePage from "../project/Seller.Portal/areas/Warehouse/pages/WarehousePage/WarehousePage";
import { header, menuTiles, footer } from "./Shared/Props";
import "../project/Seller.Portal/areas/Warehouse/pages/WarehousePage/WarehousePage.scss";

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

export const WarehousePageStory = () => <WarehousePage header={header} menuTiles={menuTiles} footer={footer} warehouseForm={formData}/>;

WarehousePageStory.story = {
  name: "Warehouse Page",
};

const SellerWarehouseStories = {
  title: "SellerPortal.Warehouse",
  component: WarehousePageStory,
};

export default SellerWarehouseStories;
