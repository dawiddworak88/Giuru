import React from "react";
import ReactDOM from "react-dom";
import ClientGroupPage from "./ClientGroupPage";
import CssSsrRemovalHelper from "../../../../../../shared/helpers/globals/CssSsrRemovalHelper";

CssSsrRemovalHelper.remove();

ReactDOM.hydrate(<ClientGroupPage {...window.data} />, document.getElementById("root"));