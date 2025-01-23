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
using Foundation.Extensions.ExtensionMethods;
using Seller.Web.Areas.Inventory.Repositories;
using Seller.Web.Areas.Products.ApiResponseModels;

namespace Seller.Web.Areas.Clients.ApiControllers
{
    [Area("Products")]
    public class ProductsApiController : BaseApiController
    {
        private readonly IProductsRepository _productsRepository;
        private readonly IStringLocalizer _productLocalizer;
        private readonly IInventoryRepository _inventoryRepository;
        private readonly IOutletRepository _outletRepository;

        public ProductsApiController(
            IProductsRepository productsRepository,
            IStringLocalizer<ProductResources> productLocalizer,
            IInventoryRepository inventoryRepository,
            IOutletRepository outletRepository)
        {
            _productsRepository = productsRepository;
            _productLocalizer = productLocalizer;
            _inventoryRepository = inventoryRepository;
            _outletRepository = outletRepository;
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

            foreach (var product in products.Data)
            {
                product.Name = $"{product.Name} ({product.Sku})";
            }

            return StatusCode((int)HttpStatusCode.OK, products);
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
                model.Images.OrEmptyIfNull().Select(x => x.Id),
                model.Files.OrEmptyIfNull().Select(x => x.Id),
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

        [HttpGet]
        public async Task<IActionResult> GetProductsQuantities(Guid id)
        {
            var token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName);
            var language = CultureInfo.CurrentUICulture.Name;

            var responseModel = new ProductQuantitiesResponseModel();

            var inventory = await _inventoryRepository.GetInventoryProductByProductIdAsync(
                token,
                language,
                id);

            if (inventory is not null)
            {
                responseModel.StockQuantity = inventory.AvailableQuantity;
            }

            var outlet = await _outletRepository.GetOutletItemByProductIdAsync(
                token,
                language,
                id);

            if (outlet is not null)
            {
                responseModel.OutletQuantity = outlet.AvailableQuantity;
            }

            return StatusCode((int)HttpStatusCode.OK, responseModel);
        }
    }
}
