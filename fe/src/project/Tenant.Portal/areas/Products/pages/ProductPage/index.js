import React from 'react';
import ReactDOM from 'react-dom';
import ProductPage from './ProductPage';

ReactDOM.hydrate(<ProductPage {...window.data} />, document.getElementById('root'));