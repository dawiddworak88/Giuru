import React from "react";
import ReactDOM from "react-dom";
import "../../../../../../shared/polyfills/index";
import CssSsrRemovalHelper from "../../../../../../shared/helpers/globals/CssSsrRemovalHelper";
import InventoryAddPage from "./InventoryAddPage";

CssSsrRemovalHelper.remove();

ReactDOM.hydrate(<InventoryAddPage {...window.data} />, document.getElementById("root"));