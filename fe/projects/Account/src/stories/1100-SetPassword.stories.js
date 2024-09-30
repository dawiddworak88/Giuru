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
  marketingApprovalHeader: "Marketing form",
  marketingApprovalText: "If you would like to receive information from us about the current offer, current campaigns, introduced functionalities or changes, as well as other activities conducted, please complete the marketing consents below. Consents may be withdrawn at any time, and the withdrawal of consent does not affect actions taken before the date of its withdrawal.",
  emailMarketingApprovalLabel: "I consent to receiving information electronically, to an e-mail address that I secure",
  smsMarketingApprovalLabel: "I consent to telephone contact for marketing purposes",
  notificationTypes: [
    {
      id: 1,
      isApproved: false,
      name: "Email notifications"
    },
    {
      id: 1,
      isApproved: false,
      name: "SMS notifications"
    }
  ]
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