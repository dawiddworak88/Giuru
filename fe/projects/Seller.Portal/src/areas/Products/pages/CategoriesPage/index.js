import React from "react";
import { hydrateRoot } from 'react-dom/client';
import CategoriesPage from "./CategoriesPage";
import CssSsrRemovalHelper from "../../../../../../shared/helpers/globals/CssSsrRemovalHelper";

CssSsrRemovalHelper.remove();

hydrateRoot(document.getElementById("root"), <CategoriesPage {...window.data} />)
