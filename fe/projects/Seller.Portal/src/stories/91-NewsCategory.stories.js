import React from "react";
import CategoryPage from "../areas/News/pages/CategoryPage/CategoryPage";
import { header, menuTiles, footer } from "./shared/Props";
import "../areas/News/pages/CategoryPage/CategoryPage.scss";

const formData = {
  title: "Category",
  nameLabel: "Name",
  parentCategoryLabel: "Parent category",
  saveText: "Save",
  nameRequiredErrorMessage: "Enter the name of category"
};

export const NewsCategoryPageStory = () => <CategoryPage header={header} menuTiles={menuTiles} footer={footer} categoryForm={formData}/>;

NewsCategoryPageStory.story = {
  name: "Category Page",
};

const SellerNewsCategoryStories = {
  title: "NewsItems",
  component: NewsCategoryPageStory,
};

export default SellerNewsCategoryStories;