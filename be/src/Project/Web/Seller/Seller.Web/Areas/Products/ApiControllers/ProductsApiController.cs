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
using System.Security.Claims;
using Foundation.Account.Definitions;
using Foundation.Extensions.Helpers;
using Seller.Web.Areas.Shared.Repositories.Products;
using Seller.Web.Areas.Inventory.Repositories.Inventories;
using System.Collections.Generic;
using Seller.Web.Areas.Products.ApiResponseModels;
using Foundation.Extensions.ExtensionMethods;

namespace Seller.Web.Areas.Clients.ApiControllers
{
    [Area("Products")]
    public class ProductsApiController : BaseApiController
    {
        private readonly IProductsRepository _productsRepository;
        private readonly IStringLocalizer _productLocalizer;
        private readonly IInventoryRepository _inventoryRepository;

        public ProductsApiController(
            IProductsRepository productsRepository,
            IStringLocalizer<ProductResources> productLocalizer,
            IInventoryRepository inventoryRepository)
        {
            _productsRepository = productsRepository;
            _productLocalizer = productLocalizer;
            _inventoryRepository = inventoryRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Get(
            string searchTerm,
            bool? hasPrimaryProduct,
            int pageIndex,
            int itemsPerPage)
        {
            var products = await _productsRepository.GetProductsAsync(
                await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName),
                CultureInfo.CurrentUICulture.Name,
                searchTerm,
                hasPrimaryProduct,
                GuidHelper.ParseNullable((User.Identity as ClaimsIdentity).Claims.FirstOrDefault(x => x.Type == AccountConstants.Claims.OrganisationIdClaim)?.Value),
                pageIndex,
                itemsPerPage,
                null);

            return StatusCode((int)HttpStatusCode.OK, products);
        }

        [HttpGet]
        public async Task<IActionResult> GetProductOrderSuggestion(
            string searchTerm,
            bool? hasPrimaryProduct,
            int pageIndex,
            int itemsPerPage)
        {
            var token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName);
            var language = CultureInfo.CurrentUICulture.Name;

            var products = await _productsRepository.GetProductsAsync(
                token,
                language,
                searchTerm,
                hasPrimaryProduct,
                GuidHelper.ParseNullable((User.Identity as ClaimsIdentity).Claims.FirstOrDefault(x => x.Type == AccountConstants.Claims.OrganisationIdClaim)?.Value),
                pageIndex,
                itemsPerPage,
                null);

            var onStockProducts = await _inventoryRepository.GetInventoryProductsAsync(
                token,
                language,
                searchTerm,
                pageIndex,
                itemsPerPage,
                null);

            List<ProductOrderSuggestionResponseModel> suggestions = new List<ProductOrderSuggestionResponseModel>();

            foreach (var product in products.Data.OrEmptyIfNull())
            {
                var suggestion = new ProductOrderSuggestionResponseModel
                {
                    Id = product.Id,
                    Name = product.Name,
                    Sku = product.Sku,
                    Images = product.Images,
                };

                var onStockProduct = onStockProducts?.Data.FirstOrDefault(x => x.ProductId == product.Id);

                if (onStockProduct is not null)
                {
                    suggestion.StockQuantity = onStockProduct.AvailableQuantity;
                }

                suggestions.Add(suggestion);
            }

            return StatusCode((int)HttpStatusCode.OK, suggestions);
        }

        [HttpPost]
        public async Task<IActionResult> Index([FromBody] SaveProductRequestModel model)
        {
            var productId = await _productsRepository.SaveAsync(
                await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName),
                CultureInfo.CurrentUICulture.Name,
                model.Id,
                model.Name,
                model.Sku,
                model.Description,
                model.IsNew,
                model.IsPublished,
                model.PrimaryProductId,
                model.CategoryId,
                model.Images.Select(x => x.Id),
                model.Files.Select(x => x.Id),
                model.Ean,
                model.FulfillmentTime,
                model.FormData);

            return StatusCode((int)HttpStatusCode.OK, new { Id = productId, Message = _productLocalizer.GetString("ProductSavedSuccessfully").Value });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(Guid? id)
        {
            await _productsRepository.DeleteAsync(
                await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName),
                CultureInfo.CurrentUICulture.Name,
                id);

            return StatusCode((int)HttpStatusCode.OK, new { Message = _productLocalizer.GetString("ProductDeletedSuccessfully").Value });
        }
    }
}
