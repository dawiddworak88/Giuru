import React from "react";
import UploadMediaPage from "../project/Seller.Portal/areas/Media/pages/UploadMediaPage/UploadMediaPage";
import { header, menuTiles, footer } from "./Shared/Props";
import "../project/Seller.Portal/areas/Media/pages/UploadMediaPage/UploadMediaPage.scss";

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

export const UploadMediaPageStory = () => <UploadMediaPage header={header} menuTiles={menuTiles} footer={footer} uploadForm={formData} />;

UploadMediaPageStory.story = {
  name: "Upload Media Page",
};

const SellerUploadMediaStories = {
  title: "SellerPortal.Media",
  component: UploadMediaPageStory,
};

export default SellerUploadMediaStories;
