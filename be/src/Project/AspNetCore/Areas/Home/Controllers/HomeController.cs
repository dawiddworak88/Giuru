using AspNetCore.Areas.Home.ViewModel;
using Foundation.Extensions.Controllers;
using Foundation.Extensions.ModelBuilders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCore.Areas.Home.Controllers
{
    [Area("Home")]
    [AllowAnonymous]
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
