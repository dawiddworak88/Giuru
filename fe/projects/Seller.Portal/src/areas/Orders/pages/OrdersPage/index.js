import React from "react";
import { hydrateRoot } from 'react-dom/client';
import OrdersPage from "./OrdersPage";
import CssSsrRemovalHelper from "../../../../../../shared/helpers/globals/CssSsrRemovalHelper";

CssSsrRemovalHelper.remove();

hydrateRoot(document.getElementById("root"), <OrdersPage {...window.data} />);
