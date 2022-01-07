import React from "react";
import InventoryPage from "../project/Seller.Portal/areas/Inventory/pages/InventoryPage/InventoryPage";
import { header, menuTiles, footer } from "./Shared/Props";
import "../project/Seller.Portal/areas/Inventory/pages/InventoryPage/InventoryPage.scss";

const formData = {
  title: "Inventory",
  newText: "New inventory item",
  newUrl: "#",
  noLabel: "No",
  yesLabel: "Yes",
  deleteConfirmationLabel: "Delete confirmation",
  areYouSureLabel: "Are you sure you want to delete this item",
  generalErrorMessage: "An Error Occurred",
  searchLabel: "Search",
  editLabel: "Edit",
  deleteLabel: "Delete",
  displayedRowsLabel: "of",
  rowsPerPageLabel: "Rows per Page",
  backIconButtonText: "Previous",
  nextIconButtonText: "Next",
  editUrl: "#",
  deleteUrl: "#",
  noResultsLabel: "There are no results",
  table: {
    actions: [
        {
            isEdit: true
        },
        {
            isDelete: false
        }
    ],
    labels: [
      "Sku",
      "Variant",
      "Name",
      "Total",
      "Last change state",
    ],
    properties: [
      {
          title: "sku",
          isDateTime: false
      },
      {
          title: "variant",
          isDateTime: false
      },
      
      {
          title: "name",
          isDateTime: false
      },
      {
        title: "total",
        isDateTime: false
      },
      {
        title: "lastModifiedDate",
        isDateTime: true
      },
    ]
  },
  pagedItems: {
    data: [
        {
            id: "1",
            sku: "An",
            name: "Anton",
            variant: "An27",
            createdDate: new Date(),
            total: 17
        },
    ],
    total: 1
  }
};

export const InventoryPageStory = () => <InventoryPage header={header} menuTiles={menuTiles} footer={footer} catalog={formData} />;

InventoryPageStory.story = {
  name: "Inventory Page",
};

const SellerInventoryStories = {
  title: "SellerPortal.Inventory",
  component: InventoryPageStory,
};

export default SellerInventoryStories;
