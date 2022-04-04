import React from "react";
import CategoriesPage from "../project/Seller.Portal/areas/News/pages/CategoriesPage/CategoriesPage";
import { header, menuTiles, footer } from "./Shared/Props";
import "../project/Seller.Portal/areas/News/pages/CategoriesPage/CategoriesPage.scss";

const catalogData = {
  title: "News Categories",
  newText: "New category",
  newUrl: "#",
  noLabel: "No",
  yesLabel: "Yes",
  deleteConfirmationLabel: "Delete confirmation",
  areYouSureLabel: "Are you sure you want to delete this item",
  generalErrorMessage: "An Error Occurred",
  editLabel: "Edit",
  deleteLabel: "Delete",
  displayedRowsLabel: "of",
  rowsPerPageLabel: "Rows per Page",
  backIconButtonText: "Previous",
  nextIconButtonText: "Next",
  editUrl: "#",
  deleteUrl: "#",
  noResultsLabel: "There are no results",
  table: {
    actions: [
      {
          isEdit: true
      },
      {
          isDelete: true
      }
    ],
    labels: [
      "Name",
      "Parent Category",
      "Last Modified Date",
      "Created Date",
    ],
    properties: [
      {
          title: "name",
          isDateTime: false
      },
      {
        title: "parentCategory",
        isDateTime: false
      },
      {
        title: "lastModifiedDate",
        isDateTime: true
      },
      {
        title: "createdDated",
        isDateTime: true
      },
    ]
  },
  pagedItems: {
    data: [
        {
            id: "1",
            name: "Special offer",
            parentCategory: "Business",
            lastModifiedDate: new Date(),
            createdDate: new Date(),
        },
    ],
    total: 1
  }
};

export const NewsCategoriesPageStory = () => <CategoriesPage header={header} menuTiles={menuTiles} footer={footer} catalog={catalogData}/>;

NewsCategoriesPageStory.story = {
  name: "Categories Page",
};

const SellerNewsCategoriesStories = {
  title: "SellerPortal.News",
  component: NewsCategoriesPageStory,
};

export default SellerNewsCategoriesStories;
