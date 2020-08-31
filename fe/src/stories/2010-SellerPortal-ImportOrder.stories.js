import React from "react";
import "../project/Seller.Portal/areas/Orders/pages/ImportOrderPage/ImportOrderPage.scss";
import ImportOrderPage from "../project/Seller.Portal/areas/Orders/pages/ImportOrderPage/ImportOrderPage";
import { header, menuTiles, footer } from "./Shared/Props";

var props = {
  title: "Import Order",
  importOrderForm: {

    saveText: "Import",
    validateOrderUrl: "#",
    generalErrorMessage: "An error has occurred",
    selectClientLabel: "Select a client",
    dropFilesLabel: "Drop the files here ...",
    dropOrSelectFilesLabel: "Drag and drop some files here, or click to select files",
    clients: [
      { name: "Hencel" },
      { name: "Pigu" }
    ]
  }
};

export const ImportOrderPageStory = () => <ImportOrderPage header={header} menuTiles={menuTiles} {...props} footer={footer} />

ImportOrderPageStory.story = {
  name: "Import Order Page",
};

export default {
  title: "SellerPortal.Import Order",
  component: ImportOrderPageStory,
};