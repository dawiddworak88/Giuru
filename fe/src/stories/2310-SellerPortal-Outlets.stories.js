import React from "react";
import OutletsPage from "../project/Seller.Portal/areas/Outlet/pages/OutletsPage/OutletsPage";
import { header, menuTiles, footer } from "./Shared/Props";
import "../project/Seller.Portal/areas/Outlet/pages/OutletsPage/OutletsPage.scss";

const catalog = {
  title: "Outlet",
  newText: "New outlet item",
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

export const OutletsPageStory = () => <OutletsPage header={header} menuTiles={menuTiles} footer={footer} catalog={catalog} />;

OutletPageStory.story = {
  name: "Outlet Page",
};

const SellerOutletsStories = {
  title: "SellerPortal.Outlet",
  component: OutletsPageStory,
};

export default SellerOutletsStories;
