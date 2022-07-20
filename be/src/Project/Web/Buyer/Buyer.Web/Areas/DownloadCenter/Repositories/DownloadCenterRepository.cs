using Buyer.Web.Areas.DownloadCenter.ApiResponseModels;
using Buyer.Web.Areas.DownloadCenter.DomainModels;
using Buyer.Web.Shared.Configurations;
using Foundation.ApiExtensions.Communications;
using Foundation.ApiExtensions.Models.Request;
using Foundation.ApiExtensions.Services.ApiClientServices;
using Foundation.ApiExtensions.Shared.Definitions;
using Foundation.Extensions.Exceptions;
using Foundation.Extensions.ExtensionMethods;
using Foundation.GenericRepository.Paginations;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Buyer.Web.Areas.DownloadCenter.Repositories
{
    public class DownloadCenterRepository : IDownloadCenterRepository
    {
        private readonly IApiClientService apiClientService;
        private readonly IOptions<AppSettings> settings;
        private readonly LinkGenerator linkGenerator;

        public DownloadCenterRepository(
            IApiClientService apiClientService,
            IOptions<AppSettings> settings,
            LinkGenerator linkGenerator)
        {
            this.apiClientService = apiClientService;
            this.settings = settings;
            this.linkGenerator = linkGenerator;
        }

        public async Task<PagedResults<IEnumerable<DownloadCenterItem>>> GetAsync(string token, string language, int pageIndex, int itemsPerPage, string searchTerm, string orderBy)
        {
            var requestModel = new PagedRequestModelBase
            {
                SearchTerm = searchTerm,
                PageIndex = pageIndex,
                ItemsPerPage = itemsPerPage,
                OrderBy = orderBy
            };

            var apiRequest = new ApiRequest<PagedRequestModelBase>
            {
                Language = language,
                Data = requestModel,
                AccessToken = token,
                EndpointAddress = $"{this.settings.Value.DownloadUrl}{ApiConstants.DownloadCenter.DownloadCenterCategoriesApiEndpoint}"
            };

            var response = await this.apiClientService.GetAsync<ApiRequest<PagedRequestModelBase>, PagedRequestModelBase, PagedResults<IEnumerable<DownloadCenterItemResponseModel>>>(apiRequest);

            if (response.IsSuccessStatusCode && response.Data?.Data != null)
            {
                var downloadCenterItems = new List<DownloadCenterItem>();

                foreach(var downloadCenterItem in response.Data.Data.OrEmptyIfNull())
                {
                    var item = new DownloadCenterItem
                    {
                        Id = downloadCenterItem.Id,
                        Name = downloadCenterItem.Name,
                        Subcategories = downloadCenterItem.Subcategories.Select(x => new DownloadCenterItemCategory
                        {
                            Id = x.Id,
                            Name = x.Name,
                            Url = this.linkGenerator.GetPathByAction("Detail", "Category", new { Area = "DownloadCenter", culture = CultureInfo.CurrentUICulture.Name, Id = x.Id })
                        }),
                        LastModifiedDate = downloadCenterItem.LastModifiedDate,
                        CreatedDate = downloadCenterItem.CreatedDate
                    };

                    downloadCenterItems.Add(item);
                }

                return new PagedResults<IEnumerable<DownloadCenterItem>>(response.Data.Total, response.Data.PageSize)
                {
                    Data = downloadCenterItems
                };
            }

            if (!response.IsSuccessStatusCode)
            {
                throw new CustomException(response.Message, (int)response.StatusCode);
            }

            return default;
        }

        public async Task<DownloadCenterCategory> GetAsync(string token, string language, Guid? id)
        {
            var apiRequest = new ApiRequest<RequestModelBase>
            {
                Language = language,
                Data = new RequestModelBase(),
                AccessToken = token,
                EndpointAddress = $"{this.settings.Value.DownloadUrl}{ApiConstants.DownloadCenter.DownloadCenterCategoriesApiEndpoint}/{id}"
            };

            var response = await this.apiClientService.GetAsync<ApiRequest<RequestModelBase>, RequestModelBase, DownloadCenterCategoryResponseModel>(apiRequest);

            if (!response.IsSuccessStatusCode)
            {
                throw new CustomException(response.Message, (int)response.StatusCode);
            }

            if (response.IsSuccessStatusCode && response.Data != null)
            {
                var downloadCenterCategory = new DownloadCenterCategory
                {
                    Id = response.Data.Id,
                    ParentCategoryId = response.Data.ParentCategoryId,
                    ParentCategoryName = response.Data.ParentCategoryName,
                    CategoryName = response.Data.CategoryName,
                    Subcategories = response.Data.Subcategories.OrEmptyIfNull().Select(x => new DownloadCenterItemCategory
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Url = this.linkGenerator.GetPathByAction("Detail", "Category", new { Area = "DownloadCenter", Culture = CultureInfo.CurrentUICulture.Name, Id = x.Id })
                    }),
                    Files = response.Data.Files,
                    LastModifiedDate = response.Data.LastModifiedDate,
                    CreatedDate = response.Data.CreatedDate
                };

                return downloadCenterCategory;
            }

            return default;
        }
    }
}
