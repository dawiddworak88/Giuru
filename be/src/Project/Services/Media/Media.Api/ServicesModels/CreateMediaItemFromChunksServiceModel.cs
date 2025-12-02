using Foundation.Extensions.Models;
using System;

namespace Media.Api.ServicesModels
{
    public class CreateMediaItemFromChunksServiceModel : BaseServiceModel
    {
        public Guid? UploadId { get; set; }
        public string Filename { get; set; }
    }
}
