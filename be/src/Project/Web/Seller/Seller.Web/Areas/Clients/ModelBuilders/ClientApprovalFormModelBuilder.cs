using Foundation.Extensions.ModelBuilders;
using Foundation.Localization;
using Foundation.PageContent.ComponentModels;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;
using Seller.Web.Areas.Clients.Repositories.ClientApprovals;
using Seller.Web.Areas.Clients.ViewModels;
using System.Globalization;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Clients.ModelBuilders
{
    public class ClientApprovalFormModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, ClientApprovalFormViewModel>
    {
        private readonly IClientApprovalsRepository _clientApprovalsRepository;
        private readonly LinkGenerator _linkGenerator;
        private readonly IStringLocalizer<ClientResources> _clientLocalizer;
        private readonly IStringLocalizer<GlobalResources> _globalLocalizer;

        public ClientApprovalFormModelBuilder(
            IClientApprovalsRepository clientApprovalsRepository,
            LinkGenerator linkGenerator,
            IStringLocalizer<ClientResources> clientLocalizer,
            IStringLocalizer<GlobalResources> globalLocalizer)
        {
            _clientApprovalsRepository = clientApprovalsRepository;
            _linkGenerator = linkGenerator;
            _clientLocalizer = clientLocalizer;
            _globalLocalizer = globalLocalizer;
        }

        public async Task<ClientApprovalFormViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = new ClientApprovalFormViewModel
            {
                Title = _clientLocalizer.GetString("EditClientApproval"),
                SaveUrl = _linkGenerator.GetPathByAction("index", "ClientApprovalsApi", new { Area = "Clients", culture = CultureInfo.CurrentCulture.Name }),
                SaveText = _globalLocalizer.GetString("SaveText"),
                ClientApprovalsUrl = _linkGenerator.GetPathByAction("Index", "ClientApprovals", new { Area = "Clients", culture = CultureInfo.CurrentCulture.Name }),
                NavigateToClientApprovals = _clientLocalizer.GetString("NavigateToClientApprovals"),
                IdLabel = _globalLocalizer.GetString("Id"),
                NameLabel = _globalLocalizer.GetString("Name"),
                GeneralErrorMessage = _globalLocalizer.GetString("AnErrorOccurred"),
                FieldRequiredErrorMessage = _globalLocalizer.GetString("FieldRequiredErrorMessage")
            };

            if (componentModel.Id.HasValue)
            {
                var approval = await _clientApprovalsRepository.GetAsync(componentModel.Token, componentModel.Language, componentModel.Id);

                if (approval is not null)
                {
                    viewModel.Id = approval.Id;
                    viewModel.Name = approval.Name;
                }
            }

            return viewModel;
        }
    }
}
