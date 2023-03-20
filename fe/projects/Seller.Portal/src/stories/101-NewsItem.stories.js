import React from "react";
import NewsItemPage from "../areas/News/pages/NewsItemPage/NewsItemPage";
import { header, menuTiles, footer } from "./shared/Props";
import "../areas/News/pages/NewsItemPage/NewsItemPage.scss";

const formData = {
  title: "News item",
  nameLabel: "Name",
  parentCategoryLabel: "Parent category",
  saveText: "Save",
  titleLabel: "Title",
  nameRequiredErrorMessage: "Enter the name of category",
  dropOrSelectImagesLabel: "Drop or select images",
  dropFilesLabel: "Drop files",
  deleteLabel: "Delete",
  generalErrorMessage: "An error has occurred",
  imagesRequiredErrorMessage: "Images are required",
  mediaItemsLabel: "Upload media",
  saveMediaText: "Upload images",
  saveMediaUrl: "#"
};

export const NewsItemPageStory = () => <NewsItemPage header={header} menuTiles={menuTiles} footer={footer} newsItemForm={formData}/>;

NewsItemPageStory.story = {
  name: "NewsItem page",
};

const SellerNewsItemStories = {
  title: "NewsItems",
  component: NewsItemPageStory,
};

export default SellerNewsItemStories;