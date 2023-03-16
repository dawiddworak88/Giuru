import React from "react";
import { hydrateRoot } from 'react-dom/client';
import EditOrderPage from "./EditOrderPage";
import CssSsrRemovalHelper from "../../../../../../shared/helpers/globals/CssSsrRemovalHelper";

CssSsrRemovalHelper.remove();

hydrateRoot(document.getElementById("root"), <EditOrderPage {...window.data} />)
