import DashboardPage from "../project/AspNetCore/areas/Dashboard/pages/DashboardPage/DashboardPage";
import { header, mainNavigation, footer } from "./Shared/AspNetCoreProps";
import "../project/AspNetCore/areas/Dashboard/pages/DashboardPage/DashboardPage.scss";

const componentProps = {
    title: "Order analysis",
    nameLabel: "Product name",
    quantityLabel: "Number of products",
    noResultsLabel: "There are no results",
    topOrderedProducts: "TOP 10 - Ordered products",
    salesAnalytics: {
        title: "Number of orders",
        datePickerViews: ["day", "month", "year"],
        chartLables: [
            "January",
            "February",
            "March",
            "April",
            "May",
            "June",
            "July",
            "August",
            "September",
            "October",
            "November",
            "December"
        ],
        chartDatasets: [
            {
                data: [95, 75, 0, 120, 31, 19, 55, 32, 241, 311, 32, 85],
            },
            {
                data: [12, 55, 34, 120, 320, 19, 67, 121, 241, 87, 85, 85]
            }
        ]
    },
    products: [
        {
            name: "Anton",
            sku: "An_25",
            quantity: 21
        },
        {
            name: "Porto",
            sku: "PO_01",
            quantity: 17
        },
        {
            name: "Anton",
            sku: "An_07",
            quantity: 13
        },
        {
            name: "Porto",
            sku: "PO_19",
            quantity: 4
        }
    ]
}

export const DashboardPageStory = () => <DashboardPage header={header} mainNavigation={mainNavigation} ordersAnalyticsDetail={componentProps} footer={footer} />;

DashboardPageStory.story = {
    name: "Analytics Dashboard"
};

const DashboardStories = {
    title: "AspNetCore.Dashboard",
    component: DashboardPageStory
};

export default DashboardStories;