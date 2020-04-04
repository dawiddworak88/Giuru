import React from 'react';
import ReactDOM from 'react-dom';
import ClientDetailPage from './ClientDetailPage';

ReactDOM.hydrate(<ClientDetailPage {...window.data} />, document.getElementById('root'));