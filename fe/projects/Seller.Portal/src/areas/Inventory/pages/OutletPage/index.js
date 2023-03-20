import React from "react";
import { hydrateRoot } from 'react-dom/client';
import "../../../../shared/polyfills/index";
import CssSsrRemovalHelper from "../../../../../../../shared/helpers/globals/CssSsrRemovalHelper";
import OutletPage from "./OutletPage";

CssSsrRemovalHelper.remove();

hydrateRoot(document.getElementById("root"), <OutletPage {...window.data} />)