using DocumentFormat.OpenXml.Office2010.Excel;
using Foundation.Extensions.ExtensionMethods;
using Foundation.Extensions.ModelBuilders;
using Foundation.GenericRepository.Definitions;
using Foundation.Localization;
using Foundation.PageContent.ComponentModels;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;
using Seller.Web.Areas.Clients.DomainModels;
using Seller.Web.Areas.Clients.Repositories.ClientTeamMembers;
using Seller.Web.Shared.Catalogs.ModelBuilders;
using Seller.Web.Shared.Repositories.Clients;
using Seller.Web.Shared.ViewModels;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Clients.ModelBuilders
{
    public class ClientTeamMemberPageCatalogModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, CatalogViewModel<TeamMember>>
    {
        private readonly ICatalogModelBuilder _catalogModelBuilder;
        private readonly IStringLocalizer<GlobalResources> _globalLocalizer;
        private readonly IStringLocalizer<TeamMembersResources> _teamMembersLocalizer;
        private readonly LinkGenerator _linkGenerator;
        private readonly IClientsRepository _clientsRepository;
        private readonly IClientTeamMembersRepository _clientTeamMembersRepository;

        public ClientTeamMemberPageCatalogModelBuilder(
            ICatalogModelBuilder catalogModelBuilder,
            IStringLocalizer<GlobalResources> globalLocalizer,
            IStringLocalizer<TeamMembersResources> teamMembersLocalizer,
            IClientsRepository clientsRepository,
            IClientTeamMembersRepository clientTeamMembersRepository,
            LinkGenerator linkGenerator)
        {
            _catalogModelBuilder = catalogModelBuilder;
            _globalLocalizer = globalLocalizer;
            _teamMembersLocalizer = teamMembersLocalizer;
            _clientsRepository = clientsRepository;
            _clientTeamMembersRepository = clientTeamMembersRepository;
            _linkGenerator = linkGenerator;
        }

        public async Task<CatalogViewModel<TeamMember>> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = _catalogModelBuilder.BuildModel<CatalogViewModel<TeamMember>, TeamMember>();

            var title = _teamMembersLocalizer.GetString("ClientTeamMembers").Value;

            Client client = null;

            if (componentModel.Id.HasValue)
            {
                client = await _clientsRepository.GetClientAsync(componentModel.Token, componentModel.Language, componentModel.Id);

                if (client is not null)
                {
                    title = $"{title} - {client.Name}";
                }
            }

            viewModel.Title = title;
            viewModel.DefaultItemsPerPage = Constants.DefaultItemsPerPage;

            viewModel.NewText = _teamMembersLocalizer.GetString("NewClientTeamMemberText");
            viewModel.NewUrl = _linkGenerator.GetPathByAction("New", "ClientTeamMemberDetail", new { Area = "Clients", culture = CultureInfo.CurrentUICulture.Name, organisationId = client.OrganisationId, clientId = client.Id});
            viewModel.EditUrl = _linkGenerator.GetPathByAction("Edit", "ClientTeamMemberDetail", new { Area = "Clients", culture = CultureInfo.CurrentUICulture.Name, clientId = client?.Id, organisationId = componentModel.Id });

            viewModel.DeleteApiUrl = _linkGenerator.GetPathByAction("Delete", "ClientTeamMembersApi", new { Area = "Clients", culture = CultureInfo.CurrentUICulture.Name });
            viewModel.SearchApiUrl = _linkGenerator.GetPathByAction("Get", "ClientTeamMembersApi", new { Area = "Clients", culture = CultureInfo.CurrentUICulture.Name, organisationId = client.OrganisationId });

            viewModel.ConfirmationDialogDeleteNameProperty = [
                nameof(TeamMember.FirstName).ToCamelCase(),
                nameof(TeamMember.LastName).ToCamelCase()
            ];

            viewModel.Table = new CatalogTableViewModel
            {
                Labels = new string[]
                {
                    _globalLocalizer.GetString("FirstName"),
                    _globalLocalizer.GetString("LastName"),
                    _globalLocalizer.GetString("Email")
                },
                Actions = new List<CatalogActionViewModel>
                {
                    new CatalogActionViewModel
                    {
                        IsEdit = true
                    },
                    new CatalogActionViewModel
                    {
                        IsDelete = true
                    }
                },
                Properties = new List<CatalogPropertyViewModel>
                {
                    new CatalogPropertyViewModel
                    {
                        Title = nameof(TeamMember.FirstName).ToCamelCase(),
                        IsDateTime = false
                    },
                    new CatalogPropertyViewModel
                    {
                        Title = nameof(TeamMember.LastName).ToCamelCase(),
                        IsDateTime = false
                    },
                    new CatalogPropertyViewModel
                    {
                        Title = nameof(TeamMember.Email).ToCamelCase(),
                        IsDateTime = false
                    }
                }
            };

            viewModel.PagedItems = await _clientTeamMembersRepository.GetAsync(componentModel.Token, componentModel.Language, client.OrganisationId, null, Constants.DefaultPageIndex, Constants.DefaultItemsPerPage, $"{nameof(TeamMember.Email)} desc");

            return viewModel;
        }
    }
}
