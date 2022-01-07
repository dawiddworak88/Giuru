import React from "react";
import "../project/Account/areas/Accounts/pages/SetPassword/SetPasswordPage.scss";
import SetPasswordPage from "../project/Account/areas/Accounts/pages/SetPassword/SetPasswordPage";
import { header, footer } from "./Shared/Props";

var setPasswordForm = {
  emailRequiredErrorMessage: "Enter e-mail address",
  passwordRequiredErrorMessage: "Enter password",
  emailFormatErrorMessage: "Enter e-mail in a correct format, e.g. your@email.com",
  passwordFormatErrorMessage: "Password must be at least 8 characters long, consist of at least one capital letter, one small letter, a digit and a special character, e.g. P@ssw0rd",
  signInText: "Sign in",
  enterPasswordText: "Enter password"
};

export const SetPasswordPageStory = () => <SetPasswordPage header={header} setPasswordForm={setPasswordForm} footer={footer} />;

SetPasswordPageStory.story = {
  name: "Sign Password Page",
};

const SetPasswordStories = {
  title: "Account.Pages",
  component: SetPasswordPageStory,
};

export default SetPasswordStories;