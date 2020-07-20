import React from 'react';
import '../project/AspNetCore/areas/Products/pages/CategoryPage/CategoryPage.scss';
import CategoryPage from '../project/AspNetCore/areas/Products/pages/CategoryPage/CategoryPage';
import { header, mainNavigation, footer } from './Shared/AspNetCoreProps';

export const CategoryPageStory = () => <CategoryPage header={header} mainNavigation={mainNavigation} footer={footer} />

CategoryPageStory.story = {
  name: 'Category Page',
};

export default {
  title: 'AspNetCore.Category',
  component: CategoryPageStory,
};