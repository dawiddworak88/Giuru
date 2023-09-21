using Catalog.Api.ServicesModels.Categories;
using Catalog.Api.v1.Categories.RequestModels;
using Catalog.Api.Services.Categories;
using Catalog.Api.Validators.Categories;
using Foundation.Account.Definitions;
using Foundation.ApiExtensions.Controllers;
using Foundation.Extensions.Definitions;
using Foundation.Extensions.Exceptions;
using Foundation.Extensions.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Collections.Generic;
using Foundation.GenericRepository.Paginations;
using Catalog.Api.v1.Categories.ResultModels;
using Foundation.Extensions.ExtensionMethods;
using Catalog.Api.v1.Categories.ResponseModels;

namespace Catalog.Api.v1.Categories.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Authorize]
    [ApiController]
    public class CategoriesController : BaseApiController
    {
        private readonly ICategoriesService _categoryService;

        public CategoriesController(ICategoriesService categoryService)
        {
            _categoryService = categoryService;
        }

        /// <summary>
        /// Gets list of categories.
        /// </summary>
        /// <param name="searchTerm">The search term.</param>
        /// <param name="level">The level.</param>
        /// <param name="leafOnly">Only categories with no subcategories.</param>
        /// <param name="pageIndex">The page index.</param>
        /// <param name="itemsPerPage">The items per page.</param>
        /// <param name="orderBy">The optional order by.</param>
        /// <returns>The list of categories.</returns>
        [HttpGet, MapToApiVersion("1.0")]
        [AllowAnonymous]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(PagedResults<IEnumerable<CategoryResponseModel>>))]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.UnprocessableEntity)]
        public IActionResult Get(string searchTerm, int? level, bool? leafOnly, int? pageIndex, int? itemsPerPage, string orderBy)
        {
            var serviceModel = new GetCategoriesServiceModel
            {
                Level = level,
                Language = CultureInfo.CurrentCulture.Name,
                SearchTerm = searchTerm,
                LeafOnly = leafOnly,
                PageIndex = pageIndex,
                ItemsPerPage = itemsPerPage,
                OrderBy = orderBy
            };

            var categories = _categoryService.Get(serviceModel);

            if (categories is not null)
            {
                var response = new PagedResults<IEnumerable<CategoryResponseModel>>(categories.Total, categories.PageSize)
                {
                    Data = categories.Data.OrEmptyIfNull().Select(x => new CategoryResponseModel
                    {
                        Id = x.Id,
                        IsLeaf = x.IsLeaf,
                        Level = x.Level,
                        Name = x.Name,
                        Schema = x.Schema,
                        UiSchema = x.UiSchema,
                        Order = x.Order,
                        ParentId = x.ParentId,
                        ThumbnailMediaId = x.ThumbnailMediaId,
                        ParentCategoryName = x.ParentCategoryName,
                        LastModifiedDate = x.LastModifiedDate,
                        CreatedDate = x.CreatedDate
                    })
                };

                return StatusCode((int)HttpStatusCode.OK, response);
            }

            return StatusCode((int)HttpStatusCode.UnprocessableEntity);
        }

        /// <summary>
        /// Gets category by id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>The category.</returns>
        [HttpGet, MapToApiVersion("1.0")]
        [Route("{id}")]
        [AllowAnonymous]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(CategoryResponseModel))]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.Conflict)]
        [ProducesResponseType((int)HttpStatusCode.UnprocessableEntity)]
        public IActionResult Get(Guid? id)
        {
            var serviceModel = new GetCategoryServiceModel
            {
                Id = id,
                Language = CultureInfo.CurrentCulture.Name
            };

            var validator = new GetCategoryModelValidator();

            var validationResult = validator.Validate(serviceModel);

            if (validationResult.IsValid)
            {
                var category = _categoryService.Get(serviceModel);

                if (category is not null)
                {
                    var response = new CategoryResponseModel
                    { 
                        Id = category.Id,
                        IsLeaf = category.IsLeaf,
                        Level = category.Level,
                        Name = category.Name,
                        Order = category.Order,
                        ParentCategoryName = category.ParentCategoryName,
                        ParentId = category.ParentId,
                        ThumbnailMediaId = category.ThumbnailMediaId,
                        LastModifiedDate = category.LastModifiedDate,
                        CreatedDate = category.CreatedDate
                    };

                    return StatusCode((int)HttpStatusCode.OK, response);
                }
                else
                {
                    return StatusCode((int)HttpStatusCode.NotFound);
                }
            }

            throw new CustomException(string.Join(ErrorConstants.ErrorMessagesSeparator, validationResult.Errors.Select(x => x.ErrorMessage)), (int)HttpStatusCode.UnprocessableEntity);
        }

        /// <summary>
        /// Creates or updates category (if category id is set).
        /// </summary>
        /// <param name="request">The model.</param>
        /// <returns>The category id.</returns>
        [HttpPost, MapToApiVersion("1.0")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(CategoryResponseModel))]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.Conflict)]
        [ProducesResponseType((int)HttpStatusCode.UnprocessableEntity)]
        public async Task<IActionResult> Save(CategoryRequestModel request)
        {
            var sellerClaim = User.Claims.FirstOrDefault(x => x.Type == AccountConstants.Claims.OrganisationIdClaim);

            if (request.Id.HasValue)
            {
                var serviceModel = new UpdateCategoryServiceModel
                {
                    Id = request.Id,
                    Files = request.Files,
                    Name = request.Name,
                    Order = request.Order,
                    Schemas = request.Schemas.Select(x => new CategorySchemaServiceModel
                    {
                        Id = x.Id,
                        Schema = x.Schema,
                        UiSchema = x.UiSchema,
                        Language = x.Language
                    }),
                    ParentId = request.ParentCategoryId,
                    Language = CultureInfo.CurrentCulture.Name,
                    Username = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value,
                    OrganisationId = GuidHelper.ParseNullable(sellerClaim?.Value)
                };

                var validator = new UpdateCategoryModelValidator();

                var validationResult = await validator.ValidateAsync(serviceModel);

                if (validationResult.IsValid)
                {
                    var category = await _categoryService.UpdateAsync(serviceModel);

                    if (category is not null)
                    {
                        var response = new CategoryResponseModel
                        {
                            Id = category.Id,
                            IsLeaf = category.IsLeaf,
                            Level = category.Level,
                            Name = category.Name,
                            Order = category.Order,
                            ParentCategoryName = category.ParentCategoryName,
                            ParentId = category.ParentId,
                            ThumbnailMediaId = category.ThumbnailMediaId,
                            LastModifiedDate = category.LastModifiedDate,
                            CreatedDate = category.CreatedDate
                        };

                        return StatusCode((int)HttpStatusCode.OK, response);
                    }
                }

                throw new CustomException(string.Join(ErrorConstants.ErrorMessagesSeparator, validationResult.Errors.Select(x => x.ErrorMessage)), (int)HttpStatusCode.UnprocessableEntity);
            }
            else
            {
                var serviceModel = new CreateCategoryServiceModel
                {
                    Name = request.Name,
                    Schemas = request.Schemas.Select(x => new CategorySchemaServiceModel 
                    { 
                        Id = x.Id,
                        Schema = x.Schema,
                        UiSchema = x.UiSchema,
                        Language = x.Language
                    }),
                    ParentId = request.ParentCategoryId,
                    Files = request.Files,
                    Language = CultureInfo.CurrentCulture.Name,
                    Username = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value,
                    OrganisationId = GuidHelper.ParseNullable(sellerClaim?.Value)
                };

                var validator = new CreateCategoryModelValidator();

                var validationResult = await validator.ValidateAsync(serviceModel);

                if (validationResult.IsValid)
                {
                    var category = await _categoryService.CreateAsync(serviceModel);

                    if (category is not null)
                    {
                        var response = new CategoryResponseModel
                        {
                            Id = category.Id,
                            IsLeaf = category.IsLeaf,
                            Level = category.Level,
                            Name = category.Name,
                            Order = category.Order,
                            ParentCategoryName = category.ParentCategoryName,
                            ParentId = category.ParentId,
                            ThumbnailMediaId = category.ThumbnailMediaId,
                            LastModifiedDate = category.LastModifiedDate,
                            CreatedDate = category.CreatedDate
                        };

                        return StatusCode((int)HttpStatusCode.Created, response);
                    }
                }

                throw new CustomException(string.Join(ErrorConstants.ErrorMessagesSeparator, validationResult.Errors.Select(x => x.ErrorMessage)), (int)HttpStatusCode.UnprocessableEntity);
            }
        }

        /// <summary>
        /// Delete category by id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>OK.</returns>
        [HttpDelete, MapToApiVersion("1.0")]
        [Route("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.Conflict)]
        [ProducesResponseType((int)HttpStatusCode.UnprocessableEntity)]
        public async Task<IActionResult> Delete(Guid? id)
        {
            var sellerClaim = User.Claims.FirstOrDefault(x => x.Type == AccountConstants.Claims.OrganisationIdClaim);
            var serviceModel = new DeleteCategoryServiceModel
            {
                Id = id,
                Language = CultureInfo.CurrentCulture.Name,
                Username = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value,
                OrganisationId = GuidHelper.ParseNullable(sellerClaim?.Value)
            };

            var validator = new DeleteCategoryModelValidator();

            var validationResult = await validator.ValidateAsync(serviceModel);

            if (validationResult.IsValid)
            {
                await _categoryService.DeleteAsync(serviceModel);

                return StatusCode((int)HttpStatusCode.OK);
            }

            throw new CustomException(string.Join(ErrorConstants.ErrorMessagesSeparator, validationResult.Errors.Select(x => x.ErrorMessage)), (int)HttpStatusCode.UnprocessableEntity);
        }

        /// <summary>
        /// Updates category schema.
        /// </summary>
        /// <param name="request">The model.</param>
        /// <returns>The category schema.</returns>
        [HttpPost, MapToApiVersion("1.0")]
        [Route("CategorySchemas")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(CategorySchemasResponseModel))]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.Conflict)]
        [ProducesResponseType((int)HttpStatusCode.UnprocessableEntity)]
        public async Task<IActionResult> SaveCategorySchema(SaveCategorySchemaRequestModel request)
        {
            var sellerClaim = User.Claims.FirstOrDefault(x => x.Type == AccountConstants.Claims.OrganisationIdClaim);

            var serviceModel = new UpdateCategorySchemaServiceModel
            {
                Id = request.CategoryId,
                Schema = request.Schema,
                UiSchema = request.UiSchema,
                Language = CultureInfo.CurrentCulture.Name,
                Username = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value,
                OrganisationId = GuidHelper.ParseNullable(sellerClaim?.Value)
            };

            var validator = new UpdateCategorySchemaModelValidator();

            var validationResult = await validator.ValidateAsync(serviceModel);

            if (validationResult.IsValid)
            {
                var categorySchemas = await _categoryService.UpdateCategorySchemaAsync(serviceModel);

                if (categorySchemas is not null)
                {
                    var response = new CategorySchemasResponseModel
                    {
                        Id = categorySchemas.Id,
                        Schemas = categorySchemas.Schemas.Select(x => new CategorySchemaResponseModel 
                        {
                            Id = x.Id,
                            Schema = x.Schema,
                            UiSchema = x.UiSchema,
                            Language = x.Language
                        }),
                        LastModifiedDate = categorySchemas.LastModifiedDate,
                        CreatedDate = categorySchemas.CreatedDate
                    };

                    return StatusCode((int)HttpStatusCode.OK, response);
                }
            }

            throw new CustomException(string.Join(ErrorConstants.ErrorMessagesSeparator, validationResult.Errors.Select(x => x.ErrorMessage)), (int)HttpStatusCode.UnprocessableEntity);
        }

        /// <summary>
        /// Gets schemas by category id.
        /// </summary>
        /// <param name="categoryId">The category id.</param>
        /// <returns>The category schema.</returns>
        [HttpGet, MapToApiVersion("1.0")]
        [Route("CategorySchemas/{categoryId}")]
        [AllowAnonymous]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(CategorySchemasResponseModel))]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.UnprocessableEntity)]
        public async Task<IActionResult> GetCategorySchemasByCategoryId(Guid? categoryId)
        {
            var serviceModel = new GetCategorySchemasServiceModel
            {
                Id = categoryId,
                Language = CultureInfo.CurrentCulture.Name
            };

            var validator = new GetCategorySchemasModelValidator();

            var validationResult = validator.Validate(serviceModel);

            if (validationResult.IsValid)
            {
                var categorySchemas = await _categoryService.GetCategorySchemasAsync(serviceModel);

                if (categorySchemas is not null)
                {
                    var response = new CategorySchemasResponseModel
                    {
                        Id = categorySchemas.Id,
                        Schemas = categorySchemas.Schemas.Select(x => new CategorySchemaResponseModel
                        {
                            Id = x.Id,
                            Schema = x.Schema,
                            UiSchema = x.UiSchema,
                            Language = x.Language
                        }),
                        LastModifiedDate = categorySchemas.LastModifiedDate,
                        CreatedDate = categorySchemas.CreatedDate
                    };

                    return StatusCode((int)HttpStatusCode.OK, response);
                }
                else
                {
                    return StatusCode((int)HttpStatusCode.NotFound);
                }
            }

            throw new CustomException(string.Join(ErrorConstants.ErrorMessagesSeparator, validationResult.Errors.Select(x => x.ErrorMessage)), (int)HttpStatusCode.UnprocessableEntity);
        }
    }
}
