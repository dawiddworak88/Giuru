using Foundation.Extensions.ModelBuilders;
using Foundation.Localization;
using Foundation.PageContent.ComponentModels;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;
using Seller.Web.Areas.Clients.Repositories.Approvals;
using Seller.Web.Areas.Clients.ViewModels;
using System.Globalization;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Clients.ModelBuilders
{
    public class ApprovalFormModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, ApprovalFormViewModel>
    {
        private readonly IApprovalsRepository _approvalsRepository;
        private readonly LinkGenerator _linkGenerator;
        private readonly IStringLocalizer<ClientResources> _clientLocalizer;
        private readonly IStringLocalizer<GlobalResources> _globalLocalizer;

        public ApprovalFormModelBuilder(
            IApprovalsRepository approvalsRepository,
            LinkGenerator linkGenerator,
            IStringLocalizer<ClientResources> clientLocalizer,
            IStringLocalizer<GlobalResources> globalLocalizer)
        {
            _approvalsRepository = approvalsRepository;
            _linkGenerator = linkGenerator;
            _clientLocalizer = clientLocalizer;
            _globalLocalizer = globalLocalizer;
        }

        public async Task<ApprovalFormViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = new ApprovalFormViewModel
            {
                Title = _clientLocalizer.GetString("EditClientApproval"),
                SaveUrl = _linkGenerator.GetPathByAction("index", "ApprovalsApi", new { Area = "Clients", culture = CultureInfo.CurrentCulture.Name }),
                SaveText = _globalLocalizer.GetString("SaveText"),
                ClientApprovalsUrl = _linkGenerator.GetPathByAction("Index", "Approvals", new { Area = "Clients", culture = CultureInfo.CurrentCulture.Name }),
                NavigateToClientApprovals = _clientLocalizer.GetString("NavigateToClientApprovals"),
                IdLabel = _globalLocalizer.GetString("Id"),
                NameLabel = _globalLocalizer.GetString("Name"),
                GeneralErrorMessage = _globalLocalizer.GetString("AnErrorOccurred"),
                FieldRequiredErrorMessage = _globalLocalizer.GetString("FieldRequiredErrorMessage")
            };

            if (componentModel.Id.HasValue)
            {
                var approval = await _approvalsRepository.GetAsync(componentModel.Token, componentModel.Language, componentModel.Id);

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
