import React from "react";
import ReactDOM from "react-dom";
import "../../../../../../shared/polyfills/index";
import SettingsPage from "./SettingsPage";
import CssSsrRemovalHelper from "../../../../../../shared/helpers/globals/CssSsrRemovalHelper";

CssSsrRemovalHelper.remove();

ReactDOM.hydrate(<SettingsPage {...window.data} />, document.getElementById("root"));
