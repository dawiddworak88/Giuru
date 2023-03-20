import React from "react";
import { hydrateRoot } from 'react-dom/client';
import ResetPasswordPage from "./ResetPasswordPage";
import CssSsrRemovalHelper from "../../../../../../../shared/helpers/globals/CssSsrRemovalHelper";

CssSsrRemovalHelper.remove();

hydrateRoot(document.getElementById("root"), <ResetPasswordPage {...window.data} />)