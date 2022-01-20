import React from "react";
import ReactDOM from "react-dom";
import "../../../../../../shared/polyfills/index";
import CssSsrRemovalHelper from "../../../../../../shared/helpers/globals/CssSsrRemovalHelper";
import MediaPage from "./MediaPage";

CssSsrRemovalHelper.remove();

ReactDOM.hydrate(<MediaPage {...window.data} />, document.getElementById("root"));