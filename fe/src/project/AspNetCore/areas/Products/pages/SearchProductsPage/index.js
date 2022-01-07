import React from "react";
import ReactDOM from "react-dom";
import SearchProductsPage from "./SearchProductsPage";
import CssSsrRemovalHelper from "../../../../../../shared/helpers/globals/CssSsrRemovalHelper";

CssSsrRemovalHelper.remove();

ReactDOM.hydrate(<SearchProductsPage {...window.data} />, document.getElementById("root"));
