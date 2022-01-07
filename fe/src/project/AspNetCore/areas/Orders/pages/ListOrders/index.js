import React from "react";
import ReactDOM from "react-dom";
import ListOrdersPage from "./ListOrdersPage";
import CssSsrRemovalHelper from "../../../../../../shared/helpers/globals/CssSsrRemovalHelper";

CssSsrRemovalHelper.remove();

ReactDOM.hydrate(<ListOrdersPage {...window.data} />, document.getElementById("root"));