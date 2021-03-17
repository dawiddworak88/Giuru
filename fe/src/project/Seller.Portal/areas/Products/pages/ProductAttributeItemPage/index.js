import React from "react";
import ReactDOM from "react-dom";
import ProductAttributeItemPage from "./ProductAttributeItemPage";
import CssSsrRemovalHelper from "../../../../../../shared/helpers/globals/CssSsrRemovalHelper";

CssSsrRemovalHelper.remove();

ReactDOM.hydrate(<ProductAttributeItemPage {...window.data} />, document.getElementById("root"));
