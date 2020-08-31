import React from 'react';
import '../project/AspNetCore/areas/Products/pages/CategoryPage/CategoryPage.scss';
import CategoryPage from '../project/AspNetCore/areas/Products/pages/CategoryPage/CategoryPage';
import { header, mainNavigation, footer } from './Shared/AspNetCoreProps';

function getItems(length) {

  var items = [];

  for (var i = 0; i < length; i++) {
    
    var item = {
      id: (i + 1),
      sku: (i + 271829),
      title: 'Mounting Dream Tilt TV Wall Mount Bracket for Most 37-70 Inches TVs, TV Mount with VESA up to 600x400mm, Fits 16"',
      imageUrl: 'https://eltap.pl/upload/gallery/190/cay02jpg163.jpg',
      imageAlt: 'Alessia',
      inStock: true,
      brand: {
        name: "eltap",
        url: '#'
      }
    };

    items.push(item);
  }

  return items;
}

var catalog = {
  title: 'Sofas',
  resultsCount: 2823,
  resultsLabel: 'results',
  noResultsLabel: "There are no results.",
  skuLabel: 'SKU:',
  byLabel: 'by',
  inStockLabel: 'In Stock',
  isAuthenticated: false,
  signInUrl: '#',
  signInToSeePricesLabel: 'Log in to see prices',
  items: getItems(48)
};

export const CategoryPageStory = () => <CategoryPage header={header} mainNavigation={mainNavigation} catalog={catalog} footer={footer} />

CategoryPageStory.story = {
  name: 'Category Page',
};

export default {
  title: 'AspNetCore.Category',
  component: CategoryPageStory,
};