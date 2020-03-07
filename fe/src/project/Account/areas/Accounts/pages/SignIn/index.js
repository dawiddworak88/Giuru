import React from 'react';
import ReactDOM from 'react-dom';
import SignInPage from './SignInPage';

ReactDOM.hydrate(<SignInPage {...window.data} />, document.getElementById('root'));