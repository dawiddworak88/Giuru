import React from "react";
import NewsItemPage from "../project/Seller.Portal/areas/News/pages/NewsItemPage/NewsItemPage";
import { header, menuTiles, footer } from "./Shared/Props";
import "../project/Seller.Portal/areas/News/pages/NewsItemPage/NewsItemPage.scss";

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
  name: "News item page",
};

const SellerNewsItemStories = {
  title: "SellerPortal.News",
  component: NewsItemPageStory,
};

export default SellerNewsItemStories;