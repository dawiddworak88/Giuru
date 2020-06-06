import React from 'react';
import ReactDOM from 'react-dom';
import HomePage from './HomePage';
import CssSsrRemovalHelper from '../../../../../../shared/helpers/globals/CssSsrRemovalHelper';

CssSsrRemovalHelper.Remove();

ReactDOM.hydrate(<HomePage {...window.data} />, document.getElementById('root'));