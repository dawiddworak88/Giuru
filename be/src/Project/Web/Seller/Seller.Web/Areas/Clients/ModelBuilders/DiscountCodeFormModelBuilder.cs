using Foundation.Extensions.ModelBuilders;
using Foundation.Localization;
using Foundation.PageContent.ComponentModels;
using Foundation.PageContent.Components.ListItems.ViewModels;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;
using Seller.Web.Areas.Clients.Repositories.DiscountCodes;
using Seller.Web.Areas.Clients.ViewModels;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Clients.ModelBuilders
{
    public class DiscountCodeFormModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, DiscountCodeFormViewModel>
    {
        private readonly IStringLocalizer<GlobalResources> _globalLocalizer;
        private readonly IStringLocalizer<ClientResources> _clientLocalizer;
        private readonly IStringLocalizer<OrderResources> _orderLocalizer;
        private readonly LinkGenerator _linkGenerator;
        private readonly IDiscountCodesRepository _discountCodesRepository;

        public DiscountCodeFormModelBuilder(
            IStringLocalizer<GlobalResources> globalLocalizer,
            IStringLocalizer<ClientResources> clientLocalizer,
            IStringLocalizer<OrderResources> orderLocalizer,
            IDiscountCodesRepository discountCodesRepository,
            LinkGenerator linkGenerator)
        {
            _globalLocalizer = globalLocalizer;
            _clientLocalizer = clientLocalizer;
            _orderLocalizer = orderLocalizer;
            _linkGenerator = linkGenerator;
            _discountCodesRepository = discountCodesRepository;
        }

        public async Task<DiscountCodeFormViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = new DiscountCodeFormViewModel
            {
                Title = componentModel.Id.HasValue ? _clientLocalizer.GetString("EditDiscountCode") : _clientLocalizer.GetString("NewDiscountCode"),
                CodeLabel = _orderLocalizer.GetString("DiscountCodeLabel"),
                CodeRequiredErrorMessage = _clientLocalizer.GetString("DiscountCodeRequiredErrorMessage"),
                DescriptionLabel = _clientLocalizer.GetString("DiscountCodeDescriptionLabel"),
                ClientsLabel = _clientLocalizer.GetString("DiscountCodeClientsLabel"),
                NoClientsHint = _clientLocalizer.GetString("DiscountCodeNoClientsHint"),
                GrulaHint = _clientLocalizer.GetString("DiscountCodeGrulaHint"),
                ActiveLabel = _globalLocalizer.GetString("Active"),
                InActiveLabel = _globalLocalizer.GetString("InActive"),
                IdLabel = _globalLocalizer.GetString("Id"),
                SaveText = _globalLocalizer.GetString("SaveText"),
                SaveUrl = _linkGenerator.GetPathByAction("Index", "DiscountCodesApi", new { Area = "Clients", culture = CultureInfo.CurrentUICulture.Name }),
                GeneralErrorMessage = _globalLocalizer.GetString("AnErrorOccurred"),
                NavigateToDiscountCodes = _clientLocalizer.GetString("NavigateToDiscountCodesText"),
                DiscountCodesUrl = _linkGenerator.GetPathByAction("Index", "DiscountCodes", new { Area = "Clients", culture = CultureInfo.CurrentUICulture.Name }),
                ClientIds = new List<System.Guid>()
            };

            if (componentModel.Id.HasValue)
            {
                var discountCode = await _discountCodesRepository.GetAsync(componentModel.Token, componentModel.Language, componentModel.Id);

                if (discountCode is not null)
                {
                    viewModel.Id = discountCode.Id;
                    viewModel.Version = discountCode.Version;
                    viewModel.Code = discountCode.Code;
                    viewModel.Description = discountCode.Description;
                    viewModel.IsDisabled = discountCode.IsDisabled;
                    viewModel.ClientIds = discountCode.ClientIds?.ToList() ?? new List<System.Guid>();
                }
            }

            // A failed, malformed or incomplete list throws here, so the form never renders empty options that a save
            // would turn into "unassign everybody".
            var clients = await _discountCodesRepository.GetClientOptionsAsync(componentModel.Token, componentModel.Language);

            viewModel.Clients = clients.Select(x => new ListItemViewModel { Id = x.Id, Name = x.Name }).ToList();

            return viewModel;
        }
    }
}
