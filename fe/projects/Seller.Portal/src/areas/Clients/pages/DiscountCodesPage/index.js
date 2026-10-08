import React from "react";
import { hydrateRoot } from 'react-dom/client';
import DiscountCodesPage from "./DiscountCodesPage";
import CssSsrRemovalHelper from "../../../../../../../shared/helpers/globals/CssSsrRemovalHelper";

CssSsrRemovalHelper.remove();

hydrateRoot(document.getElementById("root"), <DiscountCodesPage {...window.data} />)