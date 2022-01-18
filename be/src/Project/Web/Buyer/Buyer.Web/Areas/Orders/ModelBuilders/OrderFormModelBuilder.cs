using Buyer.Web.Areas.Orders.ApiResponseModels;
using Buyer.Web.Areas.Orders.Repositories.Baskets;
using Buyer.Web.Areas.Orders.ViewModel;
using Foundation.Extensions.ExtensionMethods;
using Foundation.Extensions.ModelBuilders;
using Foundation.Localization;
using Foundation.PageContent.ComponentModels;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;
using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Buyer.Web.Areas.Orders.ModelBuilders
{
    public class OrderFormModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, OrderFormViewModel>
    {
        private readonly IStringLocalizer<GlobalResources> globalLocalizer;
        private readonly IStringLocalizer<OrderResources> orderLocalizer;
        private readonly LinkGenerator linkGenerator;
        private readonly IBasketRepository basketRepository;

        public OrderFormModelBuilder(
            IStringLocalizer<GlobalResources> globalLocalizer,
            IStringLocalizer<OrderResources> orderLocalizer,
            IBasketRepository basketRepository,
            LinkGenerator linkGenerator)
        {
            this.globalLocalizer = globalLocalizer;
            this.orderLocalizer = orderLocalizer;
            this.linkGenerator = linkGenerator;
            this.basketRepository = basketRepository;
        }

        public async Task<OrderFormViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = new OrderFormViewModel
            {
                BasketId = componentModel.BasketId,
                Title = this.orderLocalizer.GetString("Order"),
                AddText = this.orderLocalizer.GetString("AddOrderItem"),
                SearchPlaceholderLabel = this.orderLocalizer.GetString("EnterSkuOrName"),
                GetSuggestionsUrl = this.linkGenerator.GetPathByAction("Get", "ProductsApi", new { Area = "Products", culture = CultureInfo.CurrentUICulture.Name }),
                OrLabel = this.globalLocalizer.GetString("Or"),
                DropFilesLabel = this.globalLocalizer.GetString("DropFile"),
                DropOrSelectFilesLabel = this.orderLocalizer.GetString("DropOrSelectOrderFile"),
                DeliveryFromLabel = this.orderLocalizer.GetString("DeliveryFrom"),
                DeliveryToLabel = this.orderLocalizer.GetString("DeliveryTo"),
                MoreInfoLabel = this.orderLocalizer.GetString("MoreInfoLabel"),
                NameLabel = this.orderLocalizer.GetString("NameLabel"),
                OrderItemsLabel = this.orderLocalizer.GetString("OrderItemsLabel"),
                QuantityLabel = this.orderLocalizer.GetString("QuantityLabel"),
                ExternalReferenceLabel = this.orderLocalizer.GetString("ExternalReferenceLabel"),
                SkuLabel = this.orderLocalizer.GetString("SkuLabel"),
                GeneralErrorMessage = this.globalLocalizer.GetString("AnErrorOccurred"),
                SaveText = this.orderLocalizer.GetString("PlaceOrder"),
                DeleteConfirmationLabel = this.globalLocalizer.GetString("DeleteConfirmationLabel"),
                YesLabel = this.globalLocalizer.GetString("Yes"),
                OkLabel = this.globalLocalizer.GetString("Ok"),
                CancelLabel = this.globalLocalizer.GetString("Cancel"),
                UpdateBasketUrl = this.linkGenerator.GetPathByAction("Index", "BasketsApi", new { Area = "Orders", culture = CultureInfo.CurrentUICulture.Name }),
                PlaceOrderUrl = this.linkGenerator.GetPathByAction("Checkout", "BasketCheckoutApi", new { Area = "Orders", culture = CultureInfo.CurrentUICulture.Name }),
                NoLabel = this.globalLocalizer.GetString("No"),
                DeleteItemBasketUrl = this.linkGenerator.GetPathByAction("DeleteItem", "BasketsApi", new { Area = "Orders", culture = CultureInfo.CurrentUICulture.Name }),
                AreYouSureLabel = this.globalLocalizer.GetString("AreYouSureLabel"),
                NavigateToOrdersListText = this.orderLocalizer.GetString("NavigateToOrdersList"),
                NoOrderItemsLabel = this.orderLocalizer.GetString("NoOrderItemsLabel"),
                ChangeDeliveryFromLabel = this.orderLocalizer.GetString("ChangeDeliveryFrom"),
                ChangeDeliveryToLabel = this.orderLocalizer.GetString("ChangeDeliveryTo"),
                OrdersUrl = this.linkGenerator.GetPathByAction("Index", "Orders", new { Area = "Orders", culture = CultureInfo.CurrentUICulture.Name }),
                SaveUrl = this.linkGenerator.GetPathByAction("Index", "ProductsApi", new { Area = "Products", culture = CultureInfo.CurrentUICulture.Name }),
                UploadOrderFileUrl = this.linkGenerator.GetPathByAction("Index", "OrderFileApi", new { Area = "Orders", culture = CultureInfo.CurrentUICulture.Name }),
                ClearBasketText = this.orderLocalizer.GetString("ClearBasketText"),
                ClearBasketUrl = this.linkGenerator.GetPathByAction("Delete", "BasketsApi", new { Area = "Orders", culture = CultureInfo.CurrentUICulture.Name })
            };

            var existingBasket = await this.basketRepository.GetBasketById(componentModel.Token, componentModel.Language, componentModel.BasketId);
            if (existingBasket != null)
            {
                var productIds = existingBasket.Items.OrEmptyIfNull().Select(x => x.ProductId.Value);
                if (productIds.OrEmptyIfNull().Any())
                {
                    var basketResponseModel = existingBasket.Items.OrEmptyIfNull().Select(x => new BasketItemResponseModel
                    {
                        ProductId = x.ProductId,
                        ProductUrl = this.linkGenerator.GetPathByAction("Edit", "Product", new { Area = "Products", culture = CultureInfo.CurrentUICulture.Name, Id = x.ProductId }),
                        Name = x.ProductName,
                        Sku = x.ProductSku,
                        Quantity = x.Quantity,
                        ExternalReference = x.ExternalReference,
                        ImageSrc = x.PictureUrl,
                        ImageAlt = x.ProductName,
                        DeliveryFrom = x.DeliveryFrom,
                        DeliveryTo = x.DeliveryTo,
                        MoreInfo = x.MoreInfo
                    });

                    viewModel.OrderItems = basketResponseModel;
                }
            }

            return viewModel;
        }
    }
}
