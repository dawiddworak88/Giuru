import React from "react";
import { hydrateRoot } from 'react-dom/client';
import "../../../../shared/polyfills/index";
import CssSsrRemovalHelper from "../../../../../../../shared/helpers/globals/CssSsrRemovalHelper";
import OutletsPage from "./OutletsPage";

CssSsrRemovalHelper.remove();

hydrateRoot(document.getElementById("root"), <OutletsPage {...window.data} />)