import OutletPage from "../project/AspNetCore/areas/Products/pages/OutletPage/OutletPage";
import { header, mainNavigation, footer } from "./Shared/AspNetCoreProps";
import { menuTilesForOrdering } from "./Shared/AspNetCoreProps";
import "../project/AspNetCore/areas/Products/pages/OutletPage/OutletPage.scss";

const catalogForm = {
    title: "Outlet products",
    productsApiUrl: "#",
    basketApiUrl: "#",
    basketLabel: "Add to basket",
    inStockLabel: "In stock: ",
    skuLabel: "Sku:",
    primaryFabricLabel: "Fabric: ",
    resultsLabel: "Results",
    displayedRowsLabel: "from",
    pagedItems: {
        data: [
          {
            id: "1",
            url: "#",
            imageUrl: "https://eltap-media-cdn.azureedge.net/api/v1/files/2aca7117-811d-45bf-3cb4-08d907684408?w=710&h=520",
            imageAlt: "image",
            sku: "An",
            title: "Aderito",
            inStock: true,
            availableQuantity: 10,
            brandName: "Eltap",
            brandUrl: "#",
            productAttributes: [
                {
                  key: "width",
                  name: "Szerokość [cm]",
                  values: [
                    216
                  ]
                },
                {
                  key: "primaryFabrics",
                  name: "Tkanina podstawowa",
                  value: "Kronos 19, Kronos 20",
                },
                {
                  key: "secondaryFabrics",
                  name: "Tkanina podstawowa",
                  value: "Kronos 19, Kronos 22",
                },
            ],
          } 
        ],
        total: 1
      }
};

export const OutletProductsPageStory = () => <OutletPage header={header} menuTiles={menuTilesForOrdering} mainNavigation={mainNavigation} footer={footer} catalog={catalogForm} />;

OutletProductsPageStory.story = {
    name: "Available products"
};

const OutletProductStories = {
    title: "AspNetCore.Products",
    component: OutletProductsPageStory
};

export default OutletProductStories;