import React from "react";
import { hydrateRoot } from 'react-dom/client';
import OrderItemPage from "./OrderItemPage";
import CssSsrRemovalHelper from "../../../../../../../shared/helpers/globals/CssSsrRemovalHelper";

CssSsrRemovalHelper.remove();

hydrateRoot(document.getElementById("root"), <OrderItemPage {...window.data} />)
