import React from "react";
import "../project/Account/areas/Accounts/pages/SetPassword/SetPasswordPage.scss";
import SetPasswordPage from "../project/Account/areas/Accounts/pages/SetPassword/SetPasswordPage";
import { header, footer } from "./Shared/Props";

var setPasswordForm = {
  submitUrl: "#",
  setPasswordText: "Set password",
  passwordLabel: "Password",
  passwordRequiredErrorMessage: "Enter password",
  passwordFormatErrorMessage: "Password must be at least 8 characters long, consist of at least one capital letter, one small letter, a digit and a special character, e.g. P@ssw0rd",
  enterPasswordText: "Enter password"
};

export const SetPasswordPageStory = () => <SetPasswordPage header={header} setPasswordForm={setPasswordForm} footer={footer} />;

SetPasswordPageStory.story = {
  name: "Set Password Page",
};

const SetPasswordStories = {
  title: "Account.Pages",
  component: SetPasswordPageStory,
};

export default SetPasswordStories;