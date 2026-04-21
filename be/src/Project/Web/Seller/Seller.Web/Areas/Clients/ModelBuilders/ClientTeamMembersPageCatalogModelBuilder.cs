using Foundation.Extensions.ExtensionMethods;
using Foundation.Extensions.ModelBuilders;
using Foundation.GenericRepository.Definitions;
using Foundation.Localization;
using Foundation.PageContent.ComponentModels;
using Seller.Web.Areas.Clients.ComponentModels;
using Seller.Web.Areas.Clients.DomainModels;
using Seller.Web.Shared.Catalogs.ModelBuilders;
using Seller.Web.Shared.Repositories.Clients;
using Seller.Web.Shared.ViewModels;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Foundation.GenericRepository.Paginations;
using System.Linq;

namespace Seller.Web.Areas.Clients.ModelBuilders
{
    public class ClientTeamMembersPageCatalogModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, CatalogViewModel<ClientTeamMember>>
    {
        private readonly ICatalogModelBuilder _catalogModelBuilder;
        private readonly IStringLocalizer<GlobalResources> _globalLocalizer;
        private readonly IStringLocalizer<TeamMembersResources> _teamMembersLocalizer;
        private readonly LinkGenerator _linkGenerator;
        private readonly IClientsRepository _clientsRepository;

        public ClientTeamMembersPageCatalogModelBuilder(
            ICatalogModelBuilder catalogModelBuilder,
            IStringLocalizer<GlobalResources> globalLocalizer,
            IStringLocalizer<TeamMembersResources> teamMembersLocalizer,
            IClientsRepository clientsRepository,
            LinkGenerator linkGenerator)
        {
            _catalogModelBuilder = catalogModelBuilder;
            _globalLocalizer = globalLocalizer;
            _teamMembersLocalizer = teamMembersLocalizer;
            _linkGenerator = linkGenerator;
            _clientsRepository = clientsRepository;
        }

        public async Task<CatalogViewModel<ClientTeamMember>> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = _catalogModelBuilder.BuildModel<CatalogViewModel<ClientTeamMember>, ClientTeamMember>();

            viewModel.Title = _teamMembersLocalizer.GetString("ClientTeamMembers");
            viewModel.DefaultItemsPerPage = Constants.DefaultItemsPerPage;

            viewModel.EditUrl = _linkGenerator.GetPathByAction("Edit", "ClientTeamMember", new { Area = "Clients", culture = CultureInfo.CurrentUICulture.Name });

            viewModel.SearchApiUrl = _linkGenerator.GetPathByAction("Get", "ClientsApi", new { Area = "Clients", culture = CultureInfo.CurrentUICulture.Name });

            viewModel.OrderBy = $"{nameof(ClientTeamMember.Name)} asc";

            viewModel.Table = new CatalogTableViewModel
            {
                Labels = new string[]
                {
                    _globalLocalizer.GetString("Name"),
                    _globalLocalizer.GetString("Email"),
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
                        Title = nameof(ClientTeamMember.Name).ToCamelCase(),
                        IsDateTime = false
                    },
                    new CatalogPropertyViewModel
                    {
                        Title = nameof(ClientTeamMember.Email).ToCamelCase(),
                        IsDateTime = false
                    },
                    new CatalogPropertyViewModel
                    {
                        Title = nameof(ClientTeamMember.LastModifiedDate).ToCamelCase(),
                        IsDateTime = true
                    },
                    new CatalogPropertyViewModel
                    {
                        Title = nameof(ClientTeamMember.CreatedDate).ToCamelCase(),
                        IsDateTime = true
                    }
                }
            };

            var clients = await _clientsRepository.GetClientsAsync(componentModel.Token, componentModel.Language, null, Constants.DefaultPageIndex, Constants.DefaultItemsPerPage, $"{nameof(ClientTeamMember.CreatedDate)} desc");

            if (clients?.Data is not null)
            {
                viewModel.PagedItems = new PagedResults<IEnumerable<ClientTeamMember>>(clients.Total, clients.PageSize)
                {
                    Data = clients.Data.Select(c => new ClientTeamMember
                    {
                        Id = c.Id,
                        Name = c.Name,
                        Email = c.Email,
                        CreatedDate = c.CreatedDate,
                        LastModifiedDate = c.LastModifiedDate
                    })
                };
            }

            return viewModel;
        }
    }
}
