import React from "react";
import { hydrateRoot } from 'react-dom/client';
import OrderAttributesPage from "./OrderAttributesPage";
import CssSsrRemovalHelper from "../../../../../../../shared/helpers/globals/CssSsrRemovalHelper";

CssSsrRemovalHelper.remove();

hydrateRoot(document.getElementById("root"), <OrderAttributesPage {...window.data} />);
