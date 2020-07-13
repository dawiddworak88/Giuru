import React from 'react';
import '../project/Seller.Portal/areas/Clients/pages/ClientDetailPage/ClientDetailPage.scss';
import ClientDetailPage from '../project/Seller.Portal/areas/Clients/pages/ClientDetailPage/ClientDetailPage';
import { header, menuTiles, footer } from './Shared/Props';

var clientDetailForm = {

  generalErrorMessage: "An error has occurred",
  nameLabel: "Name:",
  emailLabel: "E-mail:",
  languageLabel: "Communication language:",
  nameRequiredErrorMessage: "Enter name",
  emailRequiredErrorMessage: "Enter e-mail address",
  languageRequiredErrorMessage: "Select langauge",
  emailFormatErrorMessage: "Enter e-mail in a correct format, e.g. your@email.com",
  clientDetailText: "Client",
  enterNameText: "Enter name",
  enterEmailText: "Enter e-mail",
  saveUrl: "#",
  languages: [
    { value: "", text: "Select language" },
    { value: "EN", text: "EN" },
    { value: "DE", text: "DE" },
    { value: "PL", text: "PL" }
  ],
  communicationLanguage: "",
  saveText: "Save"
};

export const ClientDetailPageStory = () => <ClientDetailPage header={header} menuTiles={menuTiles} title="Client" clientDetailForm={clientDetailForm} footer={footer} />

ClientDetailPageStory.story = {
  name: 'Client Detail Page',
};

export default {
  title: 'SellerPortal.Client',
  component: ClientDetailPageStory,
};