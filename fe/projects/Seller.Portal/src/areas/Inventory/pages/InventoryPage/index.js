import React from "react";
import { hydrateRoot } from 'react-dom/client';
import "../../../../../../shared/polyfills/index";
import CssSsrRemovalHelper from "../../../../../../shared/helpers/globals/CssSsrRemovalHelper";
import InventoryPage from "./InventoryPage";

CssSsrRemovalHelper.remove();

hydrateRoot(document.getElementById("root"), <InventoryPage {...window.data} />)