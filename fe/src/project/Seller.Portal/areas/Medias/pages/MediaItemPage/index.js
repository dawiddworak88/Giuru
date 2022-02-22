import React from "react";
import ReactDOM from "react-dom";
import "../../../../../../shared/polyfills/index";
import CssSsrRemovalHelper from "../../../../../../shared/helpers/globals/CssSsrRemovalHelper";
import MediaItemPage from "./MediaItemPage";

CssSsrRemovalHelper.remove();

ReactDOM.hydrate(<MediaItemPage {...window.data} />, document.getElementById("root"));