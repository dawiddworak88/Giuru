import React from "react";
import "../areas/Clients/pages/ClientAddressPage/ClientAddressPage.scss";
import ClientAddressPage from "../areas/Clients/pages/ClientAddressPage/ClientAddressPage";
import { header, menuTiles, footer } from "./shared/Props";

const componentProps = {
  title: "Edit client address",
  generalErrorMessage: "An error has occurred",
  fieldRequiredErrorMessage: "Field is required",
  navigateToClientAddresses: "Back to client addresses",
  recipientLabel: "Recipient",
  phoneNumberLabel: "Phone number",
  streetLabel: "Street",
  regionLabel: "Region",
  postCodeLabel: "Post Code",
  cityLabel: "City",
  countryLabel: "Country",
  saveText: "Save",
  clientLabel: "Client",
  countries: [
    { id: "1", name: "Poland" }
  ],
  clients: [
    { id: "1", name: "ELTAP"}
  ]
};

export const ClientAddressPageStory = () => <ClientAddressPage header={header} menuTiles={menuTiles} clientAddressForm={componentProps} footer={footer} />

ClientAddressPageStory.story = {
  name: "Client Address Page",
};

const SellerClientAddressStories = {
  title: "Clients",
  component: ClientAddressPageStory,
};

export default SellerClientAddressStories;
