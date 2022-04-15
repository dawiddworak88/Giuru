import React from "react";
import ReactDOM from "react-dom";
import GroupsPage from "./GroupsPage";
import CssSsrRemovalHelper from "../../../../../../shared/helpers/globals/CssSsrRemovalHelper";

CssSsrRemovalHelper.remove();

ReactDOM.hydrate(<GroupsPage {...window.data} />, document.getElementById("root"));