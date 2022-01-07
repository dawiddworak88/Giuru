import React from "react";
import ReactDOM from "react-dom";
import "../../../../../../shared/polyfills/index";
import CssSsrRemovalHelper from "../../../../../../shared/helpers/globals/CssSsrRemovalHelper";
import WarehouseAddPage from "./WarehouseAddPage";

CssSsrRemovalHelper.remove();

ReactDOM.hydrate(<WarehouseAddPage {...window.data} />, document.getElementById("root"));