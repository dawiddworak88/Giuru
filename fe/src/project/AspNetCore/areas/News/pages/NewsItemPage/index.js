import React from "react";
import ReactDOM from "react-dom";
import NewsItemPage from "./NewsItemPage";
import CssSsrRemovalHelper from "../../../../../../shared/helpers/globals/CssSsrRemovalHelper";

CssSsrRemovalHelper.remove();

ReactDOM.hydrate(<NewsItemPage {...window.data} />, document.getElementById("root"));
