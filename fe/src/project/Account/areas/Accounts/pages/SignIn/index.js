import React from 'react';
import ReactDOM from 'react-dom';
import SignIn from './SignIn';

ReactDOM.hydrate(<SignIn {...window.data} />, document.getElementById('root'));