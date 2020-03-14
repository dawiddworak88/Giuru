import React from 'react';
import '../project/Account/areas/Accounts/pages/SignIn/SignInPage.scss';
import SignInPage from '../project/Account/areas/Accounts/pages/SignIn/SignInPage';

var header = {
  logo: {
    targetUrl: '/',
    logoAltLabel: 'Logo'
  },
  languageSwitcher: {
    availableLanguages: [
      {
        uniqueId: 'EN',
        url: '#',
        text: 'EN'
      },
      {
        uniqueId: 'DE',
        url: '#',
        text: 'DE'
      },
      {
        uniqueId: 'PL',
        url: '#',
        text: 'PL'
      }
    ],
    selectedLanguageUniqueId: 'PL'
  },
  links: [
    { uniqueId: "1", url: "#privacy-policy", text: "Price List" },
    { uniqueId: "2", url: "#regulations", text: "Regulations" }
  ],
  loginLink: {
    url: "#", 
    text: "Sign in"
  }
};

var signInForm = {
  emailRequiredErrorMessage: "Enter e-mail address",
  passwordRequiredErrorMessage: "Enter password",
  emailFormatErrorMessage: "Enter e-mail in a correct format, e.g. your@email.com",
  passwordFormatErrorMessage: "Password must be at least 8 characters long, consist of at least one capital letter, one small letter, a digit and a special character, e.g. P@ssw0rd",
  signInText: "Sign in",
  enterEmailText: "Enter e-mail",
  enterPasswordText: "Enter password"
};

var footer = {
  copyright: 'Copyright © 2021 Giuru',
  links: [
    { uniqueId: "1", url: "#privacy-policy", text: "Price List" },
    { uniqueId: "2", url: "#regulations", text: "Regulations" }
  ]
};

export const SignInPageStory = () => <SignInPage header={header} signInForm={signInForm} footer={footer} />

SignInPageStory.story = {
  name: 'Sign in Page',
};

export default {
  title: 'Account.Pages',
  component: SignInPageStory,
};