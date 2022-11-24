import React from "react";
import { hydrateRoot } from 'react-dom/client';
import "../../../../../../shared/polyfills/index";
import ProductCardPage from "./ProductCardPage";
import CssSsrRemovalHelper from "../../../../../../shared/helpers/globals/CssSsrRemovalHelper";

CssSsrRemovalHelper.remove();

hydrateRoot(document.getElementById("root"), <ProductCardPage {...window.data} />)
