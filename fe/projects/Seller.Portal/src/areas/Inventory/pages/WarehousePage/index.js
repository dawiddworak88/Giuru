import React from "react";
import { hydrateRoot } from 'react-dom/client';
import "../../../../../../../shared/polyfills/index";
import CssSsrRemovalHelper from "../../../../../../../shared/helpers/globals/CssSsrRemovalHelper";
import WarehousePage from "./WarehousePage";

CssSsrRemovalHelper.remove();

hydrateRoot(document.getElementById("root"), <WarehousePage {...window.data} />)