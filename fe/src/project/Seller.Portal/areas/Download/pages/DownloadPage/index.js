import React from "react";
import { hydrateRoot } from 'react-dom/client';
import DownloadPage from "./DownloadPage";
import CssSsrRemovalHelper from "../../../../../../shared/helpers/globals/CssSsrRemovalHelper";

CssSsrRemovalHelper.remove();

hydrateRoot(document.getElementById("root"), <DownloadPage {...window.data} />)