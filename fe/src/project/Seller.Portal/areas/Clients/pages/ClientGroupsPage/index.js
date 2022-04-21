import React from "react";
import ReactDOM from "react-dom";
import ClientGroupsPage from "./ClientGroupsPage";
import CssSsrRemovalHelper from "../../../../../../shared/helpers/globals/CssSsrRemovalHelper";

CssSsrRemovalHelper.remove();

ReactDOM.hydrate(<ClientGroupsPage {...window.data} />, document.getElementById("root"));