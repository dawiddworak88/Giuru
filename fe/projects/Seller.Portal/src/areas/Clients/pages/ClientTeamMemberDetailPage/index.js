import React from "react";
import { hydrateRoot } from 'react-dom/client';
import ClientTeamMemberDetailPage from "./ClientTeamMemberDetailPage";
import CssSsrRemovalHelper from "../../../../../../../shared/helpers/globals/CssSsrRemovalHelper";

CssSsrRemovalHelper.remove();

hydrateRoot(document.getElementById("root"), <ClientTeamMemberDetailPage {...window.data} />)
