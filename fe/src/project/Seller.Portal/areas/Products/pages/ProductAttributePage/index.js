import React from "react";
import ReactDOM from "react-dom";
import ProductAttributePage from "./ProductAttributePage";
import CssSsrRemovalHelper from "../../../../../../shared/helpers/globals/CssSsrRemovalHelper";

CssSsrRemovalHelper.remove();

ReactDOM.hydrate(<ProductAttributePage {...window.data} />, document.getElementById("root"));
