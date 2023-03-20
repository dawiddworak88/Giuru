import React from "react";
import { hydrateRoot } from 'react-dom/client';
import AvailableProductsPage from "./AvailableProductsPage";
import CssSsrRemovalHelper from "../../../../../../../shared/helpers/globals/CssSsrRemovalHelper";

CssSsrRemovalHelper.remove();

hydrateRoot(document.getElementById("root"), <AvailableProductsPage {...window.data} />)