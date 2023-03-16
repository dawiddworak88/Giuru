import React from "react";
import { hydrateRoot } from 'react-dom/client';
import SearchProductsPage from "./SearchProductsPage";
import CssSsrRemovalHelper from "../../../../../../shared/helpers/globals/CssSsrRemovalHelper";

CssSsrRemovalHelper.remove();

hydrateRoot(document.getElementById("root"), <SearchProductsPage {...window.data} />)
