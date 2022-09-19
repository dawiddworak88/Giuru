import React from "react";
import { hydrateRoot } from 'react-dom/client';
import ClientAccountManagersPage from "./ClientAccountManagersPage";
import CssSsrRemovalHelper from "../../../../../../shared/helpers/globals/CssSsrRemovalHelper";

CssSsrRemovalHelper.remove();

hydrateRoot(document.getElementById("root"), <ClientAccountManagersPage {...window.data} />)