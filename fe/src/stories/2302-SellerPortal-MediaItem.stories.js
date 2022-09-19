import React from "react";
import MediaItemPage from "../project/Seller.Portal/areas/MediaItems/pages/MediaItemPage/MediaItemPage";
import { header, menuTiles, footer } from "./Shared/Props";
import "../project/Seller.Portal/areas/MediaItems/pages/MediaItemPage/MediaItemPage.scss";

const formData = {
  title: "Media",
  dropOrSelectImagesLabel: "Drop or select images",
  dropFilesLabel: "Drop files",
  deleteLabel: "Delete",
  generalErrorMessage: "An error has occurred",
  imagesRequiredErrorMessage: "Images are required",
  mediaItemsLabel: "Upload media",
  saveMediaText: "Upload images",
  saveMediaUrl: "#",
  nameLabel: "Name: ",
  descriptionLabel: "Description:",
  versions: [
    {
      id: "849eab28-b92e-43d9-0458-08d9e0a8a954",
      url: "http://host.docker.internal:5131/api/v1/files/49371bde-77b0-4c98-0eca-08d9e0a8a949?w=200&h=120&o=true",
      mimeType: "image/jpeg",
      version: 1
    },
    {
      id: "849eab28-b92e-43d9-0458-08d9e0a8a954",
      url: null,
      mimeType: "application/pdf",
      version: 1
    },
  ]
};

export const MediaItemPageStory = () => <MediaItemPage header={header} menuTiles={menuTiles} footer={footer} mediaItemForm={formData} />;

MediaItemPageStory.story = {
  name: "Media item Page",
};

const SellerMediaItemStories = {
  title: "SellerPortal.Media",
  component: MediaItemPageStory,
};

export default SellerMediaItemStories;
