import React from 'react';
import { linkTo } from '@storybook/addon-links';
import { Welcome } from '@storybook/react/demo';
import '../project/AspNetCore/areas/Home/pages/HomePage/HomePage.scss';
import HomePage from '../project/AspNetCore/areas/Home/pages/HomePage/HomePage';

export default {
  title: 'Welcome',
  component: Welcome,
};

export const ToStorybook = () => <Welcome showApp={linkTo('Button')} />;

var header = {

};

export const HomePageStory = () => <HomePage header={header} welcome="Welcome from Storybook" learnMore="Learn more!" />

ToStorybook.story = {
  name: 'to Storybook',
};
