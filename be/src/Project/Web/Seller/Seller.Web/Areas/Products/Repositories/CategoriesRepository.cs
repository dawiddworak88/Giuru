using Foundation.ApiExtensions.Communications;
using Foundation.ApiExtensions.Models.Request;
using Foundation.ApiExtensions.Models.Response;
using Foundation.ApiExtensions.Services.ApiClientServices;
using Foundation.ApiExtensions.Shared.Definitions;
using Foundation.Extensions.Exceptions;
using Foundation.GenericRepository.Paginations;
using Microsoft.Extensions.Options;
using Seller.Web.Areas.Products.ApiRequestModels;
using Seller.Web.Areas.Products.DomainModels;
using Seller.Web.Areas.Products.Repositories;
using Seller.Web.Shared.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Categories.Repositories
{
    public class CategoriesRepository : ICategoriesRepository
    {
        private readonly IApiClientService _apiClientService;
        private readonly IOptions<AppSettings> _settings;

        public CategoriesRepository(
            IApiClientService apiClientService,
            IOptions<AppSettings> settings)
        {
            _apiClientService = apiClientService;
            _settings = settings;
        }

        public async Task<Guid> SaveAsync(string token, string language, Guid? id, Guid? parentCategoryId, string name, IEnumerable<Guid> files, IEnumerable<CategorySchema> schemas, int order)
        {
            var requestModel = new SaveCategoryApiRequestModel
            {
                Id = id,
                ParentCategoryId = parentCategoryId,
                Name = name,
                Order = order,
                Files = files,
                Schemas = schemas.Select(x => new CategorySchemaRequestModel 
                { 
                    Id = x.Id,
                    Schema = x.Schema,
                    UiSchema = x.UiSchema,
                    Language = x.Language
                })
            };

            var apiRequest = new ApiRequest<SaveCategoryApiRequestModel>
            {
                Language = language,
                Data = requestModel,
                AccessToken = token,
                EndpointAddress = $"{_settings.Value.CatalogUrl}{ApiConstants.Catalog.CategoriesApiEndpoint}"
            };

            var response = await _apiClientService.PostAsync<ApiRequest<SaveCategoryApiRequestModel>, SaveCategoryApiRequestModel, BaseResponseModel>(apiRequest);
            
            if (response.IsSuccessStatusCode && response.Data?.Id != null)
            {
                return response.Data.Id.Value;
            }

            if (response.IsSuccessStatusCode is false)
            {
                throw new CustomException(response.Message, (int)response.StatusCode);
            }

            return default;
        }

        public async Task DeleteAsync(string token, string language, Guid? id)
        {
            var apiRequest = new ApiRequest<RequestModelBase>
            { 
                Language = language,
                Data = new RequestModelBase(),
                AccessToken = token,
                EndpointAddress = $"{_settings.Value.CatalogUrl}{ApiConstants.Catalog.CategoriesApiEndpoint}/{id}"
            };

            var response = await _apiClientService.DeleteAsync<ApiRequest<RequestModelBase>, RequestModelBase, BaseResponseModel>(apiRequest);

            if (response.IsSuccessStatusCode is false)
            {
                throw new CustomException(response.Message, (int)response.StatusCode);
            }
        }

        public async Task<PagedResults<IEnumerable<Category>>> GetCategoriesAsync(string token, string language, string searchTerm, int pageIndex, int itemsPerPage, string orderBy)
        {
            var categoriesRequestModel = new PagedRequestModelBase
            {
                SearchTerm = searchTerm,
                PageIndex = pageIndex,
                ItemsPerPage = itemsPerPage,
                OrderBy = orderBy
            };

            var apiRequest = new ApiRequest<PagedRequestModelBase>
            {
                Language = language,
                Data = categoriesRequestModel,
                AccessToken = token,
                EndpointAddress = $"{_settings.Value.CatalogUrl}{ApiConstants.Catalog.CategoriesApiEndpoint}"
            };

            var response = await _apiClientService.GetAsync<ApiRequest<PagedRequestModelBase>, PagedRequestModelBase, PagedResults<IEnumerable<Category>>>(apiRequest);

            if (response.IsSuccessStatusCode && response.Data?.Data != null)
            {
                return new PagedResults<IEnumerable<Category>>(response.Data.Total, response.Data.PageSize)
                {
                    Data = response.Data.Data
                };
            }

            if (response.IsSuccessStatusCode is false)
            {
                throw new CustomException(response.Message, (int)response.StatusCode);
            }

            return default;
        }

        public async Task<IEnumerable<Category>> GetAllCategoriesAsync(string token, string language, bool? leafOnly, string orderBy)
        {
            var categoriesRequestModel = new PagedCategoriesRequestModel
            {
                LeafOnly = leafOnly,
                PageIndex = PaginationConstants.DefaultPageIndex,
                ItemsPerPage = PaginationConstants.DefaultPageSize,
                OrderBy = orderBy
            };

            var apiRequest = new ApiRequest<PagedCategoriesRequestModel>
            {
                Language = language,
                Data = categoriesRequestModel,
                AccessToken = token,
                EndpointAddress = $"{_settings.Value.CatalogUrl}{ApiConstants.Catalog.CategoriesApiEndpoint}"
            };

            var response = await _apiClientService.GetAsync<ApiRequest<PagedCategoriesRequestModel>, PagedCategoriesRequestModel, PagedResults<IEnumerable<Category>>>(apiRequest);
            
            if (response.IsSuccessStatusCode && response.Data?.Data != null)
            {
                var categories = new List<Category>();

                categories.AddRange(response.Data.Data);

                int totalPages = (int)Math.Ceiling(response.Data.Total / (double)PaginationConstants.DefaultPageSize);

                for (int i = PaginationConstants.SecondPage; i <= totalPages; i++)
                {
                    apiRequest.Data.PageIndex = i;

                    var nextPagesResponse = await _apiClientService.GetAsync<ApiRequest<PagedCategoriesRequestModel>, PagedCategoriesRequestModel, PagedResults<IEnumerable<Category>>>(apiRequest);

                    if (nextPagesResponse.IsSuccessStatusCode is false)
                    {
                        throw new CustomException(response.Message, (int)response.StatusCode);
                    }

                    if (nextPagesResponse.IsSuccessStatusCode && nextPagesResponse.Data?.Data != null && nextPagesResponse.Data.Data.Any())
                    {
                        categories.AddRange(nextPagesResponse.Data.Data);
                    }
                }

                return categories;
            }

            if (response.IsSuccessStatusCode is false)
            {
                throw new CustomException(response.Message, (int)response.StatusCode);
            }

            return default;
        }

        public async Task<Category> GetCategoryAsync(string token, string language, Guid? id)
        {
            var apiRequest = new ApiRequest<RequestModelBase>
            {
                Language = language,
                Data = new RequestModelBase(),
                AccessToken = token,
                EndpointAddress = $"{_settings.Value.CatalogUrl}{ApiConstants.Catalog.CategoriesApiEndpoint}/{id}"
            };

            var response = await _apiClientService.GetAsync<ApiRequest<RequestModelBase>, RequestModelBase, Category>(apiRequest);

            if (response.IsSuccessStatusCode && response.Data != null)
            {
                return response.Data;
            }

            if (response.IsSuccessStatusCode is false)
            {
                throw new CustomException(response.Message, (int)response.StatusCode);
            }

            return default;
        }

        public async Task<CategorySchemas> GetCategorySchemasAsync(string token, string language, Guid? categoryId)
        {
            var apiRequest = new ApiRequest<RequestModelBase>
            {
                Language = language,
                Data = new RequestModelBase(),
                AccessToken = token,
                EndpointAddress = $"{_settings.Value.CatalogUrl}{ApiConstants.Catalog.CategorySchemasApiEndpoint}/{categoryId}"
            };

            var response = await _apiClientService.GetAsync<ApiRequest<RequestModelBase>, RequestModelBase, CategorySchemas>(apiRequest);

            if(response.IsSuccessStatusCode && response.Data != null)
            {
                return response.Data;
            }

            return default;
        }

        public async Task SaveAsync(string token, string language, Guid? id, CategorySchema categorySchema)
        {
            var requestModel = new ProductCardApiRequestModel
            {
                CategoryId = id,
                Schema = categorySchema.Schema,
                UiSchema = categorySchema.UiSchema
            };

            var apiRequest = new ApiRequest<ProductCardApiRequestModel>
            {
                Language = language,
                Data = requestModel,
                AccessToken = token,
                EndpointAddress = $"{_settings.Value.CatalogUrl}{ApiConstants.Catalog.CategorySchemasApiEndpoint}"
            };

            var response = await _apiClientService.PostAsync<ApiRequest<ProductCardApiRequestModel>, ProductCardApiRequestModel, BaseResponseModel>(apiRequest);

            if (response.IsSuccessStatusCode is false)
            {
                throw new CustomException(response.Message, (int)response.StatusCode);
            }
        }
    }
}
