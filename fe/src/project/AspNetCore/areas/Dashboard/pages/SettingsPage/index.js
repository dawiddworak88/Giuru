import React from "react";
import { hydrateRoot } from 'react-dom/client';
import SettingsPage from "./SettingsPage";
import CssSsrRemovalHelper from "../../../../../../shared/helpers/globals/CssSsrRemovalHelper";

CssSsrRemovalHelper.remove();

hydrateRoot(document.getElementById("root"), <SettingsPage {...window.data} />)
