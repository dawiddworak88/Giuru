import NewOrderPage from "../areas/Orders/pages/NewOrder/NewOrderPage";
import { header, mainNavigation, footer } from "./shared/Props";
import "../areas/Orders/pages/NewOrder/NewOrderPage.scss";

const orderForm = {
    title: "Order",
    searchPlaceholderLabel: "Enter SKU or product name",
    quantityLabel: "Quantity",
    stockQuantityLabel: "Stock Quantity",
    outletQuantityLabel: "Outlet Quantity",
    externalReferenceLabel: "External order number",
    moreInfoLabel: "More info",
    deliveryToLabel: "Delivery To",
    deliveryFromLabel: "Delivery From",
    addText: "Add",
    noOrderItemsLabel: "The cart is empty. Add first order item.",
    generalErrorMessage: "An error has occurred",
    communicationLanguage: "",
    uploadOrderFileUrl: "",
    saveText: "Place order",
    searchLabel: "Search",
    dropOrSelectFilesLabel: "Drag and drop here, or click to select an order file",
    noResultsLabel: "No results have been found",
    maximalLabel: "max:"
};

export const NewOrderPageStory = () => <NewOrderPage header={header} mainNavigation={mainNavigation} footer={footer} newOrderForm={orderForm} />;

NewOrderPageStory.story = {
    name: "Order Page"
};

const NewOrderStories = {
    title: "Orders",
    component: NewOrderPageStory
};

export default NewOrderStories;