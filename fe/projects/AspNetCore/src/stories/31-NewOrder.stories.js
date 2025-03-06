import NewOrderPage from "../areas/Orders/pages/NewOrder/NewOrderPage";
import { header, mainNavigation, footer } from "./shared/Props";
import "../areas/Orders/pages/NewOrder/NewOrderPage.scss";

const orderForm = {
    title: "Order",
    searchPlaceholderLabel: "Enter SKU or product name",
    nameLabel: "Name:",
    skuLabel: "SKU:",
    quantityLabel: "Quantity",
    stockQuantityLabel: "Stock Quantity",
    outletQuantityLabel: "Outlet Quantity",
    inTotalLabel: "In total:",
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
    maximalLabel: "max:",
    basketItems: [
        {
            productUrl: "#",
            imageSrc: "https://media-test.eltap.com/api/v1/files/8e3d57b4-f976-4c61-1b19-08dc11b07047",
            imageAlt: "Image",
            productId: '1',
            sku: 'An_21',
            name: 'Anton',
            quantity: 1,
            stockQuantity: 1,
            outletQuantity: 1,
        }
    ]
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