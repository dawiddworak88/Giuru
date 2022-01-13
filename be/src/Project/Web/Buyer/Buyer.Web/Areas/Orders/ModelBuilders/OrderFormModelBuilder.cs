using Buyer.Web.Areas.Orders.Repositories.Baskets;
using Buyer.Web.Areas.Orders.ViewModel;
using Foundation.Extensions.ModelBuilders;
using Foundation.Localization;
using Foundation.PageContent.ComponentModels;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;
using System.Globalization;
using System.Threading.Tasks;

namespace Buyer.Web.Areas.Orders.ModelBuilders
{
    public class OrderFormModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, OrderFormViewModel>
    {
        private readonly IStringLocalizer<GlobalResources> globalLocalizer;
        private readonly IStringLocalizer<OrderResources> orderLocalizer;
        private readonly LinkGenerator linkGenerator;
        private readonly IBasketRepository basketRepositry;

        public OrderFormModelBuilder(
            IStringLocalizer<GlobalResources> globalLocalizer,
            IStringLocalizer<OrderResources> orderLocalizer,
            IBasketRepository basketRepositry,
            LinkGenerator linkGenerator)
        {
            this.globalLocalizer = globalLocalizer;
            this.orderLocalizer = orderLocalizer;
            this.linkGenerator = linkGenerator;
            this.basketRepositry = basketRepositry;
        }

        public async Task<OrderFormViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = new OrderFormViewModel
            {
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

            var existingBasket = await this.basketRepositry.GetBasketByOrganisation(componentModel.Token, componentModel.Language);
            if (existingBasket != null)
            {
                viewModel.BasketId = existingBasket.Id;
                viewModel.OrderItems = existingBasket.Items;
            }

            return viewModel;
        }
    }
}
