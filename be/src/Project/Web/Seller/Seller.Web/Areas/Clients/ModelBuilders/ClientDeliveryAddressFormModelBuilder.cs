using DocumentFormat.OpenXml.Spreadsheet;
using Foundation.Extensions.ExtensionMethods;
using Foundation.Extensions.ModelBuilders;
using Foundation.Localization;
using Foundation.PageContent.ComponentModels;
using Foundation.PageContent.Components.ListItems.ViewModels;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;
using Seller.Web.Areas.Clients.Repositories.DeliveryAddresses;
using Seller.Web.Areas.Clients.ViewModels;
using Seller.Web.Areas.Global.DomainModels;
using Seller.Web.Areas.Global.Repositories;
using Seller.Web.Shared.Repositories.Clients;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Clients.ModelBuilders
{
    public class ClientDeliveryAddressFormModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, ClientDeliveryAddressFormViewModel>
    {
        private readonly IClientsRepository _clientsRepository;
        private readonly ICountriesRepository _countriesRepository;
        private readonly IClientDeliveryAddressesRepository _clientDeliveryAddressesRepository;
        private readonly IStringLocalizer<GlobalResources> _globalLocalizer;
        private readonly IStringLocalizer<ClientResources> _clientLocalizer;
        private readonly LinkGenerator _linkGenerator;

        public ClientDeliveryAddressFormModelBuilder(
            IClientsRepository clientsRepository,
            ICountriesRepository countriesRepository,
            IClientDeliveryAddressesRepository clientDeliveryAddressesRepository,
            IStringLocalizer<ClientResources> clientLocalizer,
            IStringLocalizer<GlobalResources> globalLocalizer,
            LinkGenerator linkGenerator)
        {
            _clientsRepository = clientsRepository;
            _countriesRepository = countriesRepository;
            _clientDeliveryAddressesRepository = clientDeliveryAddressesRepository;
            _clientLocalizer = clientLocalizer;
            _globalLocalizer = globalLocalizer;
            _linkGenerator = linkGenerator;
        }

        public async Task<ClientDeliveryAddressFormViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = new ClientDeliveryAddressFormViewModel
            {
                Title = _clientLocalizer.GetString("EditDeliveryAddress"),
                FieldRequiredErrorMessage = _globalLocalizer.GetString("FieldRequiredErrorMessage"),
                SaveText = _globalLocalizer.GetString("SaveText"),
                SaveUrl = _linkGenerator.GetPathByAction("Index", "ClientDeliveryAddressesApi", new { Area = "Clients", culture = CultureInfo.CurrentUICulture.Name }),
                GeneralErrorMessage = _globalLocalizer.GetString("AnErrorOccurred"),
                IdLabel = _globalLocalizer.GetString("Id"),
                ClientLabel = _globalLocalizer.GetString("Client"),
                CityLabel = _globalLocalizer.GetString("City"),
                RegionLabel = _globalLocalizer.GetString("Region"),
                PostCodeLabel = _globalLocalizer.GetString("PostCode"),
                CompanyLabel = _globalLocalizer.GetString("CompanyName"),
                FirstNameLabel = _globalLocalizer.GetString("FirstName"),
                LastNameLabel = _globalLocalizer.GetString("LastName"),
                CountryLabel = _globalLocalizer.GetString("Country"),
                PhoneNumberLabel = _globalLocalizer.GetString("PhoneNumberLabel"),
                StreetLabel = _globalLocalizer.GetString("Street"),
                DeliveryAddressesUrl = _linkGenerator.GetPathByAction("Index", "ClientDeliveryAddresses", new { Area = "Clients", culture = CultureInfo.CurrentUICulture.Name }),
                NavigateToClientDeliveryAddresses = _clientLocalizer.GetString("BackToDeliveryAddresses")
            };

            var clients = await _clientsRepository.GetAllClientsAsync(componentModel.Token, componentModel.Language);

            if (clients is not null)
            {
                viewModel.Clients = clients.Select(x => new ListItemViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                });
            }

            var countries = await _countriesRepository.GetAsync(componentModel.Token, componentModel.Language, $"{nameof(Country.Name)} asc");

            if (countries is not null)
            {
                viewModel.Countries = countries.Select(x => new ListItemViewModel
                {
                    Id = x.Id,
                    Name = x.Name
                });
            }

            if (componentModel.Id.HasValue)
            {
                var deliveryAddress = await _clientDeliveryAddressesRepository.GetAsync(componentModel.Token, componentModel.Language, componentModel.Id);
            
                if (deliveryAddress is not null)
                {
                    viewModel.Id = componentModel.Id;
                    viewModel.Company = deliveryAddress.Company;
                    viewModel.FirstName = deliveryAddress.FirstName;
                    viewModel.LastName = deliveryAddress.LastName;
                    viewModel.PhoneNumber = deliveryAddress.PhoneNumber;
                    viewModel.ClientId = deliveryAddress.ClientId;
                    viewModel.Street = deliveryAddress.Street;
                    viewModel.City = deliveryAddress.City;
                    viewModel.Region = deliveryAddress.Region;
                    viewModel.PostCode = deliveryAddress.PostCode;
                    viewModel.CountryId = deliveryAddress.CountryId;
                }
            }

            return viewModel;
        }
    }
}
