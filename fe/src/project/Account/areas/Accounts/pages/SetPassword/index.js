import React from "react";
import ReactDOM from "react-dom";
import SetPasswordPage from "./SetPasswordPage";
import CssSsrRemovalHelper from "../../../../../../shared/helpers/globals/CssSsrRemovalHelper";

CssSsrRemovalHelper.remove();

ReactDOM.hydrate(<SetPasswordPage {...window.data} />, document.getElementById("root"));
