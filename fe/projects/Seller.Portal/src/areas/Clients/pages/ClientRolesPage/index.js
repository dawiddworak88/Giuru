import React from "react";
import { hydrateRoot } from 'react-dom/client';
import ClientRolesPage from "./ClientRolesPage";
import CssSsrRemovalHelper from "../../../../../../../shared/helpers/globals/CssSsrRemovalHelper";

CssSsrRemovalHelper.remove();

hydrateRoot(document.getElementById("root"), <ClientRolesPage {...window.data} />)
