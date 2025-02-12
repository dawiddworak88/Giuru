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
  personalDataAdministratorText: "The administrator of the personal data provided in the form is EELTAP Spółka z ograniczoną odpowiedzialnością Sp. k. The principles of data processing and your related rights are described in the",
  privacyPolicy: "privacy policy",
  privacyPolicyUrl: "#",
  approvals: [
    {
      id: 1,
      name: "I want to receive information about news, promotions, products, or services of ELTAP Spółka z ograniczoną odpowiedzialnością Sp. k. under the terms specified in the privacy policy."
    },
    {
      id: 2,
      name: "I consent to being contacted by phone and receiving SMS messages for the purpose of marketing ELTAP Spółka z ograniczoną odpowiedzialnością Sp. k. products or services under the terms specified in the privacy policy."
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