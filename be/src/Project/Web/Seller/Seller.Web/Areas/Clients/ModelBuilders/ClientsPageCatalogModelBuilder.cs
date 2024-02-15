using Foundation.Extensions.ExtensionMethods;
using Foundation.Extensions.ModelBuilders;
using Foundation.GenericRepository.Definitions;
using Foundation.Localization;
using Foundation.PageContent.ComponentModels;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;
using Seller.Web.Areas.Clients.DomainModels;
using Seller.Web.Shared.Catalogs.ModelBuilders;
using Seller.Web.Shared.Repositories.Clients;
using Seller.Web.Shared.ViewModels;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Clients.ModelBuilders
{
    public class ClientsPageCatalogModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, CatalogViewModel<Client>>
    {
        private readonly ICatalogModelBuilder _catalogModelBuilder;
        private readonly IClientsRepository _clientsRepository;
        private readonly IStringLocalizer _globalLocalizer;
        private readonly IStringLocalizer _clientLocalizer;
        private readonly LinkGenerator _linkGenerator;

        public ClientsPageCatalogModelBuilder(
            ICatalogModelBuilder catalogModelBuilder,
            IClientsRepository clientsRepository,
            IStringLocalizer<GlobalResources> globalLocalizer,
            IStringLocalizer<ClientResources> clientLocalizer,
            LinkGenerator linkGenerator)
        {
            _catalogModelBuilder = catalogModelBuilder;
            _clientsRepository = clientsRepository;
            _globalLocalizer = globalLocalizer;
            _clientLocalizer = clientLocalizer;
            _linkGenerator = linkGenerator;
        }

        public async Task<CatalogViewModel<Client>> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = _catalogModelBuilder.BuildModel<CatalogViewModel<Client>, Client>();

            viewModel.Title = _globalLocalizer.GetString("Clients");
            viewModel.DefaultItemsPerPage = Constants.DefaultItemsPerPage;

            viewModel.NewText = _clientLocalizer.GetString("NewClient");
            viewModel.NewUrl = _linkGenerator.GetPathByAction("Edit", "Client", new { Area = "Clients", culture = CultureInfo.CurrentUICulture.Name });
            viewModel.EditUrl = _linkGenerator.GetPathByAction("Edit", "Client", new { Area = "Clients", culture = CultureInfo.CurrentUICulture.Name });

            viewModel.DeleteApiUrl = _linkGenerator.GetPathByAction("Delete", "ClientsApi", new { Area = "Clients", culture = CultureInfo.CurrentUICulture.Name });
            viewModel.SearchApiUrl = _linkGenerator.GetPathByAction("Get", "ClientsApi", new { Area = "Clients", culture = CultureInfo.CurrentUICulture.Name });

            viewModel.OrderBy = $"{nameof(Client.CreatedDate)} desc";

            viewModel.Table = new CatalogTableViewModel
            {
                Labels = new string[]
                {
                    _globalLocalizer.GetString("Name"),
                    _globalLocalizer.GetString("Email"),
                    _globalLocalizer.GetString("CommunicationLanguage"),
                    _globalLocalizer.GetString("Status"),
                    _globalLocalizer.GetString("LastModifiedDate"),
                    _globalLocalizer.GetString("CreatedDate")
                },
                Actions = new List<CatalogActionViewModel>
                {
                    new CatalogActionViewModel
                    {
                        IsEdit = true
                    }
                },
                Properties = new List<CatalogPropertyViewModel>
                {
                    new CatalogPropertyViewModel
                    {
                        Title = nameof(Client.Name).ToCamelCase(),
                        IsDateTime = false
                    },
                    new CatalogPropertyViewModel
                    {
                        Title = nameof(Client.Email).ToCamelCase(),
                        IsDateTime = false
                    },
                    new CatalogPropertyViewModel
                    {
                        Title = nameof(Client.CommunicationLanguage).ToCamelCase(),
                        IsDateTime = false
                    },
                    new CatalogPropertyViewModel
                    {
                        Title = nameof(Client.IsDisabled).ToCamelCase(),
                        IsActivityTag = true
                    },
                    new CatalogPropertyViewModel
                    {
                        Title = nameof(Client.LastModifiedDate).ToCamelCase(),
                        IsDateTime = true
                    },
                    new CatalogPropertyViewModel
                    {
                        Title = nameof(Client.CreatedDate).ToCamelCase(),
                        IsDateTime = true
                    }
                }
            };

            viewModel.PagedItems = await _clientsRepository.GetClientsAsync(componentModel.Token, componentModel.Language, null, Constants.DefaultPageIndex, Constants.DefaultItemsPerPage, $"{nameof(Client.CreatedDate)} desc");

            return viewModel;
        }
    }
}