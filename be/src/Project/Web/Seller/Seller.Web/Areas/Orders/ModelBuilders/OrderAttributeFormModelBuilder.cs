using Foundation.Extensions.ModelBuilders;
using Foundation.Localization;
using Foundation.PageContent.ComponentModels;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;
using Seller.Web.Areas.Orders.ViewModel;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Orders.ModelBuilders
{
    public class OrderAttributeFormModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, OrderAttributeFormViewModel>
    {
        private readonly IStringLocalizer<GlobalResources> _globalLocalizer;
        private readonly IStringLocalizer<OrderResources> _orderLocalizer;
        private readonly LinkGenerator _linkGenerator;

        public OrderAttributeFormModelBuilder(
            IStringLocalizer<GlobalResources> globalLocalizer,
            IStringLocalizer<OrderResources> orderLocalizer,
            LinkGenerator linkGenerator)
        {
            _globalLocalizer = globalLocalizer;
            _linkGenerator = linkGenerator;
            _orderLocalizer = orderLocalizer;
        }

        public async Task<OrderAttributeFormViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = new OrderAttributeFormViewModel
            {
                IdLabel = _globalLocalizer.GetString("Id"),
                Title = _orderLocalizer.GetString("EditAttribute"),
                SaveText = _globalLocalizer.GetString("SaveText"),
                NameLabel = _globalLocalizer.GetString("Name"),
                TypeLabel = _globalLocalizer.GetString("Type"),
                NavigateToAttributesText = _orderLocalizer.GetString("BackToAttributes"),
                FieldRequiredErrorMessage = _globalLocalizer.GetString("FieldRequiredErrorMessage"),
                GeneralErrorMessage = _globalLocalizer.GetString("AnErrorOccurred"),
                OrderAttributesUrl = _linkGenerator.GetPathByAction("Index", "OrderAttributes", new { Area = "Orders", culture = CultureInfo.CurrentUICulture.Name }),
                SaveUrl = _linkGenerator.GetPathByAction("Index", "OrderAttributesApi", new { Area = "Orders", culture = CultureInfo.CurrentUICulture.Name }),
                EditUrl = _linkGenerator.GetPathByAction("Edit", "OrderAttribute", new { Area = "Orders", culture = CultureInfo.CurrentUICulture.Name }),
                //Types = new List<ClientFieldTypeViewModel>()
            };

            if (componentModel.Id.HasValue)
            {
                viewModel.Id = componentModel.Id;
            }

            return viewModel;
        }
    }
}
