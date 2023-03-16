import React from "react";
import "../project/Seller.Portal/areas/Products/pages/ProductPage/ProductPage.scss";
import ProductPage from "../project/Seller.Portal/areas/Products/pages/ProductPage/ProductPage";
import { header, menuTiles, footer } from "./Shared/Props";

var productForm = {
  title: "Product",
  nameLabel: "Name:",
  nameRequiredErrorMessage: "Enter name",
  enterNameText: "Enter name",
  selectSchemaLabel: "Select schema",
  skuLabel: "SKU:",
  isNewLabel: "New",
  skuRequiredErrorMessage: "Enter SKU",
  enterSkuText: "Enter SKU",
  saveUrl: "#",
  saveText: "Save",
  generalErrorMessage: "An error has occurred.",
  schema: JSON.stringify({ 
    "definitions": {
      "Color": {
        "title": "Color",
        "type": "string",
        "anyOf": [
          {
            "type": "string",
            "enum": [
              "#ff0000"
            ],
            "title": "Red"
          },
          {
            "type": "string",
            "enum": [
              "#00ff00"
            ],
            "title": "Green"
          },
          {
            "type": "string",
            "enum": [
              "#0000ff"
            ],
            "title": "Blue"
          }
        ]
      },
      "Fabrics": {
        "title": "Fabrics",
        "type": "string",
        "anyOf": [
          {
            "type": "string",
            "enum": [
              "1"
            ],
            "title": "Monolith 77"
          },
          {
            "type": "string",
            "enum": [
              "2"
            ],
            "title": "Sawana 14"
          },
          {
            "type": "string",
            "enum": [
              "3"
            ],
            "title": "Berlin 01"
          }
        ]
      },
      "Material": {
        "title": "Material",
        "type": "string",
        "anyOf": [
          {
            "type": "string",
            "enum": [
              "11"
            ],
            "title": "Foam-T25"
          },
          {
            "type": "string",
            "enum": [
              "12"
            ],
            "title": "Frame-Wood"
          },
          {
            "type": "string",
            "enum": [
              "13"
            ],
            "title": "Legs-Plastic"
          }
        ]
      }
    },
    "type": "object",
    "properties": {
      "primaryColor": {
        "$ref": "#/definitions/Color",
        "title": "Primary Color:"
      },
      "secondaryColor": {
        "$ref": "#/definitions/Color",
        "title": "Secondary Color:"
      },
      "primaryFabrics": {
        "type": "array",
        "uniqueItems": true,
        "items": {
          "$ref": "#/definitions/Fabrics"
        },
        "title": "Primary Fabrics:"
      },
      "secondaryFabrics": {
        "type": "array",
        "uniqueItems": true,
        "items": {
          "$ref": "#/definitions/Fabrics"
        },
        "title": "Secondary Fabrics:"
      },
      "materials": {
        "type": "array",
        "uniqueItems": true,
        "items": {
          "$ref": "#/definitions/Material"
        },
        "title": "Materials:"
      },
      "description": {
        "type": "string",
        "title": "Description:"
      },
      "minimumQuantity": {
        "type": "number",
        "title": "Minimum Quantity:"
      },
      "packages": {
        "type": "number",
        "title": "Packages:"
      },
      "width": {
        "type": "number",
        "title": "Width:"
      },
      "height": {
        "type": "number",
        "title": "Height:"
      },
      "depth": {
        "type": "number",
        "title": "Depth:"
      },
      "sleepingAreaWidth": {
        "type": "number",
        "title": "Width of Sleeping Area:"
      },
      "sleepingAreaLength": {
        "type": "number",
        "title": "Length of Sleeping Area:"
      },
      "heightOfSitting": {
        "type": "number",
        "title": "Height of Sitting:"
      },
      "package1Volume": {
        "type": "number",
        "title": "Package 1 Volume:"
      },
      "package1GrossWeight": {
        "type": "number",
        "title": "Package 1 Gross Weight:"
      },
      "package1Length": {
        "type": "number",
        "title": "Package 1 Length:"
      },
      "package1Width": {
        "type": "number",
        "title": "Package 1 Width:"
      },
      "package1Height": {
        "type": "number",
        "title": "Package 1 Height:"
      },
      "package2Volume": {
        "type": "number",
        "title": "Package 2 Volume:"
      },
      "package2GrossWeight": {
        "type": "number",
        "title": "Package 2 Gross Weight:"
      },
      "package2Length": {
        "type": "number",
        "title": "Package 2 Length:"
      },
      "package2Width": {
        "type": "number",
        "title": "Package 2 Width:"
      },
      "package2Height": {
        "type": "number",
        "title": "Package 2 Height:"
      },
      "package3Volume": {
        "type": "number",
        "title": "Package 3 Volume:"
      },
      "package3GrossWeight": {
        "type": "number",
        "title": "Package 3 Gross Weight:"
      },
      "package3Length": {
        "type": "number",
        "title": "Package 3 Length:"
      },
      "package3Width": {
        "type": "number",
        "title": "Package 3 Width:"
      },
      "package3Height": {
        "type": "number",
        "title": "Package 3 Height:"
      },
      "package4Volume": {
        "type": "number",
        "title": "Package 4 Volume:"
      },
      "package4GrossWeight": {
        "type": "number",
        "title": "Package 4 Gross Weight:"
      },
      "package4Length": {
        "type": "number",
        "title": "Package 4 Length:"
      },
      "package4Width": {
        "type": "number",
        "title": "Package 4 Width:"
      },
      "package4Height": {
        "type": "number",
        "title": "Package 4 Height:"
      },
      "package5Volume": {
        "type": "number",
        "title": "Package 5 Volume:"
      },
      "package5GrossWeight": {
        "type": "number",
        "title": "Package 5 Gross Weight:"
      },
      "package5Length": {
        "type": "number",
        "title": "Package 5 Length:"
      },
      "package5Width": {
        "type": "number",
        "title": "Package 5 Width:"
      },
      "package5Height": {
        "type": "number",
        "title": "Package 5 Height:"
      },
      "intrastat": {
        "type": "string",
        "title": "Intrastat:"
      }
    },
    uiSchema: null
  })
};

export const ProductPageStory = () => <ProductPage header={header} menuTiles={menuTiles} productForm={productForm} footer={footer} />

ProductPageStory.story = {
  name: "Product Page",
};

const SellerProductStories = {
  title: "SellerPortal.Product",
  component: ProductPageStory,
};

export default SellerProductStories;