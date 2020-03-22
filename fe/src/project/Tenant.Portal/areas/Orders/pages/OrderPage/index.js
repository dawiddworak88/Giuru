import React from 'react';
import ReactDOM from 'react-dom';
import OrderPage from './OrderPage';

ReactDOM.hydrate(<OrderPage {...window.data} />, document.getElementById('root'));