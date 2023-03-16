import React from "react";
import { hydrateRoot } from 'react-dom/client';
import ClientGroupPage from "./ClientGroupPage";
import CssSsrRemovalHelper from "../../../../../../shared/helpers/globals/CssSsrRemovalHelper";

CssSsrRemovalHelper.remove();

hydrateRoot(document.getElementById("root"), <ClientGroupPage {...window.data} />)
