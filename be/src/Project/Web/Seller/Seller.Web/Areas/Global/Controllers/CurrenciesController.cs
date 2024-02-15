using Foundation.ApiExtensions.Definitions;
using Foundation.Extensions.Controllers;
using Foundation.Extensions.ModelBuilders;
using Foundation.PageContent.ComponentModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Seller.Web.Areas.Global.ModelBuilders;
using Seller.Web.Areas.Global.ViewModels;
using System;
using System.Globalization;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Global.Controllers
{
    [Area("Global")]
    public class CurrenciesController : BaseController
    {
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, CurrenciesPageViewModel> _currenciesPageModelBuilder;

        public CurrenciesController(
            IAsyncComponentModelBuilder<ComponentModelBase, CurrenciesPageViewModel> currenciesPageModelBuilder)
        {
            _currenciesPageModelBuilder = currenciesPageModelBuilder;
        }

        public async Task<IActionResult> Index()
        {
            var componentModel = new ComponentModelBase
            {
                Language = CultureInfo.CurrentUICulture.Name,
                Name = User.Identity.Name,
                IsAuthenticated = User.Identity.IsAuthenticated,
                Token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName)
            };

            var viewModel = await _currenciesPageModelBuilder.BuildModelAsync(componentModel);

            return View(viewModel);
        }
    }
}
