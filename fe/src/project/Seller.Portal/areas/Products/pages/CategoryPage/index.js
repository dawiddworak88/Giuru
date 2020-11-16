import React from "react";
import ReactDOM from "react-dom";
import "../../../../../../shared/polyfills/index";
import CategoryPage from "./CategoryPage";
import CssSsrRemovalHelper from "../../../../../../shared/helpers/globals/CssSsrRemovalHelper";

CssSsrRemovalHelper.remove();

ReactDOM.hydrate(<CategoryPage {...window.data} />, document.getElementById("root"));
