using Foundation.ApiExtensions.Communications;
using Foundation.ApiExtensions.Models.Request;
using Foundation.ApiExtensions.Models.Response;
using Foundation.ApiExtensions.Services.ApiClientServices;
using Foundation.ApiExtensions.Shared.Definitions;
using Foundation.Extensions.Exceptions;
using Foundation.GenericRepository.Paginations;
using Microsoft.Extensions.Options;
using Seller.Web.Areas.Products.ApiResponseModels;
using Seller.Web.Areas.Products.DomainModels;
using Seller.Web.Areas.Products.Repositories;
using Seller.Web.Shared.Configurations;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Categories.Repositories
{
    public class CategoriesRepository : ICategoriesRepository
    {
        private readonly IApiClientService apiClientService;
        private readonly IOptions<AppSettings> settings;

        public CategoriesRepository(IApiClientService apiClientService,
            IOptions<AppSettings> settings)
        {
            this.apiClientService = apiClientService;
            this.settings = settings;
        }

        public async Task DeleteAsync(string token, string language, Guid? id)
        {
            var deleteRequestModel = new RequestModelBase
            {
                Id = id,
                Language = language
            };

            var apiRequest = new ApiRequest<RequestModelBase>
            { 
                Data = deleteRequestModel,
                AccessToken = token,
                EndpointAddress = $"{this.settings.Value.CatalogUrl}{ApiConstants.Catalog.CategoriesApiEndpoint}"
            };

            var response = await this.apiClientService.DeleteAsync<ApiRequest<RequestModelBase>, RequestModelBase, BaseResponseModel>(apiRequest);

            if (!response.IsSuccessStatusCode)
            {
                throw new CustomException(response.Message, (int)response.StatusCode);
            }
        }

        public async Task<PagedResults<IEnumerable<Category>>> GetCategoriesAsync(string token, string language, string searchTerm, int pageIndex, int itemsPerPage)
        {
            var categoriesRequestModel = new PagedRequestModelBase
            {
                Language = language,
                SearchTerm = searchTerm,
                PageIndex = pageIndex,
                ItemsPerPage = itemsPerPage
            };

            var apiRequest = new ApiRequest<PagedRequestModelBase>
            {
                Data = categoriesRequestModel,
                AccessToken = token,
                EndpointAddress = $"{this.settings.Value.CatalogUrl}{ApiConstants.Catalog.CategoriesApiEndpoint}"
            };

            var response = await this.apiClientService.GetAsync<ApiRequest<PagedRequestModelBase>, PagedRequestModelBase, PagedResults<IEnumerable<CategoryResponseModel>>>(apiRequest);

            if (response.IsSuccessStatusCode && response.Data != null)
            {
                var categories = new List<Category>();

                foreach (var categoryResponse in response.Data.Data)
                {
                    var category = new Category
                    {
                        Id = categoryResponse.Id,
                        Name = categoryResponse.Name,
                        ParentId = categoryResponse.ParentId,
                        ParentCategoryName = categoryResponse.ParentCategoryName,
                        Level = categoryResponse.Level,
                        IsLeaf = categoryResponse.IsLeaf,
                        Order = categoryResponse.Order,
                        ThumbnailMediaId = categoryResponse.ThumbnailMediaId,
                        LastModifiedDate = categoryResponse.LastModifiedDate,
                        CreatedDate = categoryResponse.CreatedDate
                    };

                    categories.Add(category);
                }

                return new PagedResults<IEnumerable<Category>>(response.Data.Total, response.Data.PageSize)
                {
                    Data = categories
                };
            }

            if (!response.IsSuccessStatusCode)
            {
                throw new CustomException(response.Message, (int)response.StatusCode);
            }

            return default;
        }
    }
}
