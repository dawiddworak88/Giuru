using Buyer.Web.Shared.DomainModels.Baskets;
using System;
using System.Collections.Generic;

namespace Buyer.Web.Areas.Orders.ViewModel
{
    public class OrderFormViewModel
    {
        public string Title { get; set; }
        public Guid? BasketId { get; set; }
        public string NoOrderItemsLabel { get; set; }
        public string SearchPlaceholderLabel { get; set; }
        public string GeneralErrorMessage { get; set; }
        public string SaveText { get; set; }
        public string SaveUrl { get; set; }
        public string SkuLabel { get; set; }
        public string NameLabel { get; set; }
        public string QuantityLabel { get; set; }
        public string ExternalReferenceLabel { get; set; }
        public string DeliveryFromLabel { get; set; }
        public string DeliveryToLabel { get; set; }
        public string MoreInfoLabel { get; set; }
        public string GetSuggestionsUrl { get; set; }
        public string OrderItemsLabel { get; set; }
        public string ChangeDeliveryFromLabel { get; set; }
        public string ChangeDeliveryToLabel { get; set; }
        public string AddText { get; set; }
        public string OkLabel { get; set; }
        public string CancelLabel { get; set; }
        public string UpdateBasketUrl { get; set; }
        public string DeleteConfirmationLabel { get; set; }
        public string AreYouSureLabel { get; set; }
        public string YesLabel { get; set; }
        public string NoLabel { get; set; }
        public string ClearBasketText { get; set; }
        public string ClearBasketUrl { get; set; }
        public IEnumerable<Basket> OrderItems { get; set; }
        public string DeleteItemBasketUrl { get; set; }
        public string PlaceOrderUrl { get; set; }
        public string OrdersUrl { get; set; }
        public string NavigateToOrdersListText { get; set; }
        public string UploadOrderFileUrl { get; set; }
        public string OrLabel { get; set; }
        public string DropOrSelectFilesLabel { get; set; }
        public string DropFilesLabel { get; set; }
    }
}
