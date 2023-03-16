import React from "react";
import { hydrateRoot } from 'react-dom/client';
import ClientAccountManagerPage from "./ClientAccountManagerPage";
import CssSsrRemovalHelper from "../../../../../../../shared/helpers/globals/CssSsrRemovalHelper";

CssSsrRemovalHelper.remove();

hydrateRoot(document.getElementById("root"), <ClientAccountManagerPage {...window.data} />)
