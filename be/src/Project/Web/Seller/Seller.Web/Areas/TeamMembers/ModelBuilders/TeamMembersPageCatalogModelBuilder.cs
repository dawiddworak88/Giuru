using Foundation.Extensions.ExtensionMethods;
using Foundation.Extensions.ModelBuilders;
using Foundation.GenericRepository.Definitions;
using Foundation.Localization;
using Foundation.PageContent.ComponentModels;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;
using Seller.Web.Areas.TeamMembers.DomainModels;
using Seller.Web.Areas.TeamMembers.Repositories;
using Seller.Web.Shared.Catalogs.ModelBuilders;
using Seller.Web.Shared.ViewModels;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;

namespace Seller.Web.Areas.TeamMembers.ModelBuilders
{
    public class TeamMembersPageCatalogModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, CatalogViewModel<TeamMember>>
    {
        private readonly ICatalogModelBuilder _catalogModelBuilder;
        private readonly IStringLocalizer<GlobalResources> _globalLocalizer;
        private readonly IStringLocalizer<TeamMembersResources> _teamMemberLocalizer;
        private readonly LinkGenerator _linkGenerator;
        private readonly ITeamMembersRepository _teamMembersRepository;

        public TeamMembersPageCatalogModelBuilder(
            ICatalogModelBuilder catalogModelBuilder,
            IStringLocalizer<GlobalResources> globalLocalizer,
            IStringLocalizer<TeamMembersResources> teamMemberLocalizer,
            ITeamMembersRepository teamMembersRepository,
            LinkGenerator linkGenerator)
        {
            _catalogModelBuilder = catalogModelBuilder;
            _globalLocalizer = globalLocalizer;
            _linkGenerator = linkGenerator;
            _teamMembersRepository = teamMembersRepository;
            _teamMemberLocalizer = teamMemberLocalizer;
        }

        public async Task<CatalogViewModel<TeamMember>> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = _catalogModelBuilder.BuildModel<CatalogViewModel<TeamMember>, TeamMember>();

            viewModel.Title = _globalLocalizer.GetString("TeamMembers");
            viewModel.DefaultItemsPerPage = Constants.DefaultItemsPerPage;

            viewModel.NewText = _teamMemberLocalizer.GetString("NewText");
            viewModel.NewUrl = _linkGenerator.GetPathByAction("Edit", "TeamMember", new { Area = "TeamMembers", culture = CultureInfo.CurrentUICulture.Name });
            viewModel.EditUrl = _linkGenerator.GetPathByAction("Edit", "TeamMember", new { Area = "TeamMembers", culture = CultureInfo.CurrentUICulture.Name });

            viewModel.DeleteApiUrl = _linkGenerator.GetPathByAction("Delete", "TeamMembersApi", new { Area = "TeamMembers", culture = CultureInfo.CurrentUICulture.Name });
            viewModel.SearchApiUrl = _linkGenerator.GetPathByAction("Get", "TeamMembersApi", new { Area = "TeamMembers", culture = CultureInfo.CurrentUICulture.Name });

            viewModel.ConfirmationDialogDeleteNameProperty = new List<string>
            {
                nameof(TeamMember.FirstName).ToCamelCase(),
                nameof(TeamMember.LastName).ToCamelCase()
            };

            viewModel.OrderBy = $"{nameof(TeamMember.Email)} desc";

            viewModel.Table = new CatalogTableViewModel
            {
                Labels = new string[]
                {
                    _globalLocalizer.GetString("FirstName"),
                    _globalLocalizer.GetString("LastName"),
                    _globalLocalizer.GetString("Email"),
                    _globalLocalizer.GetString("Status")
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
                    },
                    new CatalogPropertyViewModel
                    {
                        Title = nameof(TeamMember.IsDisabled).ToCamelCase(),
                        IsActivityTag = true
                    }
                }
            };

            viewModel.PagedItems = await _teamMembersRepository.GetAsync(componentModel.Token, componentModel.Language, null, Constants.DefaultPageIndex, Constants.DefaultItemsPerPage, $"{nameof(TeamMember.Email)} desc");

            return viewModel;
        }
    }
}
