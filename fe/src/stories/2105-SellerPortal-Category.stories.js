import React from "react";
import "../project/Seller.Portal/areas/Products/pages/CategoryPage/CategoryPage.scss";
import CategoryPage from "../project/Seller.Portal/areas/Products/pages/CategoryPage/CategoryPage";
import { header, menuTiles, footer } from "./Shared/Props";

var categoryDetailForm = {

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

export const CategoryPageStory = () => <CategoryPage header={header} menuTiles={menuTiles} title="Product" categoryDetailForm={categoryDetailForm} footer={footer} />

CategoryPageStory.story = {
  name: "Category Page",
};

export default {
  title: "SellerPortal.Category",
  component: CategoryPageStory,
};
