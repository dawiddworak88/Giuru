import React from "react";
import MediaPage from "../areas/MediaItems/pages/MediaPage/MediaPage";
import { header, menuTiles, footer } from "./shared/Props";
import "../areas/MediaItems/pages/MediaPage/MediaPage.scss";

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
  title: "MediaItems",
  component: MediaPageStory,
};

export default SellerMediaStories;
