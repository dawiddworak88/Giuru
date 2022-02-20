import React from "react";
import ReactDOM from "react-dom";
import "../../../../../../shared/polyfills/index";
import CssSsrRemovalHelper from "../../../../../../shared/helpers/globals/CssSsrRemovalHelper";
import WarehousesPage from "./WarehousesPage";

CssSsrRemovalHelper.remove();

ReactDOM.hydrate(<WarehousesPage {...window.data} />, document.getElementById("root"));
