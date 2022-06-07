import React from "react";
import { hydrateRoot } from 'react-dom/client';
import ClientManagersPage from "./ClientManagersPage";
import CssSsrRemovalHelper from "../../../../../../shared/helpers/globals/CssSsrRemovalHelper";

CssSsrRemovalHelper.remove();

hydrateRoot(document.getElementById("root"), <ClientManagersPage {...window.data} />)