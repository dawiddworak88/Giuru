import React from 'react';
import '../project/Seller.Portal/areas/Products/pages/ProductPage/ProductPage.scss';
import ProductPage from '../project/Seller.Portal/areas/Products/pages/ProductPage/ProductPage';
import { header, menuTiles, footer } from './Shared/Props';

var props = {
  title: 'Products',
  newText: 'New product',
  newUrl: '#',
  searchLabel: 'Search',
  editUrl: '#',
  deleteUrl: '#',
  catalog: {
    noLabel: "No",
    yesLabel: "Yes",
    deleteConfirmationLabel: "Delete confirmation",
    areYouSureLabel: 'Are you sure you want to delete this item',
    generalErrorMessage: 'An Error Occurred',
    searchLabel: 'Search',
    editLabel: 'Edit',
    deleteLabel: 'Delete',
    displayedRowsLabel: 'of',
    rowsPerPageLabel: 'Rows per Page',
    backIconButtonText: 'Previous',
    nextIconButtonText: 'Next',
    editUrl: '#',
    deleteUrl: '#',
    noResultsLabel: 'There are no results',
    skuLabel: 'SKU',
    nameLabel: 'Name',
    lastModifiedDateLabel: 'Last modified date',
    createdDateLabel: 'Created date',
    pagedProducts: {
      data: [
       {
         id: '1',
         sku: 'Berg01',
         name: 'Bergamo',
         lastModifiedDate: new Date(),
         createdDate: new Date(),
       } 
      ],
      total: 1
    }
  }
};

export const ProductPageStory = () => <ProductPage header={header} menuTiles={menuTiles} {...props} footer={footer} />

ProductPageStory.story = {
  name: 'Products Page',
};

export default {
  title: 'SellerPortal.Products',
  component: ProductPageStory,
};