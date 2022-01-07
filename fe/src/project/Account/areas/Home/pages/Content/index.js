import React from "react";
import ReactDOM from "react-dom";
import ContentPage from "./ContentPage";
import CssSsrRemovalHelper from "../../../../../../shared/helpers/globals/CssSsrRemovalHelper";

CssSsrRemovalHelper.remove();

ReactDOM.hydrate(<ContentPage {...window.data} />, document.getElementById("root"));
