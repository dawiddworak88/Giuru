import React from "react";
import { hydrateRoot } from 'react-dom/client';
import "../../../../shared/polyfills/index";
import CssSsrRemovalHelper from "../../../../../../../shared/helpers/globals/CssSsrRemovalHelper";
import MediaItemsPage from "./MediaItemsPage";

CssSsrRemovalHelper.remove();

hydrateRoot(document.getElementById("root"), <MediaItemsPage {...window.data} />);