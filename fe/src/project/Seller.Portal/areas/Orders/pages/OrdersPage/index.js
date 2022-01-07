import React from "react";
import ReactDOM from "react-dom";
import OrdersPage from "./OrdersPage";
import CssSsrRemovalHelper from "../../../../../../shared/helpers/globals/CssSsrRemovalHelper";

CssSsrRemovalHelper.remove();

ReactDOM.hydrate(<OrdersPage {...window.data} />, document.getElementById("root"));
