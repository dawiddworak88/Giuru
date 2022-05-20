import React from "react";
import ReactDOM from "react-dom";
import RegisterPage from "./RegisterPage";
import CssSsrRemovalHelper from "../../../../../../shared/helpers/globals/CssSsrRemovalHelper";

CssSsrRemovalHelper.remove();

ReactDOM.hydrate(<RegisterPage {...window.data} />, document.getElementById("root"));