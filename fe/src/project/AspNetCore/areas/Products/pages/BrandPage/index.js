import React from "react";
import ReactDOM from "react-dom";
import BrandPage from "./BrandPage";
import CssSsrRemovalHelper from "../../../../../../shared/helpers/globals/CssSsrRemovalHelper";

CssSsrRemovalHelper.remove();

ReactDOM.hydrate(<BrandPage {...window.data} />, document.getElementById("root"));