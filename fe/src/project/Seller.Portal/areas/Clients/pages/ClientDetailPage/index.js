import React from 'react';
import ReactDOM from 'react-dom';
import '../../../../../../shared/polyfills/index';
import ClientDetailPage from './ClientDetailPage';
import CssSsrRemovalHelper from '../../../../../../shared/helpers/globals/CssSsrRemovalHelper';

CssSsrRemovalHelper.Remove();

ReactDOM.hydrate(<ClientDetailPage {...window.data} />, document.getElementById('root'));