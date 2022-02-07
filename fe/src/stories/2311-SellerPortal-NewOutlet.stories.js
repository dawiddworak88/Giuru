import React from "react";
import NewOutletPage from "../project/Seller.Portal/areas/Outlet/pages/NewOutletPage/NewOutletPage";
import { header, menuTiles, footer } from "./Shared/Props";
import "../project/Seller.Portal/areas/Outlet/pages/NewOutletPage/NewOutletPage.scss";

const outletForm = {
  title: "Outlet",
};

export const OutletFormPageStory = () => <NewOutletPage header={header} menuTiles={menuTiles} footer={footer} outletForm={outletForm} />;

OutletFormPageStory.story = {
  name: "Outlet Form Page",
};

const SellerOutletFormStories = {
  title: "SellerPortal.Outlet",
  component: OutletFormPageStory,
};

export default SellerOutletFormStories;
