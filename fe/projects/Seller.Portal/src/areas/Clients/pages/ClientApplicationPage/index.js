import React from "react";
import { hydrateRoot } from 'react-dom/client';
import "../../../../../../shared/polyfills/index";
import ClientApplicationPage from "./ClientApplicationPage";
import CssSsrRemovalHelper from "../../../../../../shared/helpers/globals/CssSsrRemovalHelper";

CssSsrRemovalHelper.remove();

hydrateRoot(document.getElementById("root"), <ClientApplicationPage {...window.data} />)