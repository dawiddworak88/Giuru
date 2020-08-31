using Buyer.Web.Areas.Home.ViewModel;
using Foundation.Extensions.Controllers;
using Foundation.Extensions.ModelBuilders;
using Foundation.PageContent.ComponentModels;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Threading.Tasks;

namespace Buyer.Web.Areas.Home.Controllers
{
    [Area("Home")]
    public class HomeController : BaseController
    {
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, HomePageViewModel> homePageModelBuilder;

        public HomeController(IAsyncComponentModelBuilder<ComponentModelBase, HomePageViewModel> homePageModelBuilder)
        {
            this.homePageModelBuilder = homePageModelBuilder;
        }

        public async Task<IActionResult> Index()
        {
            var componentModel = new ComponentModelBase
            {
                Language = CultureInfo.CurrentUICulture.Name
            };
            
            var viewModel = await this.homePageModelBuilder.BuildModelAsync(componentModel);

            return this.View(viewModel);
        }
    }
}
