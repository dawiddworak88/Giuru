using Foundation.Extensions.ModelBuilders;
using Foundation.Localization;
using Foundation.PageContent.ComponentModels;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;
using Seller.Web.Areas.TeamMembers.ViewModel;
using System.Threading.Tasks;

namespace Seller.Web.Areas.TeamMembers.ModelBuilders
{
    public class TeamMemberFormModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, TeamMemberFormViewModel>
    {
        private readonly IStringLocalizer<GlobalResources> globalLocalizer;
        private readonly LinkGenerator linkGenerator;

        public TeamMemberFormModelBuilder(
            IStringLocalizer<GlobalResources> globalLocalizer,
            LinkGenerator linkGenerator)
        {
            this.linkGenerator = linkGenerator;
            this.globalLocalizer = globalLocalizer;
        }

        public async Task<TeamMemberFormViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = new TeamMemberFormViewModel
            {
                GeneralErrorMessage = this.globalLocalizer.GetString("AnErrorOccurred"),
                FieldRequiredErrorMessage = this.globalLocalizer.GetString("FieldRequiredErrorMessage"),
                EmailFormatErrorMessage = this.globalLocalizer.GetString("EmailFormatErrorMessage"),
                FirstNameLabel = this.globalLocalizer.GetString("FirstName"),
                LastNameLabel = this.globalLocalizer.GetString("LastName"),
                EmailLabel = this.globalLocalizer.GetString("Email"),
                SaveText = this.globalLocalizer.GetString("SaveText"),
                NavigateToTeamMembersListText = this.globalLocalizer.GetString("NavigateToTeamMembers")
            };

            return viewModel;
        }
    }
}
