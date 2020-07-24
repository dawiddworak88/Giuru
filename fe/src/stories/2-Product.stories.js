import React from 'react';
import '../project/AspNetCore/areas/Products/pages/ProductPage/ProductPage.scss';
import ProductPage from '../project/AspNetCore/areas/Products/pages/ProductPage/ProductPage';
import { header, mainNavigation, footer } from './Shared/AspNetCoreProps';

var productDetail = {

    title: 'Mounting Dream Tilt TV Wall Mount Bracket for Most 37-70 Inches TVs, TV Mount with VESA up to 600x400mm, Fits 16'
}

export const ProductPageStory = () => <ProductPage header={header} mainNavigation={mainNavigation} productDetail={productDetail} footer={footer} />

ProductPageStory.story = {
  name: 'Product Page',
};

export default {
  title: 'AspNetCore.Product',
  component: ProductPageStory,
};
