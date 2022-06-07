import React from "react";
import { hydrateRoot } from 'react-dom/client';
import ClientManagerPage from "./ClientManagerPage";
import CssSsrRemovalHelper from "../../../../../../shared/helpers/globals/CssSsrRemovalHelper";

CssSsrRemovalHelper.remove();

hydrateRoot(document.getElementById("root"), <ClientManagerPage {...window.data} />)
