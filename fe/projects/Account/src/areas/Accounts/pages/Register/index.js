import React from "react";
import { hydrateRoot } from 'react-dom/client';
import RegisterPage from "./RegisterPage";
import CssSsrRemovalHelper from "../../../../../../../shared/helpers/globals/CssSsrRemovalHelper";

CssSsrRemovalHelper.remove();

hydrateRoot(document.getElementById("root"), <RegisterPage {...window.data} />)