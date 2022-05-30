import React from "react";
import { hydrateRoot } from 'react-dom/client';
import "../../../../../../shared/polyfills/index";
import CssSsrRemovalHelper from "../../../../../../shared/helpers/globals/CssSsrRemovalHelper";
import MediasPage from "./MediasPage";

CssSsrRemovalHelper.remove();

hydrateRoot(document.getElementById("root"), <MediasPage {...window.data} />);