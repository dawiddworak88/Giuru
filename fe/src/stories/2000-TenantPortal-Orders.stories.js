import React from 'react';
import '../project/Tenant.Portal/areas/Orders/pages/OrderPage/OrderPage.scss';
import OrderPage from '../project/Tenant.Portal/areas/Orders/pages/OrderPage/OrderPage';

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

var orderCatalog = {
  title: 'Orders',
  newText: 'New order'
};

var footer = {
  copyright: 'Copyright © 2021 Giuru',
  links: [
  ]
};

export const OrderPageStory = () => <OrderPage header={header} menuTiles={menuTiles} catalog={orderCatalog} footer={footer} />

OrderPageStory.story = {
  name: 'Orders Page',
};

export default {
  title: 'TenantPortal.Pages',
  component: OrderPageStory,
};