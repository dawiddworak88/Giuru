using Buyer.Web.Areas.News.DomainModels;
using Buyer.Web.Areas.Products.DomainModels;
using Buyer.Web.Shared.Repositories.News;
using Foundation.ApiExtensions.Controllers;
using Foundation.ApiExtensions.Definitions;
using Foundation.GenericRepository.Paginations;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System;
using System.Globalization;
using System.Net;
using System.Threading.Tasks;
using Buyer.Web.Shared.DomainModels.Media;
using System.Linq;
using Buyer.Web.Shared.Repositories.Media;
using Buyer.Web.Shared.Definitions.Files;
using Foundation.Media.Services.MediaServices;
using Foundation.Extensions.ExtensionMethods;

namespace Buyer.Web.Areas.News.ApiControllers
{
    [Area("News")]
    public class NewsApiController : BaseApiController
    {
        private readonly INewsRepository newsRepository;
        private readonly IMediaItemsRepository mediaRepository;
        private readonly IMediaService mediaService;

        public NewsApiController(
            INewsRepository newsRepository,
            IMediaItemsRepository mediaRepository,
            IMediaService mediaService)
        {
            this.newsRepository = newsRepository;
            this.mediaRepository = mediaRepository;
            this.mediaService = mediaService;
        }

        [HttpGet]
        public async Task<IActionResult> Get(int pageIndex, int itemsPerPage)
        {
            var token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName);
            var language = CultureInfo.CurrentUICulture.Name;

            var categories = await this.newsRepository.GetNewsItemsAsync(
                token, language, pageIndex, itemsPerPage, null, $"{nameof(NewsItem.CreatedDate)} desc");

            return this.StatusCode((int)HttpStatusCode.OK, categories);
        }

        [HttpGet]
        public async Task<IActionResult> GetFiles(Guid? id, string searchTerm, int pageIndex, int itemsPerPage)
        {
            var token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName);
            var language = CultureInfo.CurrentUICulture.Name;

            var newsItemFiles = await this.newsRepository.GetNewsItemFilesAsync(token, language, id, pageIndex, itemsPerPage, searchTerm, $"{nameof(ProductFile.CreatedDate)} desc");

            var filesModel = new List<FileItem>();
            var filesIds = newsItemFiles.Data.Select(x => x.Id);

            if (newsItemFiles is not null && filesIds.Any())
            {
                var files = await this.mediaRepository.GetMediaItemsAsync(token, language, filesIds, FilesConstants.DefaultPageIndex, FilesConstants.DefaultPageSize);

                foreach (var file in files.OrEmptyIfNull())
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
