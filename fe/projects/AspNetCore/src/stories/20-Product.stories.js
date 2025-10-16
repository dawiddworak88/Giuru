import React from "react";
import "../areas/Products/pages/ProductPage/ProductPage.scss";
import ProductPage from "../areas/Products/pages/ProductPage/ProductPage";
import { header, breadcrumbs, mainNavigation, files, footer } from "./shared/Props";

const mediaItems = [
  {
    imageSrc: "https://eltap.pl/upload/gallery/172/eridano-104080rgbjpg7311.jpg",
    imageAlt: "Sectional",
    mimeType: "image"
  },
  {
    imageSrc: "https://eltap.pl/upload/gallery/65/zamowienie4berlin01soft11200000jpg3793.jpg",
    imageAlt: "Sectional",
    mimeType: "image"
  },
  {
    imageSrc: "https://eltap.pl/upload/gallery/66/beg-04jpg9238.jpg",  
    imageAlt: "Sectional",
    mimeType: "image"
  },
  {
    imageSrc: "https://eltap.pl/upload/gallery/172/eridano-104080rgbjpg7311.jpg",
    imageAlt: "Sectional",
    mimeType: "image"
  },
  {
    imageSrc: "https://eltap.pl/upload/gallery/65/zamowienie4berlin01soft11200000jpg3793.jpg",
    imageAlt: "Sectional",
    mimeType: "image"
  },
  {
    imageSrc: "https://eltap.pl/upload/gallery/66/beg-04jpg9238.jpg",  
    imageAlt: "Sectional",
    mimeType: "image"
  },
  {
    imageSrc: "https://eltap.pl/upload/gallery/172/eridano-104080rgbjpg7311.jpg",
    imageAlt: "Sectional",
    mimeType: "image"
  },
  {
    imageSrc: "https://eltap.pl/upload/gallery/65/zamowienie4berlin01soft11200000jpg3793.jpg",
    imageAlt: "Sectional",
    mimeType: "image"
  },
  {
    imageSrc: "https://eltap.pl/upload/gallery/66/beg-04jpg9238.jpg",
    imageAlt: "Sectional",
    mimeType: "image"
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
    isAuthenticated: true,
    mediaItems,
    signInUrl: "#",
    skuLabel: "SKU:",
    sku: "An_20",
    eanLabel: "EAN:",
    ean: "123089456432",
    byLabel: "by",
    brandName: "eltap",
    brandUrl: "#",
    isProductVariant: false,
    pricesLabel: "Prices:",
    productInformationLabel: "Product and Packagig Information:",
    signInToSeePricesLabel: "Log in to see prices",
    inStock: 11,
    inStockLabel: "In stock: ",
    expectedDeliveryLabel: "Expected delivery: ",
    expectedDelivery: new Date(),
    descriptionLabel: "Description:",
    description: "With more than 20 years of production and design experience, Mounting Dream dedicates to providing various kinds of TV mounts with high quality and first-class service. We always adhere to customer-centric, and win a good reputation in millions of North American families. With more than 20 years of production and design experience, Mounting Dream dedicates to providing various kinds of TV mounts with high quality and first-class service. We always adhere to customer-centric, and win a good reputation in millions of North American families. With more than 20 years of production and design experience, Mounting Dream dedicates to providing various kinds of TV mounts with high quality and first-class service. We always adhere to customer-centric, and win a good reputation in millions of North American families.",
    features: getFeatures(4),
    files,
    basketLabel: "Add to cart",
    addedProduct: "Added",
    basketUrl: "#",
    readMoreText: "Read more",
    readLessText: "Read less",
    seeMoreText: "See more",
    seeLessText: "See less",
    copyToClipboardText: "Copy to clipboard",
    copyTextError: "Copying failed",
    copiedText: "Copied",
    toastSuccessAddProductToBasket: {
      title: "Success added product to basket",
      basketUrl: "#",
      showText: "Show"
    },
    price: {
      current: 999.99,
      old: 1299.99,
      lowestPrice: 899.99,
      currency: "eur",
      lowestPriceLabel: "Lowest price in 30 days:"
    },
    sidebar: {
      lackInformation: "Lack of information",
      toBasketLabel: "View cart",
      notFound: "No variants",
      sidebarTitle: "Add the selected product variant to the cart"
    },
    modal: {
      title: "Anton",
      skuLabel: "Sku:"
    },
    productVariants: [
      {
        id: "5dddbdfb-2a79-412b-7d5f-08d907687b6a",
        title: "Product variants",
        carouselItems: [
          {
            name: "Aderito",
            sku: "Adr05",
            id: "5c5682ad-1d18-407c-e76f-08d9db1eef9f",
            imageAlt: "Aderito 140X200",
            attributes: [
              {
                key: "primaryFabrics",
                name: "Tkanina",
                value: "Kronos 15, Kronos 19"
              },
              {
                key: "secondaryFabrics",
                name: "Tkanina",
                value: "Kronos 15, Kronos 219"
              }
            ]
          },
          {
            name: "Anton",
            sku: "An27",
            id: "5c5682ad-1d18-407c-e76f-08d9db1eef9f",
            imageAlt: "Aderito 140X200",
            attributes: []
          }
        ]
      }
    ]
};

export const ProductPageStory = () => <ProductPage header={header} breadcrumbs={breadcrumbs} mainNavigation={mainNavigation} productDetail={productDetail} footer={footer} />

ProductPageStory.story = {
  name: "Product Page",
};

const ProductStories = {
  title: "Products",
  component: ProductPageStory,
};

export default ProductStories;
