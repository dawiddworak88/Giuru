import React from "react";
import MediaPage from "../project/Seller.Portal/areas/Medias/pages/MediaPage/MediaPage";
import { header, menuTiles, footer } from "./Shared/Props";
import "../project/Seller.Portal/areas/Medias/pages/MediaPage/UploadMediaPage.scss";

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

export const MediaPageStory = () => <MediaPage header={header} menuTiles={menuTiles} footer={footer} mediaForm={formData} />;

MediaPageStory.story = {
  name: "Media Page",
};

const SellerMediaStories = {
  title: "SellerPortal.Media",
  component: MediaPageStory,
};

export default SellerMediaStories;
