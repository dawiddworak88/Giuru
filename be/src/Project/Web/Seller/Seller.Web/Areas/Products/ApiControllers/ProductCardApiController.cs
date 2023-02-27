using Foundation.ApiExtensions.Controllers;
using Foundation.ApiExtensions.Definitions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Seller.Web.Areas.Products.Repositories;
using System;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Products.ApiControllers
{
    [Area("Products")]
    public class ProductCardApiController : BaseApiController
    {
        private readonly IProductAttributeItemsRepository _productAttributeItemsRepository;

        public ProductCardApiController(
            IProductAttributeItemsRepository productAttributeItemsRepository)
        {
            _productAttributeItemsRepository = productAttributeItemsRepository;
        }

        public async Task<IActionResult> Index(Guid? id) {
            var token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName);
            var language = CultureInfo.CurrentUICulture.Name;

            var attributeItems = await _productAttributeItemsRepository.GetAsync(token, language, id);

            if (attributeItems is not null)
            {
                var response = attributeItems.GroupBy(x => x.ProductAttributeId);

                Console.WriteLine(JsonConvert.SerializeObject(response));
            }

            return this.StatusCode((int)HttpStatusCode.OK);
        }
    }
}
