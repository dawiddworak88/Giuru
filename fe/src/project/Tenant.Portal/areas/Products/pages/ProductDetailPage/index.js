import React from 'react';
import ReactDOM from 'react-dom';
import ProductDetailPage from './ProductDetailPage';

ReactDOM.hydrate(<ProductDetailPage {...window.data} />, document.getElementById('root'));