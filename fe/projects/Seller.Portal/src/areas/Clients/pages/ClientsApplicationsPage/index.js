import React from "react";
import { hydrateRoot } from 'react-dom/client';
import "../../../../shared/polyfills/index";
import ClientPage from "./ClientsApplicationsPage";
import CssSsrRemovalHelper from "../../../../../../../shared/helpers/globals/CssSsrRemovalHelper";

CssSsrRemovalHelper.remove();

hydrateRoot(document.getElementById("root"), <ClientPage {...window.data} />)