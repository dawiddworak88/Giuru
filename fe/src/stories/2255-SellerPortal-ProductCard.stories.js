import React from "react";
import ProductCard from "../project/Seller.Portal/areas/Products/pages/ProductCardPage/ProductCardPage";
import { header, menuTiles, footer } from "./Shared/Props";
import "../project/Seller.Portal/areas/Products/pages/ProductCardPage/ProductCardPage.scss";

const componentProps = {
  title: "Edit product card",
  navigateToProductCardsLabel: "Back to products cards",
  uiSchema: null,
  fieldRequiredErrorMessage: "Field is required",
  generalErrorMessage: "An Error Occurred",
  saveUrl: "",
  saveText: "Save",
  defaultInputName: "NewElement",
  newText: "Add new card",
  productAttributeExistsMessage: "Attribute with that name already exists.",
  productCardModal: { 
    title: "Edit product attribute", 
    nameLabel: "Name", 
    displayNameLabel: "Display name", 
    definitionLabel: "Definition", 
    saveText: "Save", 
    cancelText: "Cancel", 
    toDefinitionText: "Go to definition",
    inputTypeLabel: "Type", 
    inputTypes: [
      {
        value: "reference",
        text: "Reference"
      },
      {
        value: "string",
        text: "String"
      },
      {
        value: "array",
        text: "Array"
      },
      {
        value: "boolean",
        text: "Boolean"
      },
      {
        value: "number",
        text: "Number"
      }
    ], 
    definitionsOptions: [
      {
        name: "Color",
        id: "a04b3368-fa25-4b4a-e4eb-08d907680a85",
        url: "#"
      },
      {
        name: "Shape",
        id: "3cb42bca-84d9-423f-e4ee-08d907680a85",
        url: "#"
      }
    ]
  },
  schema: '{}'
};

export const ProductCardPageStory = () => <ProductCard header={header} menuTiles={menuTiles} footer={footer} productCardForm={componentProps} />;

ProductCardPageStory.story = {
  name: "Product Card Page",
};

const SellerProductCardStories = {
  title: "SellerPortal.Products",
  component: ProductCardPageStory,
};

export default SellerProductCardStories;
