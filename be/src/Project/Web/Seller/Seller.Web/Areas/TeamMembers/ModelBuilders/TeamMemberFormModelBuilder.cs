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
        private readonly IStringLocalizer<GlobalResources> _globalLocalizer;
        private readonly IStringLocalizer<TeamMembersResources> _teamMembersLocalizer;
        private readonly LinkGenerator _linkGenerator;
        private readonly ITeamMembersRepository _teamMembersRepository;

        public TeamMemberFormModelBuilder(
            IStringLocalizer<GlobalResources> globalLocalizer,
            IStringLocalizer<TeamMembersResources> teamMembersLocalizer,
            ITeamMembersRepository teamMembersRepository,
            LinkGenerator linkGenerator)
        {
            _linkGenerator = linkGenerator;
            _globalLocalizer = globalLocalizer;
            _teamMembersRepository = teamMembersRepository;
            _teamMembersLocalizer = teamMembersLocalizer;
        }

        public async Task<TeamMemberFormViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = new TeamMemberFormViewModel
            {
                Title = _teamMembersLocalizer.GetString("EditTeamMember"),
                GeneralErrorMessage = _globalLocalizer.GetString("AnErrorOccurred"),
                FieldRequiredErrorMessage = _globalLocalizer.GetString("FieldRequiredErrorMessage"),
                EmailFormatErrorMessage = _globalLocalizer.GetString("EmailFormatErrorMessage"),
                FirstNameLabel = _globalLocalizer.GetString("FirstName"),
                LastNameLabel = _globalLocalizer.GetString("LastName"),
                EmailLabel = _globalLocalizer.GetString("Email"),
                SaveUrl = _linkGenerator.GetPathByAction("Post", "TeamMembersApi", new { Area = "TeamMembers", culture = CultureInfo.CurrentUICulture.Name }),
                SaveText = _globalLocalizer.GetString("SaveText"),
                NavigateToTeamMembersListText = _teamMembersLocalizer.GetString("NavigateToTeamMembers"),
                IdLabel = _globalLocalizer.GetString("Id"),
                TeamMembersUrl = _linkGenerator.GetPathByAction("Index", "TeamMembers", new { Area = "TeamMembers", culture = CultureInfo.CurrentUICulture.Name })
            };

            if (componentModel.Id.HasValue)
            {
                var teamMember = await _teamMembersRepository.GetAsync(componentModel.Token, componentModel.Language, componentModel.Id);

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
