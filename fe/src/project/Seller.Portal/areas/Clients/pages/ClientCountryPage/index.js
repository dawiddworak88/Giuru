import React from "react";
import { hydrateRoot } from 'react-dom/client';
import "../../../../../../shared/polyfills/index";
import ClientCountryPage from "./ClientCountryPage";
import CssSsrRemovalHelper from "../../../../../../shared/helpers/globals/CssSsrRemovalHelper";

CssSsrRemovalHelper.remove();

hydrateRoot(document.getElementById("root"), <ClientCountryPage {...window.data} />)