import React from "react";
import { hydrateRoot } from 'react-dom/client';
import ClientTeamMemberPage from "./ClientTeamMemberPage";
import CssSsrRemovalHelper from "../../../../../../../shared/helpers/globals/CssSsrRemovalHelper";

CssSsrRemovalHelper.remove();

hydrateRoot(document.getElementById("root"), <ClientTeamMemberPage {...window.data} />)
