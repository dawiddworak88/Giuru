using Buyer.Web.Areas.Home.ViewModel;
using Foundation.Extensions.Controllers;
using Foundation.Extensions.ModelBuilders;
using Microsoft.AspNetCore.Mvc;

namespace Buyer.Web.Areas.Home.Controllers
{
    [Area("Home")]
    public class HomeController : BaseController
    {
        private readonly IModelBuilder<HomePageViewModel> homePageModelBuilder;

        public HomeController(IModelBuilder<HomePageViewModel> homePageModelBuilder)
        {
            this.homePageModelBuilder = homePageModelBuilder;
        }

        public IActionResult Index()
        {
            var viewModel = this.homePageModelBuilder.BuildModel();

            return this.View(viewModel);
        }
    }
}
