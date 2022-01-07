import React from "react";
import "../project/Seller.Portal/areas/Products/pages/CategoryPage/CategoryPage.scss";
import CategoryPage from "../project/Seller.Portal/areas/Products/pages/CategoryPage/CategoryPage";
import { header, menuTiles, footer } from "./Shared/Props";

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
  title: "SellerPortal.Category",
  component: CategoryPageStory,
};

export default SellerCategoryStories;
