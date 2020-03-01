import React from 'react';
import '../project/AspNetCore/areas/Home/pages/HomePage/HomePage.scss';
import HomePage from '../project/AspNetCore/areas/Home/pages/HomePage/HomePage';

var header = {
  logo: {
    targetUrl: '/',
    logoAltLabel: 'Logo'
  }
};

export const HomePageStory = () => <HomePage header={header} welcome="Welcome from Storybook" learnMore="Learn more!" />

HomePageStory.story = {
  name: 'Home Page',
};

export default {
  title: 'AspNetCore.Pages',
  component: HomePageStory,
};