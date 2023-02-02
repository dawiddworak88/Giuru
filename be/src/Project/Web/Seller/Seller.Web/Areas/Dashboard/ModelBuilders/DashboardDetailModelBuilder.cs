using Foundation.Extensions.ModelBuilders;
using Foundation.PageContent.ComponentModels;
using Seller.Web.Areas.Dashboard.ViewModels;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Dashboard.ModelBuilders
{
    public class DashboardDetailModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, DashboardDetailViewModel>
    {

        public DashboardDetailModelBuilder()
        {

        }

        public async Task<DashboardDetailViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = new DashboardDetailViewModel
            {

            };

            return viewModel;
        }
    }
}
