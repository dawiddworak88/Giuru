import NewOrderPage from "../project/AspNetCore/areas/Orders/pages/NewOrder/NewOrderPage";
import { header, mainNavigation, footer } from "./Shared/AspNetCoreProps";
import { menuTilesForOrdering } from "./Shared/AspNetCoreProps";
import "../project/AspNetCore/areas/Orders/pages/NewOrder/NewOrderPage.scss";

const orderForm = {
    title: "Order",
    searchPlaceholderLabel: "Enter SKU or product name",
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
    
};

export const NewOrderPageStory = () => <NewOrderPage header={header} menuTiles={menuTilesForOrdering}  mainNavigation={mainNavigation} footer={footer} newOrderForm={orderForm} />;

NewOrderPageStory.story = {
    name: "New Order Page"
};

const NewOrderStories = {
    title: "AspNetCore.Orders",
    component: NewOrderPageStory
};

export default NewOrderStories;