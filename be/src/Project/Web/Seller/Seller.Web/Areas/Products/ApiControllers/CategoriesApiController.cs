﻿using Foundation.ApiExtensions.Controllers;
using Foundation.ApiExtensions.Definitions;
using Foundation.Extensions.ExtensionMethods;
using Foundation.Localization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Seller.Web.Areas.Products.ApiRequestModels;
using Seller.Web.Areas.Products.DomainModels;
using Seller.Web.Areas.Products.Repositories;
using Seller.Web.Areas.Shared.Repositories.Media;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Products.ApiControllers
{
    [Area("Products")]
    public class CategoriesApiController : BaseApiController
    {
        private readonly ICategoriesRepository _categoriesRepository;
        private readonly IStringLocalizer _productLocalizer;
        private readonly IMediaItemsRepository _mediaItemsRepository;

        public CategoriesApiController(
            ICategoriesRepository categoriesRepository,
            IStringLocalizer<ProductResources> productLocalizer,
            IMediaItemsRepository mediaItemsRepository)
        {
            _categoriesRepository = categoriesRepository;
            _productLocalizer = productLocalizer;
            _mediaItemsRepository = mediaItemsRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Get(string searchTerm, int pageIndex, int itemsPerPage)
        {
            var categories = await _categoriesRepository.GetCategoriesAsync(
                await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName),
                CultureInfo.CurrentUICulture.Name,
                searchTerm,
                pageIndex,
                itemsPerPage,
                $"{nameof(Category.Order)}");

            return StatusCode((int)HttpStatusCode.OK, categories);
        }

        [HttpPost]
        public async Task<IActionResult> Index([FromBody] SaveCategoryRequestModel model)
        {
            var token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName);
            var language = CultureInfo.CurrentUICulture.Name;

            var categoryId = await _categoriesRepository.SaveAsync(
                token, 
                language, 
                model.Id, 
                model.ParentCategoryId, 
                model.Name, 
                model.Files.Select(x => x.Id.Value),
                model.Schemas.OrEmptyIfNull().Select(x => new CategorySchema
                {
                    Id = x.Id,
                    Schema = x.Schema,
                    UiSchema = x.UiSchema,
                    Language = x.Language
                }), 
                model.Order);

            return StatusCode((int)HttpStatusCode.OK, new { Id = categoryId, Message = _productLocalizer.GetString("CategorySavedSuccessfully").Value });
        }

        [HttpPost]        
        public async Task<IActionResult> Order([FromBody] SaveCategoryOrderRequestModel model)
        {
            var token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName);
            var language = CultureInfo.CurrentCulture.Name;

            var category = await _categoriesRepository.GetCategoryAsync(token, language, model.Id);

            if (category is not null) 
            {
                if (category.ThumbnailMediaId.HasValue)
                {
                    var mediaItem = await _mediaItemsRepository.GetMediaItemAsync(token, language, category.ThumbnailMediaId.Value);

                    if (mediaItem is not null)
                    {
                        var files = new List<Guid> {
                            mediaItem.Id
                        };

                        category.Files = files;
                    }
                }

                var categorySchemas = await _categoriesRepository.GetCategorySchemasAsync(token, language, model.Id);

                await _categoriesRepository.SaveAsync(
                        token, language, model.Id, category.ParentId, category.Name, category.Files,
                        categorySchemas?.Schemas.OrEmptyIfNull().Select(x => new CategorySchema
                        {
                            Id = x.Id,
                            Schema = x.Schema,
                            UiSchema = x.UiSchema,
                            Language = x.Language
                        }), model.Order);

                return StatusCode((int)HttpStatusCode.OK);
            }

            return StatusCode((int)HttpStatusCode.BadRequest);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(Guid? id)
        {
            await _categoriesRepository.DeleteAsync(
                await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName),
                CultureInfo.CurrentUICulture.Name,
                id);

            return StatusCode((int)HttpStatusCode.OK, new { Message = _productLocalizer.GetString("CategoryDeletedSuccessfully").Value });
        }
    }
}
