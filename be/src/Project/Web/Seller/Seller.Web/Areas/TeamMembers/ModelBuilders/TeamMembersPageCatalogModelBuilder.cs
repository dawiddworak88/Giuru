using Foundation.Extensions.ExtensionMethods;
using Foundation.Extensions.ModelBuilders;
using Foundation.GenericRepository.Definitions;
using Foundation.Localization;
using Foundation.PageContent.ComponentModels;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;
using Seller.Web.Areas.TeamMembers.DomainModels;
using Seller.Web.Shared.Catalogs.ModelBuilders;
using Seller.Web.Shared.ViewModels;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;

namespace Seller.Web.Areas.TeamMembers.ModelBuilders
{
    public class TeamMembersPageCatalogModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, CatalogViewModel<TeamMember>>
    {
        private readonly ICatalogModelBuilder catalogModelBuilder;
        private readonly IStringLocalizer<GlobalResources> globalLocalizer;
        private readonly LinkGenerator linkGenerator;

        public TeamMembersPageCatalogModelBuilder(
            ICatalogModelBuilder catalogModelBuilder,
            IStringLocalizer<GlobalResources> globalLocalizer,
            IStringLocalizer<NewsResources> newsLocalizer,
            LinkGenerator linkGenerator)
        {
            this.catalogModelBuilder = catalogModelBuilder;
            this.globalLocalizer = globalLocalizer;
            this.linkGenerator = linkGenerator;
        }

        public async Task<CatalogViewModel<TeamMember>> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = this.catalogModelBuilder.BuildModel<CatalogViewModel<TeamMember>, TeamMember>();

            viewModel.Title = this.globalLocalizer.GetString("TeamMembers");
            viewModel.NewText = this.globalLocalizer.GetString("NewText");
            viewModel.NewUrl = this.linkGenerator.GetPathByAction("Edit", "TeamMember", new { Area = "TeamMembers", culture = CultureInfo.CurrentUICulture.Name });
            viewModel.EditUrl = this.linkGenerator.GetPathByAction("Edit", "TeamMember", new { Area = "TeamMembers", culture = CultureInfo.CurrentUICulture.Name });

            viewModel.DeleteApiUrl = this.linkGenerator.GetPathByAction("Delete", "TeamMembersApi", new { Area = "TeamMembers", culture = CultureInfo.CurrentUICulture.Name });
            viewModel.SearchApiUrl = this.linkGenerator.GetPathByAction("Get", "TeamMembersApi", new { Area = "TeamMembers", culture = CultureInfo.CurrentUICulture.Name });

            viewModel.OrderBy = $"{nameof(TeamMember.CreatedDate)} desc";

            viewModel.Table = new CatalogTableViewModel
            {
                Labels = new string[]
                {
                    this.globalLocalizer.GetString("FirstName"),
                    this.globalLocalizer.GetString("LastName"),
                    this.globalLocalizer.GetString("Email"),
                    this.globalLocalizer.GetString("LastModifiedDate"),
                    this.globalLocalizer.GetString("CreatedDate")
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
                    },
                    new CatalogPropertyViewModel
                    {
                        Title = nameof(TeamMember.LastModifiedDate).ToCamelCase(),
                        IsDateTime = true
                    },
                    new CatalogPropertyViewModel
                    {
                        Title = nameof(TeamMember.CreatedDate).ToCamelCase(),
                        IsDateTime = true
                    }
                }
            };

            return viewModel;
        }
    }
}
