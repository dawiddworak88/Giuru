import React from "react";
import { hydrateRoot } from 'react-dom/client';
import "../../../../../../shared/polyfills/index";
import CssSsrRemovalHelper from "../../../../../../shared/helpers/globals/CssSsrRemovalHelper";
import InventoriesPage from "./InventoriesPage";

CssSsrRemovalHelper.remove();

hydrateRoot(document.getElementById("root"), <InventoriesPage {...window.data} />)