using Buyer.Web.Areas.Orders.ViewModel;
using Buyer.Web.Shared.Repositories.Clients;
using Buyer.Web.Shared.Services.Baskets;
using Foundation.Extensions.ModelBuilders;
using Foundation.GenericRepository.Definitions;
using Foundation.Localization;
using Foundation.PageContent.ComponentModels;
using Foundation.PageContent.Components.ListItems.ViewModels;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Buyer.Web.Areas.Orders.ModelBuilders
{
    public class OrderFormModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, OrderFormViewModel>
    {
        private readonly IStringLocalizer<GlobalResources> _globalLocalizer;
        private readonly IStringLocalizer<OrderResources> _orderLocalizer;
        private readonly IStringLocalizer<ClientResources> _clientLocalizer;
        private readonly IClientsRepository _clientsRepository;
        private readonly IClientAddressesRepository _clientAddressesRepository;
        private readonly LinkGenerator _linkGenerator;
        private readonly IBasketService _basketService;

        public OrderFormModelBuilder(
            IStringLocalizer<GlobalResources> globalLocalizer,
            IStringLocalizer<OrderResources> orderLocalizer,
            IStringLocalizer<ClientResources> clientLocalizer,
            IClientsRepository clientsRepository,
            IClientAddressesRepository clientAddressesRepository,
            IBasketService basketService,
            LinkGenerator linkGenerator)
        {
            _globalLocalizer = globalLocalizer;
            _orderLocalizer = orderLocalizer;
            _linkGenerator = linkGenerator;
            _basketService = basketService;
            _clientLocalizer = clientLocalizer;
            _clientsRepository = clientsRepository;
            _clientAddressesRepository = clientAddressesRepository;
        }

        public async Task<OrderFormViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = new OrderFormViewModel
            {
                BasketId = componentModel.BasketId,
                Title = _orderLocalizer.GetString("Order"),
                AddText = _orderLocalizer.GetString("AddOrderItem"),
                SearchPlaceholderLabel = _orderLocalizer.GetString("EnterSkuOrName"),
                GetSuggestionsUrl = _linkGenerator.GetPathByAction("GetProductsQuantities", "ProductsApi", new { Area = "Products", culture = CultureInfo.CurrentUICulture.Name }),
                OrLabel = _globalLocalizer.GetString("Or"),
                DropFilesLabel = _globalLocalizer.GetString("DropFile"),
                DropOrSelectFilesLabel = _orderLocalizer.GetString("DropOrSelectOrderFile"),
                MoreInfoLabel = _orderLocalizer.GetString("MoreInfoLabel"),
                NameLabel = _orderLocalizer.GetString("NameLabel"),
                OrderItemsLabel = _orderLocalizer.GetString("OrderItemsLabel"),
                QuantityLabel = _orderLocalizer.GetString("QuantityLabel"),
                StockQuantityLabel = _orderLocalizer.GetString("StockQuantityLabel"),
                OutletQuantityLabel = _orderLocalizer.GetString("OutletQuantityLabel"),
                InTotalLabel = _orderLocalizer.GetString("InTotalLabel"),
                ExternalReferenceLabel = _orderLocalizer.GetString("ExternalReferenceLabel"),
                SkuLabel = _orderLocalizer.GetString("SkuLabel"),
                GeneralErrorMessage = _globalLocalizer.GetString("AnErrorOccurred"),
                SaveText = _orderLocalizer.GetString("PlaceOrder"),
                DeleteConfirmationLabel = _globalLocalizer.GetString("DeleteConfirmationLabel"),
                YesLabel = _globalLocalizer.GetString("Yes"),
                OkLabel = _globalLocalizer.GetString("Ok"),
                CancelLabel = _globalLocalizer.GetString("Cancel"),
                UpdateBasketUrl = _linkGenerator.GetPathByAction("Index", "BasketsApi", new { Area = "Orders", culture = CultureInfo.CurrentUICulture.Name }),
                PlaceOrderUrl = _linkGenerator.GetPathByAction("Checkout", "BasketCheckoutApi", new { Area = "Orders", culture = CultureInfo.CurrentUICulture.Name }),
                NoLabel = _globalLocalizer.GetString("No"),
                DeleteItemBasketUrl = _linkGenerator.GetPathByAction("DeleteItem", "BasketsApi", new { Area = "Orders", culture = CultureInfo.CurrentUICulture.Name }),
                AreYouSureLabel = _globalLocalizer.GetString("AreYouSureLabel"),
                NavigateToOrdersListText = _orderLocalizer.GetString("NavigateToOrdersList"),
                NoOrderItemsLabel = _orderLocalizer.GetString("NoOrderItemsLabel"),
                ChangeDeliveryFromLabel = _orderLocalizer.GetString("ChangeDeliveryFrom"),
                ChangeDeliveryToLabel = _orderLocalizer.GetString("ChangeDeliveryTo"),
                OrdersUrl = _linkGenerator.GetPathByAction("Index", "Orders", new { Area = "Orders", culture = CultureInfo.CurrentUICulture.Name }),
                UploadOrderFileUrl = _linkGenerator.GetPathByAction("Index", "OrderFileApi", new { Area = "Orders", culture = CultureInfo.CurrentUICulture.Name }),
                ClearBasketText = _orderLocalizer.GetString("ClearBasketText"),
                ClearBasketUrl = _linkGenerator.GetPathByAction("Delete", "BasketsApi", new { Area = "Orders", culture = CultureInfo.CurrentUICulture.Name }),
                CustomOrderLabel = _orderLocalizer.GetString("CustomOrderLabel"),
                InitCustomOrderLabel = _orderLocalizer.GetString("InitCustomOrderLabel"),
                AttachmentsLabel = _globalLocalizer.GetString("Attachments"),
                DeleteLabel = _globalLocalizer.GetString("Delete"),
                DropOrSelectAttachmentsLabel = _globalLocalizer.GetString("DropOrSelectAttachments"),
                SaveMediaUrl = _linkGenerator.GetPathByAction("Post", "FilesApi", new { Area = "Media", culture = CultureInfo.CurrentUICulture.Name }),
                DeliveryAddressLabel = _clientLocalizer.GetString("DeliveryAddress"),
                BillingAddressLabel = _clientLocalizer.GetString("BillingAddress"),
                MaximalLabel = _globalLocalizer.GetString("MaximalLabel"),
                UnitPriceLabel = _globalLocalizer.GetString("UnitPrice"),
                PriceLabel = _globalLocalizer.GetString("Price"),
                CurrencyLabel = _globalLocalizer.GetString("Currency")
            };

            if (componentModel.BasketId.HasValue)
            {
                var basketItems = await _basketService.GetBasketAsync(componentModel.BasketId, componentModel.Token, componentModel.Language);

                if (basketItems is not null)
                {
                    viewModel.BasketItems = basketItems;
                }
            }

            if (componentModel.SellerId.HasValue)
            {
                var client = await _clientsRepository.GetClientAsync(componentModel.Token, componentModel.Language);

                if (client is not null)
                {
                    viewModel.ClientId = client.Id;
                    viewModel.ClientName = client.Name;
                    viewModel.DefaultDeliveryAddressId = client.DefaultDeliveryAddressId;
                    viewModel.DefaultBillingAddressId = client.DefaultBillingAddressId;

                    var deliveryAddresses = await _clientAddressesRepository.GetAsync(componentModel.Token, componentModel.Language, client.Id, null, Constants.DefaultPageIndex, Constants.DefaultItemsPerPage, null);

                    if (deliveryAddresses is not null)
                    {
                        viewModel.ClientAddresses = deliveryAddresses.Data.Select(x => new ListItemViewModel
                        {
                            Id = x.Id,
                            Name = $"{x.Company}, {x.FirstName} {x.LastName}, {x.PostCode} {x.City}"
                        });
                    }
                }
            }

            return viewModel;
        }
    }
}
