import React from "react";
import ReactDOM from "react-dom";
import OrderPage from "./OrderPage";
import CssSsrRemovalHelper from "../../../../../../shared/helpers/globals/CssSsrRemovalHelper";

CssSsrRemovalHelper.remove();

ReactDOM.hydrate(<OrderPage {...window.data} />, document.getElementById("root"));
