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
        uniqueId: 'EN',
        url: '#',
        text: 'EN'
      },
      {
        uniqueId: 'DE',
        url: '#',
        text: 'DE'
      },
      {
        uniqueId: 'PL',
        url: '#',
        text: 'PL'
      }
    ],
    selectedLanguageUniqueId: 'PL'
  },
  links: [
    { uniqueId: "1", url: "#privacy-policy", text: "Price List" },
    { uniqueId: "2", url: "#regulations", text: "Regulations" }
  ],
  loginLink: {
    url: "#", 
    text: "Sign in"
  }
};

var menuTiles = {
  ordersText: "Zamówienia",
  productsText: "Produkty",
  clientsText: "Klienci",
  settingsText: "Ustawienia"
};

var footer = {
  copyright: 'Copyright © 2021 Giuru',
  links: [
    { uniqueId: "1", url: "#privacy-policy", text: "Price List" },
    { uniqueId: "2", url: "#regulations", text: "Regulations" }
  ]
};

export const OrderPageStory = () => <OrderPage header={header} menuTiles={menuTiles} footer={footer} />

OrderPageStory.story = {
  name: 'Orders Page',
};

export default {
  title: 'TenantPortal.Pages',
  component: OrderPageStory,
};