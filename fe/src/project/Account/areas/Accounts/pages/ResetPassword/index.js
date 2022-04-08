import React from "react";
import ReactDOM from "react-dom";
import ResetPasswordPage from "./ResetPasswordPage";
import CssSsrRemovalHelper from "../../../../../../shared/helpers/globals/CssSsrRemovalHelper";

CssSsrRemovalHelper.remove();

ReactDOM.hydrate(<ResetPasswordPage {...window.data} />, document.getElementById("root"));
