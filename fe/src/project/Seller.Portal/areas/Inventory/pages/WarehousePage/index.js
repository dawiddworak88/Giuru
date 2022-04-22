import React from "react";
import ReactDOM from "react-dom";
import "../../../../../../shared/polyfills/index";
import CssSsrRemovalHelper from "../../../../../../shared/helpers/globals/CssSsrRemovalHelper";
import WarehousePage from "./WarehousePage";

CssSsrRemovalHelper.remove();

ReactDOM.hydrate(<WarehousePage {...window.data} />, document.getElementById("root"));