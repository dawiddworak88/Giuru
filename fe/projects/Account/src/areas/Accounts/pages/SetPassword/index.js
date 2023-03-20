import React from "react";
import { hydrateRoot } from 'react-dom/client';
import SetPasswordPage from "./SetPasswordPage";
import CssSsrRemovalHelper from "../../../../../../../shared/helpers/globals/CssSsrRemovalHelper";

CssSsrRemovalHelper.remove();

hydrateRoot(document.getElementById("root"), <SetPasswordPage {...window.data} />)