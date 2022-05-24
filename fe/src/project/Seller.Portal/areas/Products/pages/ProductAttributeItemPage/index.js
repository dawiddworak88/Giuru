import React from "react";
import { hydrateRoot } from 'react-dom/client';
import ProductAttributeItemPage from "./ProductAttributeItemPage";
import CssSsrRemovalHelper from "../../../../../../shared/helpers/globals/CssSsrRemovalHelper";

CssSsrRemovalHelper.remove();

hydrateRoot(document.getElementById("root"), <ProductAttributeItemPage {...window.data} />)
