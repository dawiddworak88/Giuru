import React from "react";
import { hydrateRoot } from 'react-dom/client';
import DownloadCenterItemPage from "./DownloadCenterItemPage";
import CssSsrRemovalHelper from "../../../../../../../shared/helpers/globals/CssSsrRemovalHelper";

CssSsrRemovalHelper.remove();

hydrateRoot(document.getElementById("root"), <DownloadCenterItemPage {...window.data} />)