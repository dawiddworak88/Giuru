import React from "react";
import ReactDOM from "react-dom";
import "../../../../../../shared/polyfills/index";
import CssSsrRemovalHelper from "../../../../../../shared/helpers/globals/CssSsrRemovalHelper";
import InventoryPage from "./InventoryPage";

CssSsrRemovalHelper.remove();

ReactDOM.hydrate(<InventoryPage {...window.data} />, document.getElementById("root"));