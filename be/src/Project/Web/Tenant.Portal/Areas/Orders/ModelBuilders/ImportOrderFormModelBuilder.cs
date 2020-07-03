using Tenant.Portal.Areas.Orders.ViewModel;
using Foundation.Extensions.ModelBuilders;
using Microsoft.Extensions.Localization;
using Feature.Order;
using Foundation.Localization;
using Tenant.Portal.Areas.Clients.Repositories;
using System.Threading.Tasks;
using Tenant.Portal.Shared.ComponentModels;

namespace Tenant.Portal.Areas.Orders.ModelBuilders
{
    public class ImportOrderFormModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, ImportOrderFormViewModel>
    {
        private readonly IClientsRepository clientsRepository;
        private readonly IStringLocalizer<GlobalResources> globalLocalizer;
        private readonly IStringLocalizer<OrderResources> orderLocalizer;

        public ImportOrderFormModelBuilder(
            IClientsRepository clientsRepository,
            IStringLocalizer<GlobalResources> globalLocalizer,
            IStringLocalizer<OrderResources> orderLocalizer)
        {
            this.clientsRepository = clientsRepository;
            this.globalLocalizer = globalLocalizer;
            this.orderLocalizer = orderLocalizer;
        }

        public async Task<ImportOrderFormViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = new ImportOrderFormViewModel
            {
                Clients = await this.clientsRepository.GetAllClientsAsync(componentModel.Token, componentModel.Language),
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
