using Foundation.Extensions.ModelBuilders;
using Foundation.Localization;
using Foundation.PageContent.ComponentModels;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;
using Seller.Web.Areas.TeamMembers.Repositories;
using Seller.Web.Areas.TeamMembers.ViewModel;
using System.Globalization;
using System.Threading.Tasks;

namespace Seller.Web.Areas.TeamMembers.ModelBuilders
{
    public class TeamMemberFormModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, TeamMemberFormViewModel>
    {
        private readonly IStringLocalizer<GlobalResources> globalLocalizer;
        private readonly IStringLocalizer<TeamMembersResources> teamMembersLocalizer;
        private readonly LinkGenerator linkGenerator;
        private readonly ITeamMembersRepository teamMembersRepository;

        public TeamMemberFormModelBuilder(
            IStringLocalizer<GlobalResources> globalLocalizer,
            IStringLocalizer<TeamMembersResources> teamMembersLocalizer,
            ITeamMembersRepository teamMembersRepository,
            LinkGenerator linkGenerator)
        {
            this.linkGenerator = linkGenerator;
            this.globalLocalizer = globalLocalizer;
            this.teamMembersRepository = teamMembersRepository;
            this.teamMembersLocalizer = teamMembersLocalizer;
        }

        public async Task<TeamMemberFormViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = new TeamMemberFormViewModel
            {
                Title = this.teamMembersLocalizer.GetString("EditTeamMember"),
                GeneralErrorMessage = this.globalLocalizer.GetString("AnErrorOccurred"),
                FieldRequiredErrorMessage = this.globalLocalizer.GetString("FieldRequiredErrorMessage"),
                EmailFormatErrorMessage = this.globalLocalizer.GetString("EmailFormatErrorMessage"),
                FirstNameLabel = this.globalLocalizer.GetString("FirstName"),
                LastNameLabel = this.globalLocalizer.GetString("LastName"),
                EmailLabel = this.globalLocalizer.GetString("Email"),
                SaveUrl = this.linkGenerator.GetPathByAction("Post", "TeamMembersApi", new { Area = "TeamMembers", culture = CultureInfo.CurrentUICulture.Name }),
                SaveText = this.globalLocalizer.GetString("SaveText"),
                NavigateToTeamMembersListText = this.teamMembersLocalizer.GetString("NavigateToTeamMembers"),
                IdLabel = this.globalLocalizer.GetString("Id"),
                TeamMembersUrl = this.linkGenerator.GetPathByAction("Index", "TeamMembers", new { Area = "TeamMembers", culture = CultureInfo.CurrentUICulture.Name })
            };

            if (componentModel.Id.HasValue)
            {
                var teamMember = await this.teamMembersRepository.GetAsync(componentModel.Token, componentModel.Language, componentModel.Id);

                if (teamMember is not null)
                {
                    viewModel.Id = teamMember.Id;
                    viewModel.FirstName = teamMember.FirstName;
                    viewModel.LastName = teamMember.LastName;
                    viewModel.Email = teamMember.Email;
                    viewModel.IsActive = teamMember.IsActive;
                }
            }

            return viewModel;
        }
    }
}
