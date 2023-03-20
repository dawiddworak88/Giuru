import React from "react";
import NewsPage from "../areas/News/pages/NewsPage/NewsPage";
import { header, menuTiles, footer } from "./shared/Props";
import "../areas/News/pages/NewsPage/NewsPage.scss";

const catalogData = {
  title: "News",
  newText: "Add news",
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
      "Title",
      "Category",
      "Last Modified Date",
      "Created Date",
    ],
    properties: [
      {
          title: "title",
          isDateTime: false
      },
      {
        title: "category",
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
            title: "Welcome at B2B Platform",
            category: "Business",
            lastModifiedDate: new Date(),
            createdDate: new Date(),
        },
    ],
    total: 1
  }
};

export const NewsPageStory = () => <NewsPage header={header} menuTiles={menuTiles} footer={footer} catalog={catalogData}/>;

NewsPageStory.story = {
  name: "NewsItems Page",
};

const SellerNewsStories = {
  title: "NewsItems",
  component: NewsPageStory,
};

export default SellerNewsStories;
