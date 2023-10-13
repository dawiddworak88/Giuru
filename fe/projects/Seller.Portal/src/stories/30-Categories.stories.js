import React from "react";
import "../areas/Products/pages/CategoriesPage/CategoriesPage.scss";
import CategoriesPage from "../areas/Products/pages/CategoriesPage/CategoriesPage";
import { header, menuTiles, footer } from "./shared/Props";

var catalog = {
    defaultItemsPerPage: 25,
    title: "Categories",
    newText: "New category",
    newUrl: "#",
    searchApiUrl: "#",
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
    isDragDropEnable: true,
    prevPageAreaText: "Move To Previous Page",
    nextPageAreaText: "Move To Next Page",
    table: {
        actions: [
            {
                isDragDropOrderEnabled: true
            },
            {
                isEdit: true
            },
            {
                isDelete: true
            },
            {
                isDuplicate: true
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
                createdDate: new Date(),
                order: "1",
                level: 2
            },
            {
                id: "2",
                name: "Mirrors",
                parentName: "Bathroom",
                lastModifiedDate: new Date(),
                createdDate: new Date(),
                order: "2",
                level: 1
            },
            {
                id: "3",
                name: "Lamp",
                parentName: "Bedroom",
                lastModifiedDate: new Date(),
                createdDate: new Date(),
                order: "3",
                level: 2
            },
            {
                id: "4",
                name: "Table",
                parentName: "Kitchen",
                lastModifiedDate: new Date(),
                createdDate: new Date(),
                order: "4",
                level: 2
            },
            {
                id: "5",
                name: "Armchair",
                parentName: "Living Room",
                lastModifiedDate: new Date(),
                createdDate: new Date(),
                order: "5",
                level: 1
            }
        ],
        total: 5
    }
};

export const CategoriesPageStory = () => <CategoriesPage header={header} menuTiles={menuTiles} catalog={catalog} footer={footer} />

CategoriesPageStory.story = {
  name: "Categories Page",
};

const SellerCategorieStories = {
    title: "Products",
    component: CategoriesPageStory
};

export default SellerCategorieStories;