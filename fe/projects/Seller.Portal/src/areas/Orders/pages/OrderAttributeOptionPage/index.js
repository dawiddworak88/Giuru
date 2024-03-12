import React from "react";
import { hydrateRoot } from 'react-dom/client';
import "../../../../shared/polyfills/index";
import OrderAttributeOptionPage from "./OrderAttributeOptionPage";
import CssSsrRemovalHelper from "../../../../../../../shared/helpers/globals/CssSsrRemovalHelper";

CssSsrRemovalHelper.remove();

hydrateRoot(document.getElementById("root"), <OrderAttributeOptionPage {...window.data} />)