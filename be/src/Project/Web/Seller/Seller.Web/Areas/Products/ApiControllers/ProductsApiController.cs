using Foundation.ApiExtensions.Controllers;
using Foundation.ApiExtensions.Definitions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Threading.Tasks;
using Seller.Web.Areas.Products.ApiRequestModels;
using System;
using System.Net;
using Foundation.Localization;
using Microsoft.Extensions.Localization;
using System.Linq;
using Seller.Web.Areas.Products.Repositories;
using System.Security.Claims;
using Foundation.Account.Definitions;
using Foundation.Extensions.Helpers;

namespace Seller.Web.Areas.Clients.ApiControllers
{
    [Area("Products")]
    public class ProductsApiController : BaseApiController
    {
        private readonly IProductsRepository productsRepository;
        private readonly IStringLocalizer productLocalizer;

        public ProductsApiController(
            IProductsRepository productsRepository,
            IStringLocalizer<ProductResources> productLocalizer)
        {
            this.productsRepository = productsRepository;
            this.productLocalizer = productLocalizer;
        }

        [HttpGet]
        public async Task<IActionResult> Get(string searchTerm, int pageIndex, int itemsPerPage)
        {
            var products = await this.productsRepository.GetProductsAsync(
                await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName),
                CultureInfo.CurrentUICulture.Name,
                searchTerm,
                GuidHelper.ParseNullable((this.User.Identity as ClaimsIdentity).Claims.FirstOrDefault(x => x.Type == AccountConstants.OrganisationIdClaim)?.Value),
                pageIndex,
                itemsPerPage);

            return this.StatusCode((int)HttpStatusCode.OK, products);
        }

        //[HttpPost]
        //public async Task<IActionResult> Index([FromBody] SaveCategoryRequestModel model)
        //{
        //    var categoryId = await this.productsRepository.SaveAsync(
        //        await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName),
        //        CultureInfo.CurrentUICulture.Name,
        //        model.Id,
        //        model.ParentCategoryId,
        //        model.Name,
        //        model.Files.Select(x => x.Id.Value));

        //    return this.StatusCode((int)HttpStatusCode.OK, new { Id = categoryId, Message = this.productLocalizer.GetString("CategorySavedSuccessfully").Value });
        //}

        [HttpDelete]
        public async Task<IActionResult> Delete(Guid? id)
        {
            await this.productsRepository.DeleteAsync(
                await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName),
                CultureInfo.CurrentUICulture.Name,
                id);

            return this.StatusCode((int)HttpStatusCode.OK, new { Message = this.productLocalizer.GetString("CategoryDeletedSuccessfully").Value });
        }
    }
}
