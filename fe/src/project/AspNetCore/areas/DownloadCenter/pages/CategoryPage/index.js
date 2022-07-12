import React from "react";
import { hydrateRoot } from 'react-dom/client';
import CategoryPage from "./CategoryPage";
import CssSsrRemovalHelper from "../../../../../../shared/helpers/globals/CssSsrRemovalHelper";

CssSsrRemovalHelper.remove();

hydrateRoot(document.getElementById("root"), <CategoryPage {...window.data} />)