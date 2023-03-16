import StatusOrder from "../project/AspNetCore/areas/Orders/pages/StatusOrder/StatusOrderPage";
import { header, mainNavigation, footer } from "./Shared/AspNetCoreProps";
import { menuTilesForOrdering } from "./Shared/AspNetCoreProps";
import "../project/AspNetCore/areas/Orders/pages/StatusOrder/StatusOrderPage.scss";

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

export const StatusOrderPageStory = () => <StatusOrder header={header} menuTiles={menuTilesForOrdering} mainNavigation={mainNavigation} footer={footer} statusOrder={statusOrder} />;

StatusOrderPageStory.story = {
    name: "Status order Page"
};

const StatusOrderStories = {
    title: "AspNetCore.Orders",
    component: StatusOrderPageStory
};

export default StatusOrderStories;