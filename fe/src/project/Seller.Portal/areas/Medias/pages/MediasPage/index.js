import React from "react";
import ReactDOM from "react-dom";
import "../../../../../../shared/polyfills/index";
import CssSsrRemovalHelper from "../../../../../../shared/helpers/globals/CssSsrRemovalHelper";
import MediasPage from "./MediasPage";

CssSsrRemovalHelper.remove();

ReactDOM.hydrate(<MediasPage {...window.data} />, document.getElementById("root"));