import React from "react";
import ReactDOM from "react-dom";
import ProductsPage from "./ProductsPage";
import CssSsrRemovalHelper from "../../../../../../shared/helpers/globals/CssSsrRemovalHelper";

CssSsrRemovalHelper.remove();

ReactDOM.hydrate(<ProductsPage {...window.data} />, document.getElementById("root"));
