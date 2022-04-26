import React from "react";
import ReactDOM from "react-dom";
import OutletPage from "./OutletPage";
import CssSsrRemovalHelper from "../../../../../../shared/helpers/globals/CssSsrRemovalHelper";

CssSsrRemovalHelper.remove();

ReactDOM.hydrate(<OutletPage {...window.data} />, document.getElementById("root"));
