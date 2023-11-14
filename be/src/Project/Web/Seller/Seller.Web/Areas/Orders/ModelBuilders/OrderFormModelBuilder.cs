using Foundation.Extensions.ModelBuilders;
using Foundation.Localization;
using Foundation.PageContent.ComponentModels;
using Foundation.PageContent.Components.ListItems.ViewModels;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;
using Seller.Web.Areas.Clients.ViewModels;
using Seller.Web.Areas.Orders.ViewModel;
using Seller.Web.Shared.Repositories.Clients;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Orders.ModelBuilders
{
    public class OrderFormModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, OrderFormViewModel>
    {
        private readonly IStringLocalizer<GlobalResources> globalLocalizer;
        private readonly IStringLocalizer<OrderResources> orderLocalizer;
        private readonly IStringLocalizer<ClientResources> clientLocalizer;
        private readonly LinkGenerator linkGenerator;
        private readonly IClientsRepository clientsRepository;

        public OrderFormModelBuilder(
            IStringLocalizer<GlobalResources> globalLocalizer,
            IStringLocalizer<OrderResources> orderLocalizer,
            IStringLocalizer<ClientResources> clientLocalizer,
            LinkGenerator linkGenerator,
            IClientsRepository clientsRepository)
        {
            this.globalLocalizer = globalLocalizer;
            this.orderLocalizer = orderLocalizer;
            this.linkGenerator = linkGenerator;
            this.clientsRepository = clientsRepository;
            this.clientLocalizer = clientLocalizer;
        }

        public async Task<OrderFormViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = new OrderFormViewModel
            {
                Title = this.orderLocalizer.GetString("EditOrder"),
                AddText = this.orderLocalizer.GetString("AddOrderItem"),
                NoOrderItemsLabel = this.orderLocalizer.GetString("NoOrderItemsLabel"),
                SearchPlaceholderLabel = this.orderLocalizer.GetString("EnterSkuOrName"),
                ChangeDeliveryFromLabel = this.orderLocalizer.GetString("ChangeDeliveryFrom"),
                ChangeDeliveryToLabel = this.orderLocalizer.GetString("ChangeDeliveryTo"),
                DeliveryFromLabel = this.orderLocalizer.GetString("DeliveryFrom"),
                DeliveryToLabel = this.orderLocalizer.GetString("DeliveryTo"),
                GetSuggestionsUrl = this.linkGenerator.GetPathByAction("Get", "ProductsApi", new { Area = "Products", culture = CultureInfo.CurrentUICulture.Name }),
                MoreInfoLabel = this.orderLocalizer.GetString("MoreInfoLabel"),
                NameLabel = this.orderLocalizer.GetString("NameLabel"),
                OrderItemsLabel = this.orderLocalizer.GetString("OrderItemsLabel"),
                QuantityLabel = this.orderLocalizer.GetString("QuantityLabel"),
                OutletQuantityLabel = this.orderLocalizer.GetString("OutletQuantityLabel"),
                ExternalReferenceLabel = this.orderLocalizer.GetString("ExternalReferenceLabel"),
                SkuLabel = this.orderLocalizer.GetString("SkuLabel"),
                GeneralErrorMessage = this.globalLocalizer.GetString("AnErrorOccurred"),
                SaveText = this.orderLocalizer.GetString("PlaceOrder"),
                SaveUrl = this.linkGenerator.GetPathByAction("Index", "ProductsApi", new { Area = "Products", culture = CultureInfo.CurrentUICulture.Name }),
                SelectClientLabel = this.orderLocalizer.GetString("SelectClientLabel"),
                ClientRequiredErrorMessage = this.orderLocalizer.GetString("ClientRequiredErrorMessage"),
                OkLabel = this.globalLocalizer.GetString("Ok"),
                CancelLabel = this.globalLocalizer.GetString("Cancel"),
                UpdateBasketUrl = this.linkGenerator.GetPathByAction("Index", "BasketsApi", new { Area = "Orders", culture = CultureInfo.CurrentUICulture.Name }),
                AreYouSureLabel = this.globalLocalizer.GetString("AreYouSureLabel"),
                DeleteConfirmationLabel = this.globalLocalizer.GetString("DeleteConfirmationLabel"),
                YesLabel = this.globalLocalizer.GetString("Yes"),
                NoLabel = this.globalLocalizer.GetString("No"),
                PlaceOrderUrl = this.linkGenerator.GetPathByAction("Checkout", "BasketCheckoutApi", new { Area = "Orders", culture = CultureInfo.CurrentUICulture.Name }),
                UploadOrderFileUrl = this.linkGenerator.GetPathByAction("Index", "OrderFileApi", new { Area = "Orders", culture = CultureInfo.CurrentUICulture.Name }),
                OrdersUrl = this.linkGenerator.GetPathByAction("Index", "Orders", new { Area = "Orders", culture = CultureInfo.CurrentUICulture.Name }),
                NavigateToOrdersListText = this.orderLocalizer.GetString("NavigateToOrdersList"),
                DropFilesLabel = this.globalLocalizer.GetString("DropFile"),
                DropOrSelectFilesLabel = this.orderLocalizer.GetString("DropOrSelectOrderFile"),
                OrLabel = this.globalLocalizer.GetString("Or"),
                GetDeliveryAddressesUrl = this.linkGenerator.GetPathByAction("Get", "ClientDeliveryAddressesApi", new { Area = "Clients", culture = CultureInfo.CurrentUICulture.Name }),
                DeliveryAddressLabel = this.clientLocalizer.GetString("DeliveryAddress")
            };

            var clients = await this.clientsRepository.GetAllClientsAsync(componentModel.Token, componentModel.Language);

            if (clients is not null)
            {
                viewModel.Clients = clients.Select(x => new ClientListItemViewModel { Id = x.Id , Name = x.Name, DefaultDeliveryAddressId = x.DefaultDeliveryAddressId });
            }

            return viewModel;
        }
    }
}
