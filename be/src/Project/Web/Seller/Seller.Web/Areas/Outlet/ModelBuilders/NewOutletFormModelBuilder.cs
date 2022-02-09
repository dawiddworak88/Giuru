using Foundation.Extensions.ModelBuilders;
using Foundation.Localization;
using Foundation.PageContent.ComponentModels;
using Foundation.PageContent.Components.ListItems.ViewModels;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;
using Seller.Web.Areas.Outlet.ViewModel;
using Seller.Web.Areas.Shared.Repositories.Products;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Outlet.ModelBuilders
{
    public class NewOutletFormModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, NewOutletFormViewModel>
    {
        private readonly IStringLocalizer<GlobalResources> globalLocalizer;
        private readonly IStringLocalizer<OutletResources> outletLocalizer;
        private readonly LinkGenerator linkGenerator;
        private readonly IProductsRepository productsRepository;

        public NewOutletFormModelBuilder(
            IStringLocalizer<GlobalResources> globalLocalizer,
            IStringLocalizer<OutletResources> outletLocalizer,
            LinkGenerator linkGenerator,
            IProductsRepository productsRepository)
        {
            this.globalLocalizer = globalLocalizer;
            this.outletLocalizer = outletLocalizer;
            this.linkGenerator = linkGenerator;
            this.productsRepository = productsRepository;
        }

        public async Task<NewOutletFormViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = new NewOutletFormViewModel
            {
                Title = this.globalLocalizer.GetString("Outlet"),
                SelectProductLabel = this.globalLocalizer.GetString("Products"),
                SaveText = this.globalLocalizer.GetString("SaveText"),
                OrderNameLabel = this.outletLocalizer.GetString("OrderNameLabel"),
                ProductRequiredErrorMessage = this.outletLocalizer.GetString("ProductRequiredErrorMessage"),
                NavigateToOutletListText = this.outletLocalizer.GetString("NavigateToOutletListText"),
                OutletUrl = this.linkGenerator.GetPathByAction("Index", "Outlet", new { Area = "Outlet", culture = CultureInfo.CurrentUICulture.Name }),
                OrderNameRequiredErrorMessage = this.outletLocalizer.GetString("OrderNameRequiredErrorMessage"),
                SaveUrl = this.linkGenerator.GetPathByAction("Save", "OutletApi", new { Area = "Outlet", culture = CultureInfo.CurrentUICulture.Name }),

            };
            var products = await this.productsRepository.GetAllProductsAsync(componentModel.Token, componentModel.Language, null);
            if (products != null)
            {
                viewModel.Products = products.Select(x => new ListInventoryItemViewModel { Id = x.Id, Name = x.Name, Sku = x.Sku });
            }

            if (componentModel.Id.HasValue)
            {
                viewModel.Id = componentModel.Id.Value;
            }

            return viewModel;
        }
    }
}
