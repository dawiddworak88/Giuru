import React from "react";
import { hydrateRoot } from 'react-dom/client';
import ClientGroupsPage from "./ClientGroupsPage";
import CssSsrRemovalHelper from "../../../../../../../shared/helpers/globals/CssSsrRemovalHelper";

CssSsrRemovalHelper.remove();

hydrateRoot(document.getElementById("root"), <ClientGroupsPage {...window.data} />)