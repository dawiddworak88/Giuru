import React from 'react';
import ReactDOM from 'react-dom';
import '../../../../../../shared/polyfills/index';
import OrderDetailPage from './OrderDetailPage';
import CssSsrRemovalHelper from '../../../../../../shared/helpers/globals/CssSsrRemovalHelper';

CssSsrRemovalHelper.Remove();

ReactDOM.hydrate(<OrderDetailPage {...window.data} />, document.getElementById('root'));