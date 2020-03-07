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
    { uniqueId: "1", url: "#price-list", text: "Price List" },
    { uniqueId: "2", url: "#contact", text: "Contact" }
  ],
  loginLink: {
    url: "#", 
    text: "Sign in"
  }
};

var footer = {
  copyright: 'Copyright © 2021 Giuru',
  links: [
    { uniqueId: "1", text: 'Price List', url: '#price-list' },
    { uniqueId: "2", text: 'Contact', url: '#contact' }
  ]
};

export const HomePageStory = () => <HomePage header={header} footer={footer} welcome="Welcome from Storybook" learnMore="Learn more!" />

HomePageStory.story = {
  name: 'Home Page',
};

export default {
  title: 'AspNetCore.Pages',
  component: HomePageStory,
};