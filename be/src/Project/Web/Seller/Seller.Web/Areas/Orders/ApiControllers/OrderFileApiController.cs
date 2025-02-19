﻿using Foundation.ApiExtensions.Controllers;
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

        public OrderFileApiController(
            IOrderFileService orderFileService,
            IProductsRepository productsRepository,
            IBasketRepository basketRepository,
            LinkGenerator linkGenerator,
            IMediaService mediaService,
            IMediaItemsRepository mediaRepository,
            IOrdersRepository ordersRepository,
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
        }

        [HttpPost]
        public async Task<IActionResult> Index([FromForm] UploadMediaRequestModel model)
        {
            var importedOrderLines = this.orderFileService.ImportOrderLines(model.File);

            var basketItems = new List<BasketItem>();

            foreach (var orderLine in importedOrderLines)
            {
                var product = await this.productsRepository.GetProductAsync(
                    await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName),
                    CultureInfo.CurrentUICulture.Name,
                    orderLine.Sku);

                if (product == null)
                {
                    this.logger.LogError($"Product for SKU {orderLine.Sku} and language {CultureInfo.CurrentUICulture.Name} couldn't be found.");
                }
                else
                {
                    var basketItem = new BasketItem
                    {
                        ProductId = product.Id,
                        ProductSku = product.Sku,
                        ProductName = product.Name,
                        PictureUrl = product.Images.OrEmptyIfNull().Any() ? this.mediaService.GetMediaUrl(product.Images.First(), OrdersConstants.Basket.BasketProductImageMaxWidth) : null,
                        Quantity = orderLine.Quantity,
                        ExternalReference = orderLine.ExternalReference,
                        MoreInfo = orderLine.MoreInfo
                    };

                    basketItems.Add(basketItem);
                }
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

            var productIds = basket.Items.OrEmptyIfNull().Select(x => x.ProductId.Value);

            if (productIds.OrEmptyIfNull().Any())
            {
                basketResponseModel.Items = basket.Items.OrEmptyIfNull().Select(x => new BasketItemResponseModel
                {
                    ProductId = x.ProductId,
                    ProductUrl = this.linkGenerator.GetPathByAction("Edit", "Product", new { Area = "Products", culture = CultureInfo.CurrentUICulture.Name, Id = x.ProductId }),
                    Name = x.ProductName,
                    Sku = x.ProductSku,
                    Quantity = x.Quantity,
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