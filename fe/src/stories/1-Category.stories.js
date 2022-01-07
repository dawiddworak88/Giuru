import React from "react";
import "../project/AspNetCore/areas/Products/pages/CategoryPage/CategoryPage.scss";
import CategoryPage from "../project/AspNetCore/areas/Products/pages/CategoryPage/CategoryPage";
import { header, mainNavigation, breadcrumbs, footer } from "./Shared/AspNetCoreProps";

function getItems(length) {

  var items = [];

  for (var i = 0; i < length; i++) {
    
    var item = {
      id: (i + 1),
      sku: (i + 271829),
      title: "Mounting Dream Tilt TV Wall Mount Bracket for Most 37-70 Inches TVs, TV Mount with VESA up to 600x400mm, Fits 16",
      imageUrl: "https://eltap.pl/upload/gallery/190/cay02jpg163.jpg",
      imageAlt: "Alessia",
      inStock: true,
      brandName: "eltap",
      brandUrl: "#",
    };

    items.push(item);
  }

  return {
    data: items,
    total: length
  };
}

var catalog = {
  title: "Sofas",
  resultsLabel: "results",
  noResultsLabel: "There are no results.",
  skuLabel: "SKU:",
  byLabel: "by",
  inStockLabel: "In Stock",
  isAuthenticated: false,
  signInUrl: "#",
  signInToSeePricesLabel: "Log in to see prices",
  displayedRowsLabel: "of",
  rowsPerPageLabel: "Rows per Page",
  backIconButtonText: "Previous",
  nextIconButtonText: "Next",
  generalErrorMessage: "An Error Occurred",
  productsApiUrl: "#",
  id: "11",
  pagedItems: getItems(100)
};
 
export const CategoryPageStory = () => <CategoryPage header={header} mainNavigation={mainNavigation} breadcrumbs={breadcrumbs} catalog={catalog} footer={footer} />

CategoryPageStory.story = {
  name: "Category Page",
};

const CategoryStories = {
  title: "AspNetCore.Category",
  component: CategoryPageStory,
};

export default CategoryStories;