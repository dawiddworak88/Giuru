import React from "react";
import { hydrateRoot } from 'react-dom/client';
import TeamMembers from "./TeamMembers";
import CssSsrRemovalHelper from "../../../../../../../shared/helpers/globals/CssSsrRemovalHelper";

CssSsrRemovalHelper.remove();

hydrateRoot(document.getElementById("root"), <TeamMembers {...window.data} />)
