using Foundation.Extensions.ModelBuilders;
using Foundation.Localization;
using Foundation.PageContent.ComponentModels;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;
using Seller.Web.Areas.Clients.Repositories.DiscountCodes;
using Seller.Web.Areas.Clients.ViewModels;
using System.Globalization;
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
                Title = _clientLocalizer.GetString("EditDiscountCode"),
                IdLabel = _globalLocalizer.GetString("Id"),
                CodeLabel = _orderLocalizer.GetString("DiscountCodeLabel"),
                DescriptionLabel = _clientLocalizer.GetString("DiscountCodeDescriptionLabel"),
                ActiveLabel = _globalLocalizer.GetString("Active"),
                InActiveLabel = _globalLocalizer.GetString("InActive"),
                SaveText = _globalLocalizer.GetString("SaveText"),
                GeneralErrorMessage = _globalLocalizer.GetString("AnErrorOccurred"),
                CodeRequiredErrorMessage = _clientLocalizer.GetString("DiscountCodeRequiredErrorMessage"),
                CodeMaxLengthErrorMessage = _clientLocalizer.GetString("DiscountCodeMaxLengthErrorMessage"),
                DescriptionMaxLengthErrorMessage = _clientLocalizer.GetString("DiscountCodeDescriptionMaxLengthErrorMessage"),
                NavigateToDiscountCodesText = _clientLocalizer.GetString("NavigateToDiscountCodesText"),
                SaveUrl = _linkGenerator.GetPathByAction("Index", "DiscountCodesApi", new { Area = "Clients", culture = CultureInfo.CurrentUICulture.Name }),
                DiscountCodesUrl = _linkGenerator.GetPathByAction("Index", "DiscountCodes", new { Area = "Clients", culture = CultureInfo.CurrentUICulture.Name })
            };

            if (componentModel.Id.HasValue)
            {
                var discountCode = await _discountCodesRepository.GetAsync(componentModel.Token, componentModel.Language, componentModel.Id);

                if (discountCode is not null)
                {
                    viewModel.Id = discountCode.Id;
                    viewModel.Code = discountCode.Code;
                    viewModel.Description = discountCode.Description;
                    viewModel.IsDisabled = discountCode.IsDisabled;
                }
            }

            return viewModel;
        }
    }
}
