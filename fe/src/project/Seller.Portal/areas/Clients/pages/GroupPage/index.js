import React from "react";
import ReactDOM from "react-dom";
import GroupPage from "./GroupPage";
import CssSsrRemovalHelper from "../../../../../../shared/helpers/globals/CssSsrRemovalHelper";

CssSsrRemovalHelper.remove();

ReactDOM.hydrate(<GroupPage {...window.data} />, document.getElementById("root"));