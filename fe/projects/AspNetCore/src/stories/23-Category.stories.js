import React from "react";
import "../areas/Products/pages/CategoryPage/CategoryPage.scss";
import CategoryPage from "../areas/Products/pages/CategoryPage/CategoryPage";
import { header, mainNavigation, breadcrumbs, footer } from "./shared/Props";

function getItems(length) {

  var items = [];

  for (var i = 0; i < length; i++) {
    
    var item = {
      id: (i + 1),
      sku: (i + 271829),
      title: "Mounting Dream Tilt TV Wall Mount Bracket for Most 37-70 Inches TVs, TV Mount with VESA up to 600x400mm, Fits 16",
      imageUrl: "https://media-test.eltap.com/api/v1/files/3d6baf66-d543-4c81-1a86-08dc52e27976",
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
  pagedItems: getItems(5),
  sidebar: {
    basketUrl: "#"
  },
  filterCollector: {
    allFiltresLabel: "All filtres",
    sortLabel: "Sort by:",
    clearAllFiltresLabel: "Clear all",
    filtresLabel: "Filtres",
    sortItems: [
      {
        label: "Defautl",
        key: "defautl"
      },
      {
        label: "Price: Low to High",
        key: "priceAsc"
      },
      {
        label: "Price: High to Low",
        key: "priceDsc"
      },
      {
        label: "Name",
        key: "name"
      },
      {
        label: "Newest",
        key: "new"
      }
    ],
    filterItems: [
      {
        label: "Price",
        key: "price",
        variants: [
          {
            label: "0-999 EUR",
            value: 1
          },
          {
            label: "1000-1999 EUR",
            value: 2
          },
          {
            label: "2000-2999 EUR",
            value: 3
          },
          {
            label: "3000-3999 EUR",
            value: 4
          },
          {
            label: "more 4000 EUR",
            value: 5
          },
        ]
      },
      {
        label: "Fabric",
        key: "fabric",
        variants: [
          {
            label: "Aspen 20",
            value: 1
          },
          {
            label: "Aspen 05",
            value: 2
          },
          {
            label: "Manhattan 20",
            value: 3
          },
          {
            label: "Touch 24",
            value: 4
          },
          {
            label: "Amore 45",
            value: 5
          },
        ]
      }
    ]
  }
};
 
export const CategoryPageStory = () => <CategoryPage header={header} mainNavigation={mainNavigation} breadcrumbs={breadcrumbs} catalog={catalog} footer={footer} />

CategoryPageStory.story = {
  name: "Category Products Page",
};

const CategoryStories = {
  title: "Products",
  component: CategoryPageStory,
};

export default CategoryStories;