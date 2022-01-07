import React from "react";
import ReactDOM from "react-dom";
import ClientsPage from "./ClientsPage";
import CssSsrRemovalHelper from "../../../../../../shared/helpers/globals/CssSsrRemovalHelper";

CssSsrRemovalHelper.remove();

ReactDOM.hydrate(<ClientsPage {...window.data} />, document.getElementById("root"));
