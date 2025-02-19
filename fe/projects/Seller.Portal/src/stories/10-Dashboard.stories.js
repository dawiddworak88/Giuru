import React from "react";
import DashboardPage from "../areas/Dashboard/pages/DashboardPage/DashboardPage";
import { header, menuTiles, footer } from "./shared/Props";
import "../areas/Dashboard/pages/DashboardPage/DashboardPage.scss";

const componentProps = {
    title: "Dashboard",
    countrySalesAnalytics: {
        title: "Last 3 months sales by country",
        datePickerViews: ["month", "year"],
        chartLables: [
            "Poland",
            "Germany",
            "Czech Republic",
            "France"
        ],
        chartDatasets: [
            {
                data: [12, 56, 86, 2],
                backgroundColor: ["#ea5545", "#f46a9b", "#ef9b20", "#edbf33", "#ede15b", "#bdcf32", "#87bc45", "#27aeef", "#b33dc6"]
            }
        ]
    },
    dailySalesAnalytics: {
        title: "Daily sales",
        datePickerViews: ["day", "month", "year"],
        chartLables: [
            "Monday - 8.12",
            "Tuesday - 9.12",
            "Wednesday - 10.12",
            "Tuesday - 11.12",
            "Friday - 12.12",
            "Saturday - 13.12",
            "Sunday - 14.12"
        ],
        chartDatasets: [
            {
                data: [12, 55, 34, 120, 320, 19, 67],
                borderColor: "#1B5A6E",
                backgroundColor: "#1B5A6E"
            }
        ]
    },
    productsSalesAnalytics: {
        title: "Top sales products",
        nameLabel: "Name",
        skuLabel: "Sku",
        quantityLabel: "Quantity",
        noResultsLabel: "No results",
        products: [
            {
                id: "1",
                name: "Anton",
                sku: "An_01",
                quantity: 33
            },
            {
                id: "2",
                name: "Anton",
                sku: "An_21",
                quantity: 19
            },
            {
                id: "3",
                name: "Anton",
                sku: "An_15",
                quantity: 17
            },
            {
                id: "4",
                name: "Anton",
                sku: "An_11",
                quantity: 3
            },
            {
                id: "1",
                name: "Anton",
                sku: "An_01",
                quantity: 33
            },
            {
                id: "2",
                name: "Anton",
                sku: "An_21",
                quantity: 19
            },
            {
                id: "3",
                name: "Anton",
                sku: "An_15",
                quantity: 17
            },
            {
                id: "4",
                name: "Anton",
                sku: "An_11",
                quantity: 3
            }
        ]
    }
}

export const DashboardPageStory = () => <DashboardPage header={header} menuTiles={menuTiles} dashboardDetail={componentProps} footer={footer}/>;

DashboardPageStory.story = {
  name: "Dashboard Page"
};

const SellerDashboardStories = {
  title: "Dashboard",
  component: DashboardPageStory
};

export default SellerDashboardStories;
