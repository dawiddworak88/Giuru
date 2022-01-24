import React from "react";
import ReactDOM from "react-dom";
import "../../../../../../shared/polyfills/index";
import CssSsrRemovalHelper from "../../../../../../shared/helpers/globals/CssSsrRemovalHelper";
import UploadMediaPage from "./UploadMediaPage";

CssSsrRemovalHelper.remove();

ReactDOM.hydrate(<UploadMediaPage {...window.data} />, document.getElementById("root"));