import React from "react";
import ReactDOM from "react-dom";
import CategoriesPage from "./CategoriesPage";
import CssSsrRemovalHelper from "../../../../../../shared/helpers/globals/CssSsrRemovalHelper";

CssSsrRemovalHelper.remove();

ReactDOM.hydrate(<CategoriesPage {...window.data} />, document.getElementById("root"));
