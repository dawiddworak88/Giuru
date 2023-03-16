import React from "react";
import "../project/Seller.Portal/areas/Products/pages/CategoriesPage/CategoriesPage.scss";
import CategoriesPage from "../project/Seller.Portal/areas/Products/pages/CategoriesPage/CategoriesPage";
import { header, menuTiles, footer } from "./Shared/Props";

var catalog = {
    title: "Categories",
    newText: "New category",
    newUrl: "#",
    noLabel: "No",
    yesLabel: "Yes",
    deleteConfirmationLabel: "Delete confirmation",
    areYouSureLabel: "Are you sure you want to delete this item",
    generalErrorMessage: "An Error Occurred",
    searchLabel: "Search",
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
            "Parent name",
            "Last modified date",
            "Created date"
        ],
        properties: [
            {
                title: "name",
                isDateTime: false
            },
            {
                title: "parentName",
                isDateTime: false
            },
            {
                title: "lastModifiedDate",
                isDateTime: true
            },
            {
                title: "createdDate",
                isDateTime: true
            }
        ]
    },
    pagedItems: {
        data: [
            {
                id: "1",
                name: "Sectionals",
                parentName: "Living Room",
                lastModifiedDate: new Date(),
                createdDate: new Date()
            }
        ],
        total: 1
    }
};

export const CategoriesPageStory = () => <CategoriesPage header={header} menuTiles={menuTiles} catalog={catalog} footer={footer} />

CategoriesPageStory.story = {
  name: "Categories Page",
};

const SellerCategorieStories = {
    title: "SellerPortal.Categories",
    component: CategoriesPageStory
};

export default SellerCategorieStories;