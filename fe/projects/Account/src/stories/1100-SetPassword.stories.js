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
  enterPasswordText: "Enter password"
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