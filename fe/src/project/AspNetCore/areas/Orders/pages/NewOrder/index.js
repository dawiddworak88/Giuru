import React from "react";
import ReactDOM from "react-dom";
import NewOrderPage from "./NewOrderPage";
import CssSsrRemovalHelper from "../../../../../../shared/helpers/globals/CssSsrRemovalHelper";

CssSsrRemovalHelper.remove();

ReactDOM.hydrate(<NewOrderPage {...window.data} />, document.getElementById("root"));