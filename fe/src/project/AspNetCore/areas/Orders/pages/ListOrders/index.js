import React from "react";
import { hydrateRoot } from 'react-dom/client';
import ListOrdersPage from "./ListOrdersPage";
import CssSsrRemovalHelper from "../../../../../../shared/helpers/globals/CssSsrRemovalHelper";

CssSsrRemovalHelper.remove();

hydrateRoot(document.getElementById("root"), <ListOrdersPage {...window.data} />)