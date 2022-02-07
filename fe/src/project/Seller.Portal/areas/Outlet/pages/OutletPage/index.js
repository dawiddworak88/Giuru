import React from "react";
import ReactDOM from "react-dom";
import "../../../../../../shared/polyfills/index";
import CssSsrRemovalHelper from "../../../../../../shared/helpers/globals/CssSsrRemovalHelper";
import OutletPage from "./OutletPage";

CssSsrRemovalHelper.remove();

ReactDOM.hydrate(<OutletPage {...window.data} />, document.getElementById("root"));