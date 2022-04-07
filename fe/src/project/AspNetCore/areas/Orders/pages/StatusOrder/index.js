import React from "react";
import ReactDOM from "react-dom";
import StatusOrderPage from "./StatusOrderPage";
import CssSsrRemovalHelper from "../../../../../../shared/helpers/globals/CssSsrRemovalHelper";

CssSsrRemovalHelper.remove();

ReactDOM.hydrate(<StatusOrderPage {...window.data} />, document.getElementById("root"));
