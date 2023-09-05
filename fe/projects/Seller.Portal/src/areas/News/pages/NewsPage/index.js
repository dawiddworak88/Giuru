import React from "react";
import { hydrateRoot } from 'react-dom/client';
import NewsPage from "./NewsPage";
import CssSsrRemovalHelper from "../../../../../../../shared/helpers/globals/CssSsrRemovalHelper";

CssSsrRemovalHelper.remove();

hydrateRoot(document.getElementById("root"), <NewsPage {...window.data} />)