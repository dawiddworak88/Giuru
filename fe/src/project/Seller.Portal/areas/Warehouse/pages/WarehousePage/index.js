import React from "react";
import ReactDOM from "react-dom";
import "../../../../../../shared/polyfills/index";
import CssSsrRemovalHelper from "../../../../../../shared/helpers/globals/CssSsrRemovalHelper";
import Warehouse from "./WarehousePage";

CssSsrRemovalHelper.remove();

ReactDOM.hydrate(<Warehouse {...window.data} />, document.getElementById("root"));