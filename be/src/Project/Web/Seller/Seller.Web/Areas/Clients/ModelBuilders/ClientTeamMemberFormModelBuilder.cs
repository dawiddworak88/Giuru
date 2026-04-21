using Foundation.Extensions.ModelBuilders;
using Foundation.Localization;
using Seller.Web.Areas.Clients.ComponentModels;
using Seller.Web.Areas.Clients.Repositories.ClientTeamMembers;
using Seller.Web.Areas.Clients.ViewModels;
using Seller.Web.Shared.Repositories.Clients;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;
using System.Globalization;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Clients.ModelBuilders
{
    public class ClientTeamMemberFormModelBuilder : IAsyncComponentModelBuilder<ClientTeamMemberComponentModel, ClientTeamMemberFormViewModel>
    {
        private readonly IStringLocalizer<GlobalResources> _globalLocalizer;
        private readonly IStringLocalizer<TeamMembersResources> _teamMembersLocalizer;
        private readonly LinkGenerator _linkGenerator;
        private readonly IClientTeamMembersRepository _clientTeamMembersRepository;

        public ClientTeamMemberFormModelBuilder(
            IStringLocalizer<GlobalResources> globalLocalizer,
            IStringLocalizer<TeamMembersResources> teamMembersLocalizer,
            IClientTeamMembersRepository clientTeamMembersRepository,
            LinkGenerator linkGenerator)
        {
            _linkGenerator = linkGenerator;
            _globalLocalizer = globalLocalizer;
            _clientTeamMembersRepository = clientTeamMembersRepository;
            _teamMembersLocalizer = teamMembersLocalizer;
        }

        public async Task<ClientTeamMemberFormViewModel> BuildModelAsync(ClientTeamMemberComponentModel componentModel)
        {
            var viewModel = new ClientTeamMemberFormViewModel
            {

                Title = _teamMembersLocalizer.GetString("EditClientTeamMember"),
                GeneralErrorMessage = _globalLocalizer.GetString("AnErrorOccurred"),
                FieldRequiredErrorMessage = _globalLocalizer.GetString("FieldRequiredErrorMessage"),
                EmailFormatErrorMessage = _globalLocalizer.GetString("EmailFormatErrorMessage"),
                FirstNameLabel = _globalLocalizer.GetString("FirstName"),
                LastNameLabel = _globalLocalizer.GetString("LastName"),
                EmailLabel = _globalLocalizer.GetString("Email"),
                SaveUrl = _linkGenerator.GetPathByAction("Post", "ClientTeamMembersApi", new { Area = "Clients", culture = CultureInfo.CurrentUICulture.Name, organisationId = componentModel.OrganisationId }),
                SaveText = _globalLocalizer.GetString("SaveText"),
                NavigateToClientTeamMembersListText = _teamMembersLocalizer.GetString("NavigateToClientTeamMembers"),
                IdLabel = _globalLocalizer.GetString("Id"),
                ClientTeamMembersUrl = _linkGenerator.GetPathByAction("Edit", "ClientTeamMember", new { Area = "Clients", culture = CultureInfo.CurrentUICulture.Name, id = componentModel.ClientId }),
                ActiveLabel = _globalLocalizer.GetString("Active"),
                InActiveLabel = _globalLocalizer.GetString("InActive")
            };

            if (componentModel.Id.HasValue)
            {
                var teamMember = await _clientTeamMembersRepository.GetAsync(componentModel.Token, componentModel.Language, componentModel.Id);

                if (teamMember is not null)
                {
                    viewModel.Id = teamMember.Id;
                    viewModel.FirstName = teamMember.FirstName;
                    viewModel.LastName = teamMember.LastName;
                    viewModel.Email = teamMember.Email;
                    viewModel.IsDisabled = teamMember.IsDisabled;
                }
            }

            return viewModel;
        }
    }
}
