import React from "react";
import { hydrateRoot } from 'react-dom/client';
import NewsItemPage from "./NewsItemPage";
import CssSsrRemovalHelper from "../../../../../../shared/helpers/globals/CssSsrRemovalHelper";

CssSsrRemovalHelper.remove();

hydrateRoot(document.getElementById("root"), <NewsItemPage {...window.data} />)
