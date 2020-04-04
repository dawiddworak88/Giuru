import React from 'react';
import '../project/Tenant.Portal/areas/Clients/pages/ClientDetailPage/ClientDetailPage.scss';
import ClientDetailPage from '../project/Tenant.Portal/areas/Clients/pages/ClientDetailPage/ClientDetailPage';

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

var clientDetailForm = {

  nameLabel: "Name:",
  emailLabel: "E-mail:",
  languageLabel: "Communication language:",
  nameRequiredErrorMessage: "Enter name",
  emailRequiredErrorMessage: "Enter e-mail address",
  languageRequiredErrorMessage: "Select langauge",
  emailFormatErrorMessage: "Enter e-mail in a correct format, e.g. your@email.com",
  passwordFormatErrorMessage: "Password must be at least 8 characters long, consist of at least one capital letter, one small letter, a digit and a special character, e.g. P@ssw0rd",
  clientDetailText: "Client",
  enterNameText: "Enter name",
  enterEmailText: "Enter e-mail",
  enterPasswordText: "Enter password",
  languages: [
    { value: "", text: "Select language" },
    { value: "EN", text: "EN" },
    { value: "DE", text: "DE" },
    { value: "PL", text: "PL" }
  ],
  saveText: "Save"
};

var footer = {
  copyright: 'Copyright © 2021 Giuru',
  links: [
  ]
};

export const ClientDetailPageStory = () => <ClientDetailPage header={header} menuTiles={menuTiles} title="Client" clientDetailForm={clientDetailForm} footer={footer} />

ClientDetailPageStory.story = {
  name: 'Client Detail Page',
};

export default {
  title: 'TenantPortal.Clients',
  component: ClientDetailPageStory,
};