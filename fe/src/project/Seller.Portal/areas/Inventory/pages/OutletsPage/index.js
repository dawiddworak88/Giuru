import React from "react";
import ReactDOM from "react-dom";
import "../../../../../../shared/polyfills/index";
import CssSsrRemovalHelper from "../../../../../../shared/helpers/globals/CssSsrRemovalHelper";
import OutletsPage from "./OutletsPage";

CssSsrRemovalHelper.remove();

ReactDOM.hydrate(<OutletsPage {...window.data} />, document.getElementById("root"));