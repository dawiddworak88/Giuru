import React from "react";
import { hydrateRoot } from 'react-dom/client';
import "../../../../shared/polyfills/index";
import CssSsrRemovalHelper from "../../../../../../../shared/helpers/globals/CssSsrRemovalHelper";
import WarehousesPage from "./WarehousesPage";

CssSsrRemovalHelper.remove();

hydrateRoot(document.getElementById("root"), <WarehousesPage {...window.data} />)