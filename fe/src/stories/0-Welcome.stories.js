import React from 'react';
import '../project/AspNetCore/areas/Home/pages/HomePage/HomePage.scss';
import HomePage from '../project/AspNetCore/areas/Home/pages/HomePage/HomePage';

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
    { url: "#price-list", text: "Price List" },
    { url: "#contact", text: "Contact" }
  ],
  loginLink: {
    url: "#", 
    text: "Sign in"
  }
};

var footer = {
  copyright: 'Copyright © 2021 Giuru',
  links: [
    { text: 'Price List', url: '#price-list' },
    { text: 'Contact', url: '#contact' }
  ]
};

export const HomePageStory = () => <HomePage header={header} footer={footer} />

HomePageStory.story = {
  name: 'Home Page',
};

export default {
  title: 'AspNetCore.Pages',
  component: HomePageStory,
};