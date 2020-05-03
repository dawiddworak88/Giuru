import React from 'react';
import ReactDOM from 'react-dom';
import '../../../../../../shared/polyfills/PromisePolyfill';
import 'whatwg-fetch';
import ClientDetailPage from './ClientDetailPage';

ReactDOM.hydrate(<ClientDetailPage {...window.data} />, document.getElementById('root'));