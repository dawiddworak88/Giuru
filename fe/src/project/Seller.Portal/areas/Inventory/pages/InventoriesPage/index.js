import React from "react";
import ReactDOM from "react-dom";
import "../../../../../../shared/polyfills/index";
import CssSsrRemovalHelper from "../../../../../../shared/helpers/globals/CssSsrRemovalHelper";
import InventoriesPage from "./InventoriesPage";

CssSsrRemovalHelper.remove();

ReactDOM.hydrate(<InventoriesPage {...window.data} />, document.getElementById("root"));
