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
        url: '/en',
        text: 'EN'
      },
      {
        url: '/de',
        text: 'DE'
      },
      {
        url: '/pl',
        text: 'PL'
      }
    ],
    selectedLanguageUrl: '/en'
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
  saveUrl: "#",
  saveText: "Save",
  schemas: [
    { name: "Narożniki bez strony", value: "1" },
    { name: "Narożniki ze stroną", value: "2" },
    { name: "Sofy", value: "3" },
    { name: "Szafy", value: "4" }
  ],
  schema: {
    id: "1",
    jsonSchema: {
      "type": "object",
      "required": [
        "firstName",
        "lastName"
      ],
      "properties": {
        "firstName": {
          "type": "string",
          "title": "First name"
        },
        "lastName": {
          "type": "string",
          "title": "Last name"
        }
      }
    },
    uiSchema: {},
    formData: {}
  }
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