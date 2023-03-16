import React from "react";
import { hydrateRoot } from 'react-dom/client';
import DownloadCenterCategoriesPage from "./DownloadCenterCategoriesPage";
import CssSsrRemovalHelper from "../../../../../../../shared/helpers/globals/CssSsrRemovalHelper";

CssSsrRemovalHelper.remove();

hydrateRoot(document.getElementById("root"), <DownloadCenterCategoriesPage {...window.data} />)