import React from "react";
import "../project/Seller.Portal/areas/Products/pages/ProductPage/ProductPage.scss";
import ProductsPage from "../project/Seller.Portal/areas/Products/pages/ProductsPage/ProductsPage";
import { header, menuTiles, footer } from "./Shared/Props";

var catalog = {
  title: "Products",
  newText: "New product",
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
            isDelete: true
        }
    ],
    labels: [
        "Sku",
        "Name",
        "Last modified date",
        "Created date"
    ],
    properties: [
        {
            title: "sku",
            isDateTime: false
        },
        {
            title: "name",
            isDateTime: false
        },
        {
            title: "lastModifiedDate",
            isDateTime: true
        },
        {
            title: "createdDate",
            isDateTime: true
        }
    ]
  },
  pagedItems: {
    data: [
      {
        id: "1",
        sku: "Berg01",
        name: "Bergamo",
        lastModifiedDate: new Date(),
        createdDate: new Date(),
      } 
    ],
    total: 1
  }
};

export const ProductsPageStory = () => <ProductsPage header={header} menuTiles={menuTiles} catalog={catalog} footer={footer} />

ProductsPageStory.story = {
  name: "Products Page",
};

const SellerProductStories = {
  title: "SellerPortal.Products",
  component: ProductsPageStory,
};

export default SellerProductStories;