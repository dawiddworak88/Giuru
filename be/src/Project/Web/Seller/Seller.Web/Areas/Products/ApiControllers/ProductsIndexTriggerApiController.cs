using Foundation.ApiExtensions.Controllers;
using Foundation.ApiExtensions.Definitions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Threading.Tasks;
using System.Net;
using Foundation.Localization;
using Microsoft.Extensions.Localization;
using Seller.Web.Areas.Shared.Repositories.Products;

namespace Seller.Web.Areas.Clients.ApiControllers
{
    [Area("Products")]
    public class ProductsIndexTriggerApiController : BaseApiController
    {
        private readonly IProductsRepository productsRepository;
        private readonly IStringLocalizer globalLocalizer;

        public ProductsIndexTriggerApiController(
            IProductsRepository productsRepository,
            IStringLocalizer<GlobalResources> globalLocalizer)
        {
            this.productsRepository = productsRepository;
            this.globalLocalizer = globalLocalizer;
        }

        [HttpPost]
        public async Task<IActionResult> Index()
        {
            await this.productsRepository.TriggerProductsReindexingAsync(
                await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName),
                CultureInfo.CurrentUICulture.Name);

            return this.StatusCode((int)HttpStatusCode.OK, new { Message = this.globalLocalizer.GetString("ReindexingTriggeredSuccessfully").Value });
        }
    }
}
