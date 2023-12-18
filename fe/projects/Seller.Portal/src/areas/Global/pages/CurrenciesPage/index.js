import React from "react";
import { hydrateRoot } from "react-dom/client";
import CurrenciesPage from "./CurrenciesPage";
import CssSsrRemovalHelper from "../../../../../../../shared/helpers/globals/CssSsrRemovalHelper";

CssSsrRemovalHelper.remove();

hydrateRoot(document.getElementById("root"), <CurrenciesPage {...window.data} />)