import React from "react";
import { hydrateRoot } from 'react-dom/client';
import DashboardPage from "./DashboardPage";
import CssSsrRemovalHelper from "../../../../../../../shared/helpers/globals/CssSsrRemovalHelper";

CssSsrRemovalHelper.remove();

hydrateRoot(document.getElementById("root"), <DashboardPage {...window.data} />)