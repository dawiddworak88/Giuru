import React from "react";
import "../areas/Clients/pages/ClientDeliveryAddressPage/ClientDeliveryAddressPage.scss";
import ClientDeliveryAddressPage from "../areas/Clients/pages/ClientDeliveryAddressPage/ClientDeliveryAddressPage";
import { header, menuTiles, footer } from "./shared/Props";

const componentProps = {
  title: "Edit client delivery address",
  generalErrorMessage: "An error has occurred",
  fieldRequiredErrorMessage: "Field is required",
  navigateToClientDeliveryAddresses: "Back to client delivery addresses",
  recipientLabel: "Recipient",
  phoneNumberLabel: "Phone number",
  streetLabel: "Street",
  regionLabel: "Region",
  postCodeLabel: "Post Code",
  cityLabel: "City",
  countryLabel: "Country",
  saveText: "Save",
  countries: [
    {
      id: "1",
      name: "Poland"
    }
  ]
};

export const ClientDeliveryAddressPageStory = () => <ClientDeliveryAddressPage header={header} menuTiles={menuTiles} clientDeliveryAddressForm={componentProps} footer={footer} />

ClientDeliveryAddressPageStory.story = {
  name: "Client Delivery Address Page",
};

const SellerClientDeliveryAddressStories = {
  title: "Clients",
  component: ClientDeliveryAddressPageStory,
};

export default SellerClientDeliveryAddressStories;
