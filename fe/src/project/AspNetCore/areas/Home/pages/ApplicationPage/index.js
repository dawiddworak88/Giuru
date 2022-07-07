import React from "react";
import { hydrateRoot } from 'react-dom/client';
import ApplicationPage from "./ApplicationPage";
import CssSsrRemovalHelper from "../../../../../../shared/helpers/globals/CssSsrRemovalHelper";

CssSsrRemovalHelper.remove();

hydrateRoot(document.getElementById("root"), <ApplicationPage {...window.data} />)