import React from "react";
import EditMediaPage from "../project/Seller.Portal/areas/Media/pages/EditMediaPage/EditMediaPage";
import { header, menuTiles, footer } from "./Shared/Props";
import "../project/Seller.Portal/areas/Media/pages/EditMediaPage/EditMediaPage.scss";

const formData = {
  title: "Media",
  dropOrSelectImagesLabel: "Drop or select images",
  dropFilesLabel: "Drop files",
  deleteLabel: "Delete",
  generalErrorMessage: "An error has occurred",
  mediaItemsLabel: "Upload media",
  saveMediaText: "Upload images",
  saveMediaUrl: "#"
};

export const EditMediaPageStory = () => <EditMediaPage header={header} menuTiles={menuTiles} footer={footer} editForm={formData} />;

EditMediaPageStory.story = {
  name: "Edit Media Page",
};

const SellerEditMediaStories = {
  title: "SellerPortal.Media",
  component: EditMediaPageStory,
};

export default SellerEditMediaStories;
