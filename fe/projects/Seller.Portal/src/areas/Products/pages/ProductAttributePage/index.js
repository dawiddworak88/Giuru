import React from "react";
import { hydrateRoot } from 'react-dom/client';
import ProductAttributePage from "./ProductAttributePage";
import CssSsrRemovalHelper from "../../../../../../../shared/helpers/globals/CssSsrRemovalHelper";

CssSsrRemovalHelper.remove();

hydrateRoot(document.getElementById("root"), <ProductAttributePage {...window.data} />)
