using Foundation.Extensions.ModelBuilders;
using Foundation.Localization;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;
using Seller.Web.Areas.Orders.ComponentModels;
using Seller.Web.Areas.Orders.ViewModel;
using System.Globalization;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Orders.ModelBuilders
{
    public class OrderAttributeOptionFormModelBuilder : IAsyncComponentModelBuilder<OrderAttributeOptionComponentModel, OrderAttributeOptionFormViewModel>
    {
        private readonly IStringLocalizer<GlobalResources> _globalLocalizer;
        private readonly IStringLocalizer<OrderResources> _orderLocalizer;
        private readonly LinkGenerator _linkGenerator;

        public OrderAttributeOptionFormModelBuilder(
            IStringLocalizer<GlobalResources> globalLocalizer, 
            IStringLocalizer<OrderResources> orderLocalizer, 
            LinkGenerator linkGenerator)
        {
            _globalLocalizer = globalLocalizer;
            _orderLocalizer = orderLocalizer;
            _linkGenerator = linkGenerator;
        }

        public async Task<OrderAttributeOptionFormViewModel> BuildModelAsync(OrderAttributeOptionComponentModel componentModel)
        {
            var viewModel = new OrderAttributeOptionFormViewModel
            {
                IdLabel = _globalLocalizer.GetString("Id"),
                Title = _orderLocalizer.GetString("EditAttributeOption"),
                SaveText = _globalLocalizer.GetString("SaveText"),
                NameLabel = _globalLocalizer.GetString("Name"),
                ValueLabel = _globalLocalizer.GetString("Value"),
                NavigateToAttributeText = _orderLocalizer.GetString("BackToAttribute"),
                FieldRequiredErrorMessage = _globalLocalizer.GetString("FieldRequiredErrorMessage"),
                GeneralErrorMessage = _globalLocalizer.GetString("AnErrorOccurred"),
                SaveUrl = _linkGenerator.GetPathByAction("Index", "OrderAttributeOptionsApi", new { Area = "Orders", culture = CultureInfo.CurrentUICulture.Name })
            };

            var attributeId = componentModel.OrderAttributeOptionId;


            return viewModel;
        }
    }
}
