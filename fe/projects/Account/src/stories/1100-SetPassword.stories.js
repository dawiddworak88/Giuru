import React from "react";
import "../areas/Accounts/pages/SetPassword/SetPasswordPage.scss";
import SetPasswordPage from "../areas/Accounts/pages/SetPassword/SetPasswordPage";
import { header, footer } from "./shared/Props";

var setPasswordForm = {
  submitUrl: "#",
  generalErrorMessage: "An error has occurred.",
  setPasswordText: "Set password",
  passwordLabel: "Password",
  passwordRequiredErrorMessage: "Enter password",
  passwordFormatErrorMessage: "Password must be at least 8 characters long, consist of at least one capital letter, one small letter, a digit and a special character, e.g. P@ssw0rd",
  enterPasswordText: "Enter password",
  marketingApprovalHeader: "Marketing form for people cooperating with ELTAP",
  marketingApprovalText: "If you would like to receive information from us about the current offer, current campaigns, introduced functionalities or changes, as well as other activities conducted by ELTAP, please complete the marketing consents below. Consents may be withdrawn at any time, and the withdrawal of consent does not affect actions taken before the date of its withdrawal.",
  emailMarketingApprovalLabel: "I consent to receiving information from ELTAP electronically, to an e-mail address that I secure",
  smsMarketingApprovalLabel: "I consent to telephone contact from ELTAP for marketing purposes"
};

export const SetPasswordPageStory = () => <SetPasswordPage header={header} setPasswordForm={setPasswordForm} footer={footer} />;

SetPasswordPageStory.story = {
  name: "SetPassword Page",
};

const SetPasswordStories = {
  title: "Pages",
  component: SetPasswordPageStory,
};

export default SetPasswordStories;