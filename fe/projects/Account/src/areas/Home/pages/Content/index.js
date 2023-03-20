import React from "react";
import { hydrateRoot } from 'react-dom/client';
import ContentPage from "./ContentPage";
import CssSsrRemovalHelper from "../../../../../../../shared/helpers/globals/CssSsrRemovalHelper";

CssSsrRemovalHelper.remove();

hydrateRoot(document.getElementById("root"), <ContentPage {...window.data} />)