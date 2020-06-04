import React from 'react';
import '../project/Tenant.Portal/areas/Products/pages/ProductPage/ProductPage.scss';
import ProductPage from '../project/Tenant.Portal/areas/Products/pages/ProductPage/ProductPage';

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

var props = {
  title: 'Products',
  showNew: true,
  newText: 'New product',
  newUrl: '#',
  searchLabel: 'Search',
  editUrl: '#',
  deleteUrl: '#'
};

var footer = {
  copyright: 'Copyright © 2021 Giuru',
  links: [
  ]
};

export const ProductPageStory = () => <ProductPage header={header} menuTiles={menuTiles} {...props} footer={footer} />

ProductPageStory.story = {
  name: 'Products Page',
};

export default {
  title: 'TenantPortal.Products',
  component: ProductPageStory,
};