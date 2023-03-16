import React from "react";
import { hydrateRoot } from 'react-dom/client';
import HomePage from "./HomePage";
import CssSsrRemovalHelper from "../../../../../../shared/helpers/globals/CssSsrRemovalHelper";

CssSsrRemovalHelper.remove();

hydrateRoot(document.getElementById("root"), <HomePage {...window.data} />)
