import React from 'react';
import '../project/Tenant.Portal/areas/Products/pages/ProductDetailPage/ProductDetailPage.scss';
import ProductDetailPage from '../project/Tenant.Portal/areas/Products/pages/ProductDetailPage/ProductDetailPage';

var header = {
  logo: {
    targetUrl: '/',
    logoAltLabel: 'Logo'
  },
  languageSwitcher: {
    availableLanguages: [
      {
        url: '#',
        text: 'EN'
      },
      {
        url: '#',
        text: 'DE'
      },
      {
        url: '#',
        text: 'PL'
      }
    ],
    selectedLanguageText: 'EN'
  },
  links: [
  ],
  loginLink: {
    url: "#", 
    text: "Sign in"
  }
};

var menuTiles = {
  tiles: [
    { icon: 'ShoppingCart', title: 'Orders', url: '#' },
    { icon: 'Package', title: 'Products', url: '#' },
    { icon: 'Users', title: 'Clients', url: '#' },
    { icon: 'Settings', title: 'Settings', url: '#' }
  ],
};

var productDetailForm = {

  nameLabel: "Name:",
  nameRequiredErrorMessage: "Enter name",
  enterNameText: "Enter name",
  selectSchemaLabel: "Select schema",
  skuLabel: "SKU:",
  skuRequiredErrorMessage: "Enter SKU",
  enterSkuText: "Enter SKU",
  schemas: [],
  saveUrl: "#",
  saveText: "Save"
};

var footer = {
  copyright: 'Copyright © 2021 Giuru',
  links: [
  ]
};

export const ProductDetailPageStory = () => <ProductDetailPage header={header} menuTiles={menuTiles} title="Product" productDetailForm={productDetailForm} footer={footer} />

ProductDetailPageStory.story = {
  name: 'Product Detail Page',
};

export default {
  title: 'TenantPortal.Products',
  component: ProductDetailPageStory,
};