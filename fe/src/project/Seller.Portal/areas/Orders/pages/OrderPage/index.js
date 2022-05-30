import React from "react";
import { hydrateRoot } from 'react-dom/client';
import OrderPage from "./OrderPage";
import CssSsrRemovalHelper from "../../../../../../shared/helpers/globals/CssSsrRemovalHelper";

CssSsrRemovalHelper.remove();

hydrateRoot(document.getElementById("root"), <OrderPage {...window.data} />)
