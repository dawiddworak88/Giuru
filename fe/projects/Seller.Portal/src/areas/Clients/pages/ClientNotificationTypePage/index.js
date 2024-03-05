import React from "react";
import { hydrateRoot } from "react-dom/client";
import ClientNotificationTypePage from "./ClientNotificationTypePage";
import CssSsrRemovalHelper from "../../../../../../../shared/helpers/globals/CssSsrRemovalHelper";

CssSsrRemovalHelper.remove();

hydrateRoot(document.getElementById("root"), <ClientNotificationTypePage {...window.data} />)