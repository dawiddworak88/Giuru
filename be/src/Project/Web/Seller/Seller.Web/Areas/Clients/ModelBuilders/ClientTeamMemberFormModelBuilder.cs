using Foundation.Extensions.ExtensionMethods;
using Foundation.Extensions.ModelBuilders;
using Foundation.GenericRepository.Definitions;
using Foundation.Localization;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;
using Seller.Web.Areas.Clients.ComponentModels;
using Seller.Web.Areas.Clients.Repositories.Approvals;
using Seller.Web.Areas.Clients.Repositories.ClientTeamMembers;
using Seller.Web.Areas.Clients.ViewModels;
using Seller.Web.Areas.Shared.Repositories.UserApprovals;
using Seller.Web.Shared.Repositories.Clients;
using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Clients.ModelBuilders
{
    public class ClientTeamMemberFormModelBuilder : IAsyncComponentModelBuilder<ClientTeamMemberComponentModel, ClientTeamMemberFormViewModel>
    {
        private readonly IStringLocalizer<GlobalResources> _globalLocalizer;
        private readonly IStringLocalizer<TeamMembersResources> _teamMembersLocalizer;
        private readonly IStringLocalizer<ClientResources> _clientLocalizer;
        private readonly IClientTeamMembersRepository _clientTeamMembersRepository;
        private readonly IApprovalsRepository _approvalsRepository;
        private readonly IUserApprovalsRepository _userApprovalsRepository;
        private readonly LinkGenerator _linkGenerator;

        public ClientTeamMemberFormModelBuilder(
            IStringLocalizer<GlobalResources> globalLocalizer,
            IStringLocalizer<TeamMembersResources> teamMembersLocalizer,
            IStringLocalizer<ClientResources> clientLocalizer,
            IClientTeamMembersRepository clientTeamMembersRepository,
            IApprovalsRepository approvalsRepository,
            IUserApprovalsRepository userApprovalsRepository,
            LinkGenerator linkGenerator)
        {
            _linkGenerator = linkGenerator;
            _globalLocalizer = globalLocalizer;
            _clientLocalizer = clientLocalizer;
            _clientTeamMembersRepository = clientTeamMembersRepository;
            _teamMembersLocalizer = teamMembersLocalizer;
            _userApprovalsRepository = userApprovalsRepository;
            _approvalsRepository = approvalsRepository;
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
                InActiveLabel = _globalLocalizer.GetString("InActive"),
                ExpressedOnLabel = _clientLocalizer.GetString("ExpressedOnLabel")
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

                var approvals = await _approvalsRepository.GetAsync(
                    token: componentModel.Token, 
                    language: componentModel.Language, 
                    pageIndex: Constants.DefaultPageIndex, 
                    itemsPerPage: Constants.DefaultItemsPerPage,
                    searchTerm: null,
                    orderBy: null);

                if (approvals is not null)
                {
                    var userApprovals = await _userApprovalsRepository.GetAsync(
                        componentModel.Token,
                        componentModel.Language,
                        componentModel.Id);

                    viewModel.TeamMemberApprovals = approvals.Data.OrEmptyIfNull().Select(x =>
                    {
                        var userApproval = userApprovals.FirstOrDefault(y => y.ApprovalId == x.Id);

                        return new ApprovalViewModel
                        {
                            Id = x.Id,
                            Name = x.Name,
                            ApprovalDate = userApproval is not null ? userApproval.CreatedDate : null,
                            IsApproved = userApproval is not null
                        };
                    });
                }

            }

            return viewModel;
        }
    }
}
