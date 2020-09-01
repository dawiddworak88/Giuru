import React from "react";
import ReactDOM from "react-dom";
import "../../../../../../shared/polyfills/index";
import ProductDetailPage from "./ProductDetailPage";
import CssSsrRemovalHelper from "../../../../../../shared/helpers/globals/CssSsrRemovalHelper";

CssSsrRemovalHelper.remove();

ReactDOM.hydrate(<ProductDetailPage {...window.data} />, document.getElementById("root"));