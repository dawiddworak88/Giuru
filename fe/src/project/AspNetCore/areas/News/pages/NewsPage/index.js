import React from "react";
import ReactDOM from "react-dom";
import NewsPage from "./NewsPage";
import CssSsrRemovalHelper from "../../../../../../shared/helpers/globals/CssSsrRemovalHelper";

CssSsrRemovalHelper.remove();

ReactDOM.hydrate(<NewsPage {...window.data} />, document.getElementById("root"));
