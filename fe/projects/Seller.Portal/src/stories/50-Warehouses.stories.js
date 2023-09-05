import React from "react";
import WarehousesPage from "../areas/Inventory/pages/WarehousesPage/WarehousesPage";
import { header, menuTiles, footer } from "./shared/Props";
import "../areas/Inventory/pages/WarehousesPage/WarehousesPage.scss";

const formData = {
  title: "Warehouses",
  newText: "New warehouse",
  newUrl: "#",
  noLabel: "No",
  yesLabel: "Yes",
  deleteConfirmationLabel: "Delete confirmation",
  areYouSureLabel: "Are you sure you want to delete this item",
  generalErrorMessage: "An Error Occurred",
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
          isDelete: true
      }
    ],
    labels: [
      "Warehouse",
      "Total",
      "Last Modified Date",
      "Created Date",
    ],
    properties: [
      {
          title: "warehouse",
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
      {
        title: "createdDated",
        isDateTime: true
      },
    ]
  },
  pagedItems: {
    data: [
        {
            id: "1",
            warehouse: "Magazyn 1",
            total: 17,
            lastModifiedDate: new Date(),
            createdDate: new Date(),
        },
    ],
    total: 1
  }
};

export const WarehousesPageStory = () => <WarehousesPage header={header} menuTiles={menuTiles} footer={footer} catalog={formData}/>;

WarehousesPageStory.story = {
  name: "Warehouses Page",
};

const SellerWarehousesStories = {
  title: "Warehouses",
  component: WarehousesPageStory,
};

export default SellerWarehousesStories;
