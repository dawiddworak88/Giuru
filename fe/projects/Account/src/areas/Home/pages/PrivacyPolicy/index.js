import { hydrateRoot } from "react-dom/client";
import CssSsrRemovalHelper from "../../../../../../../shared/helpers/globals/CssSsrRemovalHelper";
import PrivacyPolicyPage from "./PrivacyPolicyPage";
import React from "react";

CssSsrRemovalHelper.remove();

hydrateRoot(document.getElementById("root"), <PrivacyPolicyPage {...window.data} />);