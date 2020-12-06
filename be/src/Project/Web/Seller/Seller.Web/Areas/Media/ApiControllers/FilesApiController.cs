using Foundation.ApiExtensions.Controllers;
using Foundation.ApiExtensions.Definitions;
using Foundation.Extensions.Services.MediaServices;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Seller.Web.Areas.Media.ApiRequestModels;
using Seller.Web.Areas.Media.Repositories;
using Seller.Web.Shared.Configurations;
using System.Globalization;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Media.ApiControllers
{
    [Area("Media")]
    public class FilesApiController : BaseApiController
    {
        private readonly IFilesRepository filesRepository;
        private readonly IMediaHelperService mediaHelperService;
        private readonly IOptionsMonitor<AppSettings> settings;

        public FilesApiController(
            IFilesRepository filesRepository,
            IMediaHelperService mediaHelperService,
            IOptionsMonitor<AppSettings> settings)
        {
            this.filesRepository = filesRepository;
            this.mediaHelperService = mediaHelperService;
            this.settings = settings;
        }

        [HttpPost]
        [DisableRequestSizeLimit]
        public async Task<IActionResult> Post([FromForm] UploadMediaRequestModel model)
        {
            if (model.File == null)
            {
                return this.StatusCode((int)HttpStatusCode.UnprocessableEntity);
            }

            using (var ms = new MemoryStream())
            {
                await model.File.CopyToAsync(ms);

                var fileId = await this.filesRepository.SaveAsync(
                    await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName),
                    CultureInfo.CurrentUICulture.Name,
                    ms.ToArray(),
                    model.File.FileName);

                return this.StatusCode((int)HttpStatusCode.OK, new { Id = fileId, Url = this.mediaHelperService.GetFileUrl(this.settings.CurrentValue.MediaUrl, fileId, true) });
            }
        }
    }
}
