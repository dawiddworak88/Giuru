import React from 'react';
import ReactDOM from 'react-dom';
import SignInPage from './SignInPage';
import CssSsrRemovalHelper from '../../../../../../shared/helpers/globals/CssSsrRemovalHelper';

CssSsrRemovalHelper.Remove();

ReactDOM.hydrate(<SignInPage {...window.data} />, document.getElementById('root'));