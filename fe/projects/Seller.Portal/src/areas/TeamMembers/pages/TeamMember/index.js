import React from "react";
import { hydrateRoot } from 'react-dom/client';
import TeamMember from "./TeamMember";
import CssSsrRemovalHelper from "../../../../../../../shared/helpers/globals/CssSsrRemovalHelper";

CssSsrRemovalHelper.remove();

hydrateRoot(document.getElementById("root"), <TeamMember {...window.data} />)
