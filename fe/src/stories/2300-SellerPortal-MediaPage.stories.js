import React from "react";
import MediaPage from "../project/Seller.Portal/areas/Media/pages/MediaPage/MediaPage";
import { header, menuTiles, footer } from "./Shared/Props";
import "../project/Seller.Portal/areas/Media/pages/MediaPage/MediaPage.scss";

const formData = {
  title: "Media",
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

export const MediaPageStory = () => <MediaPage header={header} menuTiles={menuTiles} footer={footer} catalog={formData} />;

MediaPageStory.story = {
  name: "Media Page",
};

const SellerInventoryStories = {
  title: "SellerPortal.Media",
  component: MediaPageStory,
};

export default SellerInventoryStories;
