using Foundation.PageContent.Components.Footers.ViewModels;
using Foundation.PageContent.Components.Headers.ViewModels;
using Foundation.PageContent.MenuTiles.ViewModels;
using Foundation.Extensions.ModelBuilders;
using Foundation.PageContent.ComponentModels;
using System.Threading.Tasks;
using System.Globalization;
using Seller.Web.Areas.TeamMembers.ViewModel;

namespace Seller.Web.Areas.TeamMembers.ModelBuilders
{
    public class TeamMemberPageModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, TeamMemberPageViewModel>
    {
        private readonly IModelBuilder<HeaderViewModel> headerModelBuilder;
        private readonly IModelBuilder<MenuTilesViewModel> menuTilesModelBuilder;
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, TeamMemberFormViewModel> teamMemberFormModelBuilder;
        private readonly IModelBuilder<FooterViewModel> footerModelBuilder;

        public TeamMemberPageModelBuilder(
            IModelBuilder<HeaderViewModel> headerModelBuilder,
            IModelBuilder<MenuTilesViewModel> menuTilesModelBuilder,
            IAsyncComponentModelBuilder<ComponentModelBase, TeamMemberFormViewModel> teamMemberFormModelBuilder,
            IModelBuilder<FooterViewModel> footerModelBuilder)
        {
            this.headerModelBuilder = headerModelBuilder;
            this.menuTilesModelBuilder = menuTilesModelBuilder;
            this.teamMemberFormModelBuilder = teamMemberFormModelBuilder;
            this.footerModelBuilder = footerModelBuilder;
        }

        public async Task<TeamMemberPageViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = new TeamMemberPageViewModel
            {
                Locale = CultureInfo.CurrentUICulture.Name,
                Header = this.headerModelBuilder.BuildModel(),
                MenuTiles = this.menuTilesModelBuilder.BuildModel(),
                TeamMemberForm = await this.teamMemberFormModelBuilder.BuildModelAsync(componentModel),
                Footer = this.footerModelBuilder.BuildModel()
            };

            return viewModel;
        }
    }
}
