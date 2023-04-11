import React from "react";
import { hydrateRoot } from 'react-dom/client';
import ClientRolePage from "./ClientRolePage";
import CssSsrRemovalHelper from "../../../../../../../shared/helpers/globals/CssSsrRemovalHelper";

CssSsrRemovalHelper.remove();

hydrateRoot(document.getElementById("root"), <ClientRolePage {...window.data} />)
