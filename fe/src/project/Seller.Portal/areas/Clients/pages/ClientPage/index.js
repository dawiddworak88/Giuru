import React from "react";
import ReactDOM from "react-dom";
import ClientPage from "./ClientPage";
import CssSsrRemovalHelper from "../../../../../../shared/helpers/globals/CssSsrRemovalHelper";

CssSsrRemovalHelper.remove();

ReactDOM.hydrate(<ClientPage {...window.data} />, document.getElementById("root"));