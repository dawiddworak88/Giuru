import React from "react";
import ReactDOM from "react-dom";
import AvailableProductsPage from "./AvailableProductsPage";
import CssSsrRemovalHelper from "../../../../../../shared/helpers/globals/CssSsrRemovalHelper";

CssSsrRemovalHelper.remove();

ReactDOM.hydrate(<AvailableProductsPage {...window.data} />, document.getElementById("root"));
