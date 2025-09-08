using Foundation.ApiExtensions.Controllers;
using Foundation.ApiExtensions.Definitions;
using Foundation.Extensions.ExtensionMethods;
using Foundation.GenericRepository.Paginations;
using Foundation.Media.Services.MediaServices;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using Seller.Web.Areas.Media.ApiRequestModels;
using Seller.Web.Areas.Orders.ApiResponseModels;
using Seller.Web.Areas.Orders.Definitions;
using Seller.Web.Areas.Orders.DomainModels;
using Seller.Web.Areas.Orders.Repositories.Baskets;
using Seller.Web.Areas.Orders.Repositories.Orders;
using Seller.Web.Areas.Orders.Services.OrderFiles;
using Seller.Web.Areas.Shared.Repositories.Media;
using Seller.Web.Areas.Shared.Repositories.Products;
using Seller.Web.Shared.Definitions;
using Seller.Web.Shared.DomainModels.Media;
using Seller.Web.Shared.Repositories.Inventory;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Orders.ApiControllers
{
    [Area("Orders")]
    public class OrderFileApiController : BaseApiController
    {
        private readonly IOrderFileService orderFileService;
        private readonly IProductsRepository productsRepository;
        private readonly IBasketRepository basketRepository;
        private readonly LinkGenerator linkGenerator;
        private readonly IMediaService mediaService;
        private readonly ILogger<OrderFileApiController> logger;
        private readonly IMediaItemsRepository mediaRepository;
        private readonly IOrdersRepository ordersRepository;
        private readonly IInventoryRepository inventoryRepository;

        public OrderFileApiController(
            IOrderFileService orderFileService,
            IProductsRepository productsRepository,
            IBasketRepository basketRepository,
            LinkGenerator linkGenerator,
            IMediaService mediaService,
            IMediaItemsRepository mediaRepository,
            IOrdersRepository ordersRepository,
            IInventoryRepository inventoryRepository,
            ILogger<OrderFileApiController> logger)
        {
            this.orderFileService = orderFileService;
            this.productsRepository = productsRepository;
            this.basketRepository = basketRepository;
            this.linkGenerator = linkGenerator;
            this.mediaService = mediaService;
            this.logger = logger;
            this.mediaRepository = mediaRepository;
            this.ordersRepository = ordersRepository;
            this.inventoryRepository = inventoryRepository;
        }

        [HttpPost]
        public async Task<IActionResult> Index([FromForm] UploadMediaRequestModel model)
        {
            var importedOrderLines = this.orderFileService.ImportOrderLines(model.File);

            var skus = importedOrderLines.OrEmptyIfNull().Select(x => x.Sku).Distinct();

            var token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName);
            var language = CultureInfo.CurrentUICulture.Name;

            var products = await this.productsRepository.GetProductsBySkusAsync(token, language, skus);

            var productBySku = products
                .OrEmptyIfNull()
                .ToDictionary(g => g.Sku, g => g);

            var productIds = products.OrEmptyIfNull().Select(x => x.Id).Distinct().ToList();
            var stockAvailableProducts = await this.inventoryRepository.GetAvailbleProductsByProductIdsAsync(token, language, productIds);

            var stockByProductId = stockAvailableProducts
               .OrEmptyIfNull()
               .ToDictionary(g => g.ProductId, g => g.AvailableQuantity);

            var basketItems = new List<BasketItem>();

            foreach (var orderLine in importedOrderLines)
            {
                if (!productBySku.TryGetValue(orderLine.Sku, out var product) || product == null)
                {
                    this.logger.LogError($"Product for SKU {orderLine.Sku} and language {language} couldn't be found.");
                    continue;
                }

                var availableStock = stockByProductId.TryGetValue(product.Id, out var qty) ? qty : 0;

                var stockQuantity = Math.Min((double)orderLine.Quantity, (double)availableStock);
                var quantity = orderLine.Quantity - stockQuantity;

                var basketItem = new BasketItem
                {
                    ProductId = product.Id,
                    ProductSku = product.Sku,
                    ProductName = product.Name,
                    PictureUrl = product.Images.OrEmptyIfNull().Any() ? this.mediaService.GetMediaUrl(product.Images.First(), OrdersConstants.Basket.BasketProductImageMaxWidth) : null,
                    Quantity = quantity,
                    StockQuantity = stockQuantity,
                    ExternalReference = orderLine.ExternalReference,
                    MoreInfo = orderLine.MoreInfo
                };

                basketItems.Add(basketItem);
            }

            var basket = await this.basketRepository.SaveAsync(
                await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName),
                CultureInfo.CurrentUICulture.Name,
                model.Id,
                basketItems);

            var basketResponseModel = new BasketResponseModel
            {
                Id = basket.Id
            };

            if (basket.Items.OrEmptyIfNull().Any())
            {
                basketResponseModel.Items = basket.Items.OrEmptyIfNull().Select(x => new BasketItemResponseModel
                {
                    ProductId = x.ProductId,
                    ProductUrl = this.linkGenerator.GetPathByAction("Edit", "Product", new { Area = "Products", culture = CultureInfo.CurrentUICulture.Name, Id = x.ProductId }),
                    Name = x.ProductName,
                    Sku = x.ProductSku,
                    Quantity = x.Quantity,
                    StockQuantity = x.StockQuantity,
                    OutletQuantity = x.OutletQuantity,
                    ExternalReference = x.ExternalReference,
                    ImageSrc = x.PictureUrl,
                    ImageAlt = x.ProductName,
                    MoreInfo = x.MoreInfo
                });
            }

            return this.StatusCode((int)HttpStatusCode.OK, basketResponseModel);
        }

        [HttpGet]
        public async Task<IActionResult> GetFiles(Guid? id, string searchTerm, int pageIndex, int itemsPerPage)
        {
            var token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName);
            var language = CultureInfo.CurrentUICulture.Name;

            var productFiles = await this.ordersRepository.GetOrderFilesAsync(token, language, id, pageIndex, itemsPerPage, searchTerm, $"{nameof(OrderFile.CreatedDate)} desc");

            var filesModel = new List<FileItem>();
            var filesIds = productFiles.Data.Select(x => x.Id);

            if (productFiles is not null && filesIds.Any())
            {
                var files = await this.mediaRepository.GetMediaItemsAsync(filesIds, language, FilesConstants.DefaultPageIndex, FilesConstants.DefaultPageSize, token);

                foreach (var file in files.Data.OrEmptyIfNull())
                {
                    var fileModel = new FileItem
                    {
                        Id = file.Id,
                        Name = file.Name,
                        Filename = file.Filename,
                        Url = this.mediaService.GetNonCdnMediaUrl(file.Id),
                        Description = file.Description ?? "-",
                        IsProtected = file.IsProtected,
                        Size = this.mediaService.ConvertToMB(file.Size),
                        LastModifiedDate = file.LastModifiedDate,
                        CreatedDate = file.CreatedDate
                    };

                    filesModel.Add(fileModel);
                }
            }

            var pagedFiles = new PagedResults<IEnumerable<FileItem>>(filesModel.Count, FilesConstants.DefaultPageSize)
            {
                Data = filesModel
            };

            return this.StatusCode((int)HttpStatusCode.OK, pagedFiles);
        }
    }
}