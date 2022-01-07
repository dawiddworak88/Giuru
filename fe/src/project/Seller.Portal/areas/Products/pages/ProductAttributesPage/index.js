import React from "react";
import ReactDOM from "react-dom";
import ProductAttributesPage from "./ProductAttributesPage";
import CssSsrRemovalHelper from "../../../../../../shared/helpers/globals/CssSsrRemovalHelper";

CssSsrRemovalHelper.remove();

ReactDOM.hydrate(<ProductAttributesPage {...window.data} />, document.getElementById("root"));
