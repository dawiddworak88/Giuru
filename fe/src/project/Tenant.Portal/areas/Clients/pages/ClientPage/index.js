import React from 'react';
import ReactDOM from 'react-dom';
import ClientPage from './ClientPage';

ReactDOM.hydrate(<ClientPage {...window.data} />, document.getElementById('root'));