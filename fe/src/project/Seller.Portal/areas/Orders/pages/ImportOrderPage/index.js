import React from "react";
import ReactDOM from "react-dom";
import ImportOrderPage from "./ImportOrderPage";
import CssSsrRemovalHelper from "../../../../../../shared/helpers/globals/CssSsrRemovalHelper";

CssSsrRemovalHelper.remove();

ReactDOM.hydrate(<ImportOrderPage {...window.data} />, document.getElementById("root"));