import React from "react";
import { hydrateRoot } from 'react-dom/client';
import SlugPage from "./SlugPage";
import CssSsrRemovalHelper from "../../../../../../../shared/helpers/globals/CssSsrRemovalHelper";

CssSsrRemovalHelper.remove();

hydrateRoot(document.getElementById("root"), <SlugPage {...window.data} />)