import React from "react";
import "../project/Account/areas/Accounts/pages/SignIn/SignInPage.scss";
import SignInPage from "../project/Account/areas/Accounts/pages/SignIn/SignInPage";
import { header, footer } from "./Shared/Props";

var signInForm = {
  emailRequiredErrorMessage: "Enter e-mail address",
  passwordRequiredErrorMessage: "Enter password",
  emailFormatErrorMessage: "Enter e-mail in a correct format, e.g. your@email.com",
  passwordFormatErrorMessage: "Password must be at least 8 characters long, consist of at least one capital letter, one small letter, a digit and a special character, e.g. P@ssw0rd",
  signInText: "Sign in",
  enterEmailText: "Enter e-mail",
  enterPasswordText: "Enter password"
};

export const SignInPageStory = () => <SignInPage header={header} signInForm={signInForm} footer={footer} />

SignInPageStory.story = {
  name: "Sign in Page",
};

const SignInStories = {
  title: "Account.Pages",
  component: SignInPageStory,
};

export default SignInStories;