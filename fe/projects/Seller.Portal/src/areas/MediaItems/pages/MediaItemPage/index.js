import React from "react";
import { hydrateRoot } from 'react-dom/client';
import "../../../../../../../shared/polyfills/index";
import CssSsrRemovalHelper from "../../../../../../../shared/helpers/globals/CssSsrRemovalHelper";
import MediaItemPage from "./MediaItemPage";

CssSsrRemovalHelper.remove();

hydrateRoot(document.getElementById("root"), <MediaItemPage {...window.data} />);