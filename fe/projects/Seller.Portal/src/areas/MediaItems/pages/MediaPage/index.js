import React from "react";
import { hydrateRoot } from 'react-dom/client';
import "../../../../shared/polyfills/index";
import CssSsrRemovalHelper from "../../../../../../../shared/helpers/globals/CssSsrRemovalHelper";
import MediaPage from "./MediaPage";

CssSsrRemovalHelper.remove();

hydrateRoot(document.getElementById("root"), <MediaPage {...window.data} />);