import React from "react";
import ReactDOM from "react-dom";
import ProductPage from "./ProductPage";
import CssSsrRemovalHelper from "../../../../../../shared/helpers/globals/CssSsrRemovalHelper";

CssSsrRemovalHelper.remove();

ReactDOM.hydrate(<ProductPage {...window.data} />, document.getElementById("root"));
