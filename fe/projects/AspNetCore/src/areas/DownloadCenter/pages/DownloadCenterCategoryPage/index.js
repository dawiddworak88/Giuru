import React from "react";
import { hydrateRoot } from 'react-dom/client';
import DownloadCenterCategoryPage from "./DownloadCenterCategoryPage";
import CssSsrRemovalHelper from "../../../../../../../shared/helpers/globals/CssSsrRemovalHelper";

CssSsrRemovalHelper.remove();

hydrateRoot(document.getElementById("root"), <DownloadCenterCategoryPage {...window.data} />)