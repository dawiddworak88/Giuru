using Foundation.ApiExtensions.Definitions;
using Foundation.Extensions.Controllers;
using Foundation.Extensions.ModelBuilders;
using Foundation.PageContent.ComponentModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Seller.Web.Areas.Global.ViewModels;
using System.Globalization;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Global.Controllers
{
    [Area("Global")]
    public class CountriesController : BaseController
    {
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, CountriesPageViewModel> countriesPageModelBuilder;

        public CountriesController(
            IAsyncComponentModelBuilder<ComponentModelBase, CountriesPageViewModel> countriesPageModelBuilder)
        {
            this.countriesPageModelBuilder = countriesPageModelBuilder;
        }

        public async Task<IActionResult> Index()
        {
            var componentModel = new ComponentModelBase
            {
                Language = CultureInfo.CurrentUICulture.Name,
                Name = this.User.Identity.Name,
                IsAuthenticated = this.User.Identity.IsAuthenticated,
                Token = await this.HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName)
            };

            var viewModel = await this.countriesPageModelBuilder.BuildModelAsync(componentModel);

            return View(viewModel);
        }
    }
}
