import React from "react";
import InventoryAddPage from "../project/Seller.Portal/areas/Inventory/pages/InventoryAddPage/InventoryAddPage";
import { header, menuTiles, footer } from "./Shared/Props";
import "../project/Seller.Portal/areas/Inventory/pages/InventoryAddPage/InventoryAddPage.scss";

const formData = {
  title: "Inventory",
  saveText: "Save",
  selectProductLabel: "Select product",
  selectWarehouseLabel: "Select warehouse",
  quantityLabel: "Quantity",
  expectedDeliveryLabel: "Expected delivery",
  availableQuantityLabel: "Available quantity",
  restockableInDaysLabel: "Restockable in days",
  saveUrl: "#",
  inventoryUrl: "#",
  generalErrorMessage: "An error has occurred.",
  warehouseRequiredErrorMessage: "Select warehouse name",
  productRequiredErrorMessage: "Enter product name or variant",
  quantityRequiredErrorMessage: "Enter quantity",
  quantityFormatErrorMessage: "Quantity must be bigger than negative values",
  selectWarehouse: "Select warehouse",
  products: [
    {
      id: "62BA2504-9C83-4065-F4A4-08D9AB53BC68",
      name: "Anton",
      sku: "An01",
    },
    {
      id: "62BA2504-9C83-4065-F4A4-08D9AB53BC62",
      name: "Anton2",
      sku: "An02",
    },
    {
      id: "62BA2504-9C83-4065-F4A4-08D9AB53BC63",
      name: "Anton3",
      sku: "An03",
    }
  ],
  warehouses: [
    {
      id: "62BA2504-9C83-4065-F4A4-08D9AB53BC68",
      name: "Magazyn I"
    },
    {
      id: "3FA85F64-5717-4562-B3FC-2C963F66AFA6",
      name: "Magazyn II"
    },
  ]
};

export const InventoryAddPageStory = () => <InventoryAddPage header={header} menuTiles={menuTiles} footer={footer} inventoryForm={formData} />;

InventoryAddPageStory.story = {
  name: "Inventory Add Page",
};

const SellerInventoryAddStories = {
  title: "SellerPortal.Inventory",
  component: InventoryAddPageStory,
};

export default SellerInventoryAddStories;
