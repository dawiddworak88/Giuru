using Foundation.Extensions.ModelBuilders;
using Foundation.Localization;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;
using Seller.Web.Areas.Orders.ComponentModels;
using Seller.Web.Areas.Orders.Repositories.OrderAttributeOptions;
using Seller.Web.Areas.Orders.ViewModel;
using System.Globalization;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Orders.ModelBuilders
{
    public class OrderAttributeOptionFormModelBuilder : IAsyncComponentModelBuilder<OrderAttributeOptionComponentModel, OrderAttributeOptionFormViewModel>
    {
        private readonly IStringLocalizer<GlobalResources> _globalLocalizer;
        private readonly IStringLocalizer<OrderResources> _orderLocalizer;
        private readonly IOrderAttributeOptionsRepository _orderAttributeOptionsRepository;
        private readonly LinkGenerator _linkGenerator;

        public OrderAttributeOptionFormModelBuilder(
            IStringLocalizer<GlobalResources> globalLocalizer, 
            IStringLocalizer<OrderResources> orderLocalizer,
            IOrderAttributeOptionsRepository orderAttributeOptionsRepository,
            LinkGenerator linkGenerator)
        {
            _globalLocalizer = globalLocalizer;
            _orderLocalizer = orderLocalizer;
            _orderAttributeOptionsRepository = orderAttributeOptionsRepository;
            _linkGenerator = linkGenerator;
        }

        public async Task<OrderAttributeOptionFormViewModel> BuildModelAsync(OrderAttributeOptionComponentModel componentModel)
        {
            var viewModel = new OrderAttributeOptionFormViewModel
            {
                IdLabel = _globalLocalizer.GetString("Id"),
                Title = _orderLocalizer.GetString("EditOrderAttributeOption"),
                SaveText = _globalLocalizer.GetString("SaveText"),
                NameLabel = _globalLocalizer.GetString("Name"),
                ValueLabel = _globalLocalizer.GetString("Value"),
                NavigateToAttributeText = _orderLocalizer.GetString("BackToOrderAttribute"),
                FieldRequiredErrorMessage = _globalLocalizer.GetString("FieldRequiredErrorMessage"),
                GeneralErrorMessage = _globalLocalizer.GetString("AnErrorOccurred"),
                SaveUrl = _linkGenerator.GetPathByAction("Index", "OrderAttributeOptionsApi", new { Area = "Orders", culture = CultureInfo.CurrentUICulture.Name })
            };

            var attributeId = componentModel.OrderAttributeOptionId;

            if (componentModel.Id.HasValue)
            {
                viewModel.Id = componentModel.Id;

                var attributeOption = await _orderAttributeOptionsRepository.GetAsync(componentModel.Token, componentModel.Language, componentModel.Id);

                if (attributeOption is not null)
                {
                    viewModel.Name = attributeOption.Name;
                    viewModel.Value = attributeOption.Value;

                    attributeId = attributeOption.AttributeId;
                }
            }

            viewModel.AttributeId = attributeId;
            viewModel.OrderAttributeUrl = _linkGenerator.GetPathByAction("Edit", "OrderAttribute", new { Id = attributeId, Area = "Orders", culture = CultureInfo.CurrentUICulture.Name });

            return viewModel;
        }
    }
}
