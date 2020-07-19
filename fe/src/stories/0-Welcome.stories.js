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
  searchUrl: '#',
  searchLabel: 'Search',
  searchPlaceholderLabel: 'Search',
  loginLink: {
    url: "#", 
    text: "Sign in"
  }
};

var mainNavigation = {
  links: [
    { url: '#', text: 'Categories' },
    { url: '#', text: 'Sell' }
  ]
}

var heroSlider = {
  items: [
    { imageSrc: 'https://eltap.pl/upload/gallery/55/marinosavana05soft11okajpg6870.jpg', imageAlt: 'Best sectionals', imageTitle: 'Best sectionals', teaserTitle: 'Shop sectionals', teaserText: 'Best sectionals in the industry', ctaUrl: '#', ctaText: 'Shop now!' },
    { imageSrc: 'https://eltap.pl/upload/gallery/83/sofa-neva01197rgbjpg8615.jpg', imageAlt: 'Best sectionals', imageTitle: 'Best sectionals' }
  ]
}

var footer = {
  copyright: 'Copyright © 2021 Giuru',
  links: [
    { text: 'Price List', url: '#price-list' },
    { text: 'Contact', url: '#contact' }
  ]
};

export const HomePageStory = () => <HomePage header={header} mainNavigation={mainNavigation} heroSlider={heroSlider} footer={footer} />

HomePageStory.story = {
  name: 'Home Page',
};

export default {
  title: 'AspNetCore.Pages',
  component: HomePageStory,
};