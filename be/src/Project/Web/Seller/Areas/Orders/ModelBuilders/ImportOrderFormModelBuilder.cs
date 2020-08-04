using Seller.Web.Areas.Orders.ViewModel;
using Foundation.Extensions.ModelBuilders;
using Microsoft.Extensions.Localization;
using Feature.Order;
using Foundation.Localization;
using Seller.Web.Areas.Clients.Repositories;
using System.Threading.Tasks;
using Seller.Web.Shared.ComponentModels;
using Microsoft.AspNetCore.Routing;
using System.Globalization;

namespace Seller.Web.Areas.Orders.ModelBuilders
{
    public class ImportOrderFormModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, ImportOrderFormViewModel>
    {
        private readonly IClientsRepository clientsRepository;
        private readonly IStringLocalizer<GlobalResources> globalLocalizer;
        private readonly IStringLocalizer<OrderResources> orderLocalizer;
        private readonly LinkGenerator linkGenerator;
        public ImportOrderFormModelBuilder(
            IClientsRepository clientsRepository,
            IStringLocalizer<GlobalResources> globalLocalizer,
            IStringLocalizer<OrderResources> orderLocalizer,
            LinkGenerator linkGenerator)
        {
            this.clientsRepository = clientsRepository;
            this.globalLocalizer = globalLocalizer;
            this.orderLocalizer = orderLocalizer;
            this.linkGenerator = linkGenerator;
        }

        public async Task<ImportOrderFormViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = new ImportOrderFormViewModel
            {
                Clients = await this.clientsRepository.GetAllClientsAsync(componentModel.Token, componentModel.Language),
                ValidateOrderUrl = this.linkGenerator.GetPathByAction("Validate", "ImportOrderApi", new { Area = "Orders", culture = CultureInfo.CurrentUICulture.Name }),
                DropFilesLabel = this.orderLocalizer["DropFile"],
                DropOrSelectFilesLabel = this.orderLocalizer["DropOrSelectFile"],
                SelectClientLabel = this.orderLocalizer["SelectClient"],
                SaveText = this.orderLocalizer["ImportOrder"],
                GeneralErrorMessage = this.globalLocalizer["AnErrorOccurred"]
            };

            return viewModel;
        }
    }
}
