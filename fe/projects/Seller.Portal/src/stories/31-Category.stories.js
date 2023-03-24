import React from "react";
import "../areas/Products/pages/CategoryPage/CategoryPage.scss";
import CategoryPage from "../areas/Products/pages/CategoryPage/CategoryPage";
import { header, menuTiles, footer } from "./shared/Props";

var categoryForm = {
  title: "Category",
  nameLabel: "Name:",
  nameRequiredErrorMessage: "Enter name",
  enterNameText: "Enter name",
  parentCategoryLabel: "Parent category:",
  saveUrl: "#",
  saveText: "Save",
  generalErrorMessage: "An error has occurred.",
  parentCategories: [
    {
      id: 1,
      name: "Living Room"
    },
    {
      id: 2,
      name: "Bedroom"
    }
  ]
};

export const CategoryPageStory = () => <CategoryPage header={header} menuTiles={menuTiles} categoryForm={categoryForm} footer={footer} />

CategoryPageStory.story = {
  name: "Category Page",
};

const SellerCategoryStories = { 
  title: "Products",
  component: CategoryPageStory,
};

export default SellerCategoryStories;
