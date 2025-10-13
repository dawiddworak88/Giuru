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
  noFilteredResultsLabel: "Sorry, we couldn't find what you were looking for. Try selecting different parameters.",
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
    allFilters: "All filters",
    sortLabel: "Sort by:",
    clearAllFilters: "Clear all",
    seeResult: "See results",
    filtersLabel: "Filters",
    sortItems: [
      {
        label: "Default",
        key: "default"
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
    filterInputs: [
      {
        label: "Price",
        key: "price",
        isNested: false,
        items: [
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
        isNested: false,
        items: [
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
      },
      {
        label: "Sizes",
        key: "sizes",
        isNested: true,
        items: [
          {
            label: "Height",
            key: "height",
            items: [
              {
                label: "0-99",
                value: 1
              },
              {
                label: "100-149",
                value: 2
              },
              {
                label: "150-199",
                value: 3
              },
              {
                label: "200+",
                value: 4
              }
            ]
          },
          {
            label: "Width",
            key: "width",
            items: [
              {
                label: "0-99",
                value: 1
              },
              {
                label: "100-149",
                value: 2
              },
              {
                label: "150-199",
                value: 3
              },
              {
                label: "200+",
                value: 4
              }
            ]
          },
          {
            label: "Depth",
            key: "depth",
            items: [
              {
                label: "0-99",
                value: 1
              },
              {
                label: "100-149",
                value: 2
              },
              {
                label: "150-199",
                value: 3
              },
              {
                label: "200+",
                value: 4
              }
            ]
          }
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