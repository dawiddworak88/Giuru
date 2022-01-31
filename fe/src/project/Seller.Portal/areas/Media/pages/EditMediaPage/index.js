import React from "react";
import ReactDOM from "react-dom";
import "../../../../../../shared/polyfills/index";
import CssSsrRemovalHelper from "../../../../../../shared/helpers/globals/CssSsrRemovalHelper";
import EditMediaPage from "./EditMediaPage";

CssSsrRemovalHelper.remove();

ReactDOM.hydrate(<EditMediaPage {...window.data} />, document.getElementById("root"));