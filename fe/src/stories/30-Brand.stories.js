import React from "react";
import "../project/AspNetCore/areas/Products/pages/BrandPage/BrandPage.scss";
import BrandPage from "../project/AspNetCore/areas/Products/pages/BrandPage/BrandPage";
import { header, mainNavigation, breadcrumbs, files, footer } from "./Shared/AspNetCoreProps";

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
      brand: {
        name: "eltap",
        url: "#"
      }
    };

    items.push(item);
  }

  return items;
}

var catalog = {
  title: "Eltap",
  resultsCount: 2823,
  resultsLabel: "results",
  noResultsLabel: "There are no results.",
  skuLabel: "SKU:",
  byLabel: "by",
  inStockLabel: "In Stock",
  isAuthenticated: false,
  signInUrl: "#",
  signInToSeePricesLabel: "Log in to see prices",
  pagedItems: getItems(20)
};

var brandDetail = {
    name: "eltap",
    logoUrl: "https://eltap.pl/templates/default/img/logo.jpg",
    description: "The ELTAP upholstered furniture factory was established in 1993. Its founder is Leszek Dworak, who, together with his wife, ran the company until 2020 - since then the company changed its legal status to ELTAP Spółka z ograniczoną odpowiedzialnością Sp.k. which the son joined. Over the years, as a result of development, the company has gained the name of a well-known and respected brand on the Polish and international market.",
    files
};

export const BrandPageStory = () => <BrandPage header={header} mainNavigation={mainNavigation} breadcrumbs={breadcrumbs} brandDetail={brandDetail} catalog={catalog} footer={footer} />

BrandPageStory.story = {
  name: "Brand Page",
};

const BrandStories = {
  title: "AspNetCore.Brand",
  component: BrandPageStory,
};

export default BrandStories;
