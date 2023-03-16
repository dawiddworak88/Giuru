import ListOrders from "../project/AspNetCore/areas/Orders/pages/ListOrders/ListOrdersPage";
import { header, mainNavigation, footer } from "./Shared/AspNetCoreProps";
import { menuTilesForOrdering } from "./Shared/AspNetCoreProps";
import "../project/AspNetCore/areas/Orders/pages/ListOrders/ListOrdersPage.scss";

const catalog = {
    title: "Orders",
    newText: "New Order",
    newUrl: "#",
    noLabel: "No",
    yesLabel: "Yes",
    deleteConfirmationLabel: "Delete confirmation",
    areYouSureLabel: "Are you sure you want to delete this item",
    generalErrorMessage: "An Error Occurred",
    searchLabel: "Search",
    editLabel: "Edit",
    deleteLabel: "Delete",
    displayedRowsLabel: "of",
    rowsPerPageLabel: "Rows per Page",
    backIconButtonText: "Previous",
    nextIconButtonText: "Next",
    editUrl: "#",
    deleteUrl: "#",
    noResultsLabel: "There are no results",
    table: {
      actions: [
          {
              isEdit: true
          },
          {
              isDelete: true
          }
      ],
      labels: [
          "Order number",
          "Status",
          "Last modified date",
          "Created date"
      ],
      properties: [
          {
              title: "orderNum",
              isDateTime: false
          },
          {
              title: "status",
              isDateTime: false
          },
          {
              title: "lastModifiedDate",
              isDateTime: true
          },
          {
              title: "createdDate",
              isDateTime: true
          }
      ]
    },
    pagedItems: {
      data: [
        {
          id: "1",
          orderNum: "PIGU-01",
          status: "Canceled",
          lastModifiedDate: new Date(),
          createdDate: new Date(),
        } 
      ],
      total: 1
    }
  };


export const ListOrdersPageStory = () => <ListOrders header={header} menuTiles={menuTilesForOrdering} mainNavigation={mainNavigation} footer={footer} catalog={catalog} />

ListOrdersPageStory.story = {
    name: "List Orders Page"
};

const OrdersStories = {
    title: "AspNetCore.Orders",
    component: ListOrdersPageStory
};

export default OrdersStories;