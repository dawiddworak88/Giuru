using Foundation.Extensions.ModelBuilders;
using Foundation.GenericRepository.Definitions;
using Foundation.Localization;
using Foundation.PageContent.ComponentModels;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;
using Seller.Web.Areas.Clients.ViewModels;
using Seller.Web.Areas.Orders.Repositories.OrderAttributes;
using Seller.Web.Areas.Orders.ViewModel;
using Seller.Web.Shared.Repositories.Clients;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Orders.ModelBuilders
{
    public class OrderFormModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, OrderFormViewModel>
    {
        private readonly IStringLocalizer<GlobalResources> _globalLocalizer;
        private readonly IStringLocalizer<OrderResources> _orderLocalizer;
        private readonly IStringLocalizer<ClientResources> _clientLocalizer;
        private readonly IClientsRepository _clientsRepository;
        private readonly IOrderAttributesRepository _orderAttributesRepository;
        private readonly LinkGenerator _linkGenerator;

        public OrderFormModelBuilder(
            IStringLocalizer<GlobalResources> globalLocalizer,
            IStringLocalizer<OrderResources> orderLocalizer,
            IStringLocalizer<ClientResources> clientLocalizer,
            IClientsRepository clientsRepository,
            IOrderAttributesRepository orderAttributesRepository,
            LinkGenerator linkGenerator)
        {
            _globalLocalizer = globalLocalizer;
            _orderLocalizer = orderLocalizer;
            _linkGenerator = linkGenerator;
            _clientsRepository = clientsRepository;
            _clientLocalizer = clientLocalizer;
            _orderAttributesRepository = orderAttributesRepository;
        }

        public async Task<OrderFormViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = new OrderFormViewModel
            {
                Title = _orderLocalizer.GetString("EditOrder"),
                AddText = _orderLocalizer.GetString("AddOrderItem"),
                NoOrderItemsLabel = _orderLocalizer.GetString("NoOrderItemsLabel"),
                SearchPlaceholderLabel = _orderLocalizer.GetString("EnterSkuOrName"),
                GetSuggestionsUrl = _linkGenerator.GetPathByAction("Get", "ProductsApi", new { Area = "Products", culture = CultureInfo.CurrentUICulture.Name }),
                MoreInfoLabel = _orderLocalizer.GetString("MoreInfoLabel"),
                NameLabel = _orderLocalizer.GetString("NameLabel"),
                OrderItemsLabel = _orderLocalizer.GetString("OrderItemsLabel"),
                QuantityLabel = _orderLocalizer.GetString("QuantityLabel"),
                OutletQuantityLabel = _orderLocalizer.GetString("OutletQuantityLabel"),
                ExternalReferenceLabel = _orderLocalizer.GetString("ExternalReferenceLabel"),
                SkuLabel = _orderLocalizer.GetString("SkuLabel"),
                GeneralErrorMessage = _globalLocalizer.GetString("AnErrorOccurred"),
                SaveText = _orderLocalizer.GetString("PlaceOrder"),
                SaveUrl = _linkGenerator.GetPathByAction("Index", "ProductsApi", new { Area = "Products", culture = CultureInfo.CurrentUICulture.Name }),
                SelectClientLabel = _orderLocalizer.GetString("SelectClientLabel"),
                ClientRequiredErrorMessage = _orderLocalizer.GetString("ClientRequiredErrorMessage"),
                OkLabel = _globalLocalizer.GetString("Ok"),
                CancelLabel = _globalLocalizer.GetString("Cancel"),
                UpdateBasketUrl = _linkGenerator.GetPathByAction("Index", "BasketsApi", new { Area = "Orders", culture = CultureInfo.CurrentUICulture.Name }),
                AreYouSureLabel = _globalLocalizer.GetString("AreYouSureLabel"),
                DeleteConfirmationLabel = _globalLocalizer.GetString("DeleteConfirmationLabel"),
                YesLabel = _globalLocalizer.GetString("Yes"),
                NoLabel = _globalLocalizer.GetString("No"),
                PlaceOrderUrl = _linkGenerator.GetPathByAction("Checkout", "BasketCheckoutApi", new { Area = "Orders", culture = CultureInfo.CurrentUICulture.Name }),
                UploadOrderFileUrl = _linkGenerator.GetPathByAction("Index", "OrderFileApi", new { Area = "Orders", culture = CultureInfo.CurrentUICulture.Name }),
                OrdersUrl = _linkGenerator.GetPathByAction("Index", "Orders", new { Area = "Orders", culture = CultureInfo.CurrentUICulture.Name }),
                NavigateToOrdersListText = _orderLocalizer.GetString("NavigateToOrdersList"),
                DropFilesLabel = _globalLocalizer.GetString("DropFile"),
                DropOrSelectFilesLabel = _orderLocalizer.GetString("DropOrSelectOrderFile"),
                OrLabel = _globalLocalizer.GetString("Or"),
                GetClientAddressesUrl = _linkGenerator.GetPathByAction("Get", "ClientAddressesApi", new { Area = "Clients", culture = CultureInfo.CurrentUICulture.Name }),
                DeliveryAddressLabel = _clientLocalizer.GetString("DeliveryAddress"),
                BillingAddressLabel = _clientLocalizer.GetString("BillingAddress"),
                DefaultItemsPerPage = Constants.DefaultItemsPerPage
            };

            var clients = await _clientsRepository.GetAllClientsAsync(componentModel.Token, componentModel.Language);

            if (clients is not null)
            {
                viewModel.Clients = clients.Where(x => !x.IsDisabled).Select(x => new ClientListItemViewModel { Id = x.Id , Name = x.Name, DefaultDeliveryAddressId = x.DefaultDeliveryAddressId, DefaultBillingAddressId = x.DefaultBillingAddressId });
            }

            var orderAttributes = await _orderAttributesRepository.GetAsync(componentModel.Token, componentModel.Language, false);

            if (orderAttributes is not null)
            {
                viewModel.OrderAttributes = orderAttributes.Select(x => new OrderAttributeViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Type = x.Type,
                    IsRequired = x.IsRequired,
                    Options = x.Options.Select(x => new OrderAttributeOptionViewModel
                    {
                        Name = x.Name,
                        Value = x.Value
                    })
                });
            }

            return viewModel;
        }
    }
}
