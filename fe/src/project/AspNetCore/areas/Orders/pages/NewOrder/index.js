import React from "react";
import { hydrateRoot } from 'react-dom/client';
import NewOrderPage from "./NewOrderPage";
import CssSsrRemovalHelper from "../../../../../../shared/helpers/globals/CssSsrRemovalHelper";

CssSsrRemovalHelper.remove();

hydrateRoot(document.getElementById("root"), <NewOrderPage {...window.data} />)