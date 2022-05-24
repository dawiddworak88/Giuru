import React from "react";
import { hydrateRoot } from 'react-dom/client';
import StatusOrderPage from "./StatusOrderPage";
import CssSsrRemovalHelper from "../../../../../../shared/helpers/globals/CssSsrRemovalHelper";

CssSsrRemovalHelper.remove();

hydrateRoot(document.getElementById("root"), <StatusOrderPage {...window.data} />)