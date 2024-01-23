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
    public class ClientAddressFormModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, ClientAddressFormViewModel>
    {
        private readonly IClientsRepository _clientsRepository;
        private readonly ICountriesRepository _countriesRepository;
        private readonly IClientAddressesRepository _clientAddressesRepository;
        private readonly IStringLocalizer<GlobalResources> _globalLocalizer;
        private readonly IStringLocalizer<ClientResources> _clientLocalizer;
        private readonly LinkGenerator _linkGenerator;

        public ClientAddressFormModelBuilder(
            IClientsRepository clientsRepository,
            ICountriesRepository countriesRepository,
            IClientAddressesRepository clientAddressesRepository,
            IStringLocalizer<ClientResources> clientLocalizer,
            IStringLocalizer<GlobalResources> globalLocalizer,
            LinkGenerator linkGenerator)
        {
            _clientsRepository = clientsRepository;
            _countriesRepository = countriesRepository;
            _clientAddressesRepository = clientAddressesRepository;
            _clientLocalizer = clientLocalizer;
            _globalLocalizer = globalLocalizer;
            _linkGenerator = linkGenerator;
        }

        public async Task<ClientAddressFormViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = new ClientAddressFormViewModel
            {
                Title = _globalLocalizer.GetString("EditClientAddress"),
                FieldRequiredErrorMessage = _globalLocalizer.GetString("FieldRequiredErrorMessage"),
                SaveText = _globalLocalizer.GetString("SaveText"),
                SaveUrl = _linkGenerator.GetPathByAction("Index", "ClientAddressesApi", new { Area = "Clients", culture = CultureInfo.CurrentUICulture.Name }),
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
                ClientAddressesUrl = _linkGenerator.GetPathByAction("Index", "ClientAddresses", new { Area = "Clients", culture = CultureInfo.CurrentUICulture.Name }),
                NavigateToClientAddresses = _clientLocalizer.GetString("BackToAddresses")
            };

            var clients = await _clientsRepository.GetAllClientsAsync(componentModel.Token, componentModel.Language);

            if (clients is not null)
            {
                viewModel.Clients = clients.Where(x => x.IsActive).Select(x => new ListItemViewModel
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
                var clientAddress = await _clientAddressesRepository.GetAsync(componentModel.Token, componentModel.Language, componentModel.Id);
            
                if (clientAddress is not null)
                {
                    viewModel.Id = componentModel.Id;
                    viewModel.Company = clientAddress.Company;
                    viewModel.FirstName = clientAddress.FirstName;
                    viewModel.LastName = clientAddress.LastName;
                    viewModel.PhoneNumber = clientAddress.PhoneNumber;
                    viewModel.ClientId = clientAddress.ClientId;
                    viewModel.Street = clientAddress.Street;
                    viewModel.City = clientAddress.City;
                    viewModel.Region = clientAddress.Region;
                    viewModel.PostCode = clientAddress.PostCode;
                    viewModel.CountryId = clientAddress.CountryId;
                }
            }

            return viewModel;
        }
    }
}
