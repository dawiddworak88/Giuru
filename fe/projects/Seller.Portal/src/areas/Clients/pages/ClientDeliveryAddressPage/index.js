import React from "react";
import { hydrateRoot } from 'react-dom/client';
import "../../../../shared/polyfills/index";
import ClientDeliveryAddressPage from "./ClientDeliveryAddressPage";
import CssSsrRemovalHelper from "../../../../../../../shared/helpers/globals/CssSsrRemovalHelper";

CssSsrRemovalHelper.remove();

hydrateRoot(document.getElementById("root"), <ClientDeliveryAddressPage {...window.data} />)