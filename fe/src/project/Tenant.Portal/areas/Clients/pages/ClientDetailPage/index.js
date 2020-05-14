import React from 'react';
import ReactDOM from 'react-dom';
import '../../../../../../shared/polyfills/index';
import ClientDetailPage from './ClientDetailPage';

ReactDOM.hydrate(<ClientDetailPage {...window.data} />, document.getElementById('root'));