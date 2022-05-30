import React from "react";
import { hydrateRoot } from 'react-dom/client';
import ClientsApplicationPage from "./ClientsApplicationPage";
import CssSsrRemovalHelper from "../../../../../../shared/helpers/globals/CssSsrRemovalHelper";

CssSsrRemovalHelper.remove();

hydrateRoot(document.getElementById("root"), <ClientsApplicationPage {...window.data} />)
