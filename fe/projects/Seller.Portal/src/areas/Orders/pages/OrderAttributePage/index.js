import React from "react";
import { hydrateRoot } from 'react-dom/client';
import "../../../../shared/polyfills/index";
import OrderAttributePage from "./OrderAttributePage";
import CssSsrRemovalHelper from "../../../../../../../shared/helpers/globals/CssSsrRemovalHelper";

CssSsrRemovalHelper.remove();

hydrateRoot(document.getElementById("root"), <OrderAttributePage {...window.data} />)