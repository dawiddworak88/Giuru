import React from 'react';
import '../project/AspNetCore/areas/Products/pages/CategoryPage/CategoryPage.scss';
import CategoryPage from '../project/AspNetCore/areas/Products/pages/CategoryPage/CategoryPage';
import { header, mainNavigation, footer } from './Shared/AspNetCoreProps';

var catalog = {
  title: 'Sofas',
  resultsCount: 2823,
  resultsLabel: 'results',
  items: [
    {
      id: 1,
      title: 'Alessia',
      imageUrl: 'https://eltap.pl/upload/gallery/190/cay02jpg163.jpg',
      imageAlt: 'Alessia'
    },
    {
      id: 2,
      title: 'Alessia',
      imageUrl: 'https://eltap.pl/upload/gallery/190/cay02jpg163.jpg',
      imageAlt: 'Alessia'
    },
    {
      id: 3,
      title: 'Alessia',
      imageUrl: 'https://eltap.pl/upload/gallery/190/cay02jpg163.jpg',
      imageAlt: 'Alessia'
    },
    {
      id: 4,
      title: 'Alessia',
      imageUrl: 'https://eltap.pl/upload/gallery/190/cay02jpg163.jpg',
      imageAlt: 'Alessia'
    },
    {
      id: 5,
      title: 'Alessia',
      imageUrl: 'https://eltap.pl/upload/gallery/190/cay02jpg163.jpg',
      imageAlt: 'Alessia'
    },
    {
      id: 6,
      title: 'Alessia',
      imageUrl: 'https://eltap.pl/upload/gallery/190/cay02jpg163.jpg',
      imageAlt: 'Alessia'
    },
    {
      id: 7,
      title: 'Alessia',
      imageUrl: 'https://eltap.pl/upload/gallery/190/cay02jpg163.jpg',
      imageAlt: 'Alessia'
    },
    {
      id: 8,
      title: 'Alessia',
      imageUrl: 'https://eltap.pl/upload/gallery/190/cay02jpg163.jpg',
      imageAlt: 'Alessia'
    }
  ]
};

export const CategoryPageStory = () => <CategoryPage header={header} mainNavigation={mainNavigation} catalog={catalog} footer={footer} />

CategoryPageStory.story = {
  name: 'Category Page',
};

export default {
  title: 'AspNetCore.Category',
  component: CategoryPageStory,
};