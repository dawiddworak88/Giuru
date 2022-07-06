import React from "react";
import { hydrateRoot } from 'react-dom/client';
import DownloadsPage from "./DownloadsPage";
import CssSsrRemovalHelper from "../../../../../../shared/helpers/globals/CssSsrRemovalHelper";

CssSsrRemovalHelper.remove();

hydrateRoot(document.getElementById("root"), <DownloadsPage {...window.data} />)