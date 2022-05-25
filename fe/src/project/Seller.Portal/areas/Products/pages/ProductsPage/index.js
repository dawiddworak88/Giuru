import React from "react";
import { hydrateRoot } from 'react-dom/client';
import ProductsPage from "./ProductsPage";
import CssSsrRemovalHelper from "../../../../../../shared/helpers/globals/CssSsrRemovalHelper";

CssSsrRemovalHelper.remove();

hydrateRoot(document.getElementById("root"), <ProductsPage {...window.data} />)
