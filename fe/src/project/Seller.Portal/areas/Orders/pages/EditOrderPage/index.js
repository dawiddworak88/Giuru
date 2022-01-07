import React from "react";
import ReactDOM from "react-dom";
import EditOrderPage from "./EditOrderPage";
import CssSsrRemovalHelper from "../../../../../../shared/helpers/globals/CssSsrRemovalHelper";

CssSsrRemovalHelper.remove();

ReactDOM.hydrate(<EditOrderPage {...window.data} />, document.getElementById("root"));
