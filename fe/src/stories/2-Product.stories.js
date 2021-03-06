import React from "react";
import "../project/AspNetCore/areas/Products/pages/ProductPage/ProductPage.scss";
import ProductPage from "../project/AspNetCore/areas/Products/pages/ProductPage/ProductPage";
import { header, breadcrumbs, mainNavigation, files, footer } from "./Shared/AspNetCoreProps";

const images = [
  {
    original: "https://eltap.pl/upload/gallery/172/eridano-104080rgbjpg7311.jpg",
    thumbnail: "https://eltap.pl/upload/gallery/172/eridano-104080rgbjpg7311.jpg",
  },
  {
    original: "https://eltap.pl/upload/gallery/65/zamowienie4berlin01soft11200000jpg3793.jpg",
    thumbnail: "https://eltap.pl/upload/gallery/65/zamowienie4berlin01soft11200000jpg3793.jpg",
  },
  {
    original: "https://eltap.pl/upload/gallery/66/beg-04jpg9238.jpg",
    thumbnail: "https://eltap.pl/upload/gallery/66/beg-04jpg9238.jpg",
  }
];

function getFeatures(count) {

  var features = [];

  for (var i = 0; i < count; i++) {

    var feature = {
      key: "Term",
      value: "Value"
    };

    features.push(feature);
  }

  return features;

}

var productDetail = {

    title: "Mounting Dream Tilt TV Wall Mount Bracket for Most 37-70 Inches TVs, TV Mount with VESA up to 600x400mm, Fits 16",
    isAuthenticated: false,
    images,
    signInUrl: "#",
    skuLabel: "SKU:",
    sku: "23829",
    byLabel: "by",
    brandName: "eltap",
    brandUrl: "#",
    pricesLabel: "Prices:",
    productInformationLabel: "Product and Packagig Information:",
    signInToSeePricesLabel: "Log in to see prices",
    inStock: true,
    descriptionLabel: "Description:",
    description: "With more than 20 years of production and design experience, Mounting Dream dedicates to providing various kinds of TV mounts with high quality and first-class service. We always adhere to customer-centric, and win a good reputation in millions of North American families. With more than 20 years of production and design experience, Mounting Dream dedicates to providing various kinds of TV mounts with high quality and first-class service. We always adhere to customer-centric, and win a good reputation in millions of North American families. With more than 20 years of production and design experience, Mounting Dream dedicates to providing various kinds of TV mounts with high quality and first-class service. We always adhere to customer-centric, and win a good reputation in millions of North American families.",
    inStockLabel: "In Stock",
    features: getFeatures(11),
    files
};

export const ProductPageStory = () => <ProductPage header={header} breadcrumbs={breadcrumbs} mainNavigation={mainNavigation} productDetail={productDetail} footer={footer} />

ProductPageStory.story = {
  name: "Product Page",
};

export default {
  title: "AspNetCore.Product",
  component: ProductPageStory,
};
