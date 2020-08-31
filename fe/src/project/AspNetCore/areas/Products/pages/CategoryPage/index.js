import React from "react";
import ReactDOM from "react-dom";
import CategoryPage from "./CategoryPage";
import CssSsrRemovalHelper from "../../../../../../shared/helpers/globals/CssSsrRemovalHelper";

CssSsrRemovalHelper.Remove();

ReactDOM.hydrate(<CategoryPage {...window.data} />, document.getElementById("root"));