import React from "react";
import "../project/Seller.Portal/areas/Orders/pages/OrderPage/OrderPage.scss";
import OrderPage from "../project/Seller.Portal/areas/Orders/pages/OrderPage/OrderPage";
import { header, menuTiles, footer } from "./Shared/Props";

var props = {
  title: "Orders",
  newText: "New order",
  newUrl: "#"
};

export const OrderPageStory = () => <OrderPage header={header} menuTiles={menuTiles} {...props} footer={footer} />

OrderPageStory.story = {
  name: "Orders Page",
};

export default {
  title: "SellerPortal.Orders",
  component: OrderPageStory,
};