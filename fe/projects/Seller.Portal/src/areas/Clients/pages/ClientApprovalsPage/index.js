import React from "react";
import { hydrateRoot } from "react-dom/client";
import ClientApprovalsPage from "./ClientApprovalsPage";
import CssSsrRemovalHelper from "../../../../../../../shared/helpers/globals/CssSsrRemovalHelper";

CssSsrRemovalHelper.remove();

hydrateRoot(document.getElementById("root"), <ClientApprovalsPage {...window.data} />)