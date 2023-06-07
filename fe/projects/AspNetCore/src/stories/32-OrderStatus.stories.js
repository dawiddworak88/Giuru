import StatusOrder from "../areas/Orders/pages/StatusOrder/StatusOrderPage";
import { header, mainNavigation, footer } from "./shared/Props";
import "../areas/Orders/pages/StatusOrder/StatusOrderPage.scss";

const statusOrder = {
    title: "Status order",
    skuLabel: "SKU",
    nameLabel: "Name",
    expectedDeliveryLabel: "Expected delivery",
    expectedDelivery: new Date(),
    quantityLabel: "Quantity",
    externalReferenceLabel: "External reference",
    deliveryFromLabel: "Delivery from",
    deliveryToLabel: "Delivery to",
    moreInfoLabel: "More info",
    orderItemsLabel: "Order items",
    orderStatusLabel: "Status",
    orderStatuses: [
        {
            id: "",
            name: "New"
        }
    ],
    orderItems: [
        {
            productUrl: "https://b2badmin.eltap.com/en/Products/Product/Edit/a5fd574a-83c9-4d26-97ee-08d92e360cb4",
            sku: "An01",
            name: "Anton",
            quantity: 1,
        },
        {
            productUrl: "https://b2badmin.eltap.com/en/Products/Product/Edit/a5fd574a-83c9-4d26-97ee-08d92e360cb4",
            sku: "An02",
            name: "Anton",
            quantity: 1
        }
    ]
};

export const StatusOrderPageStory = () => <StatusOrder header={header} mainNavigation={mainNavigation} footer={footer} statusOrder={statusOrder} />;

StatusOrderPageStory.story = {
    name: "Status Order Page"
};

const StatusOrderStories = {
    title: "Orders",
    component: StatusOrderPageStory
};

export default StatusOrderStories;