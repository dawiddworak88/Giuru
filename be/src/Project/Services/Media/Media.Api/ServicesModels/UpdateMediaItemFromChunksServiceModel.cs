using Foundation.Extensions.Models;
using System;

namespace Media.Api.ServicesModels
{
    public class UpdateMediaItemFromChunksServiceModel : BaseServiceModel
    {
        public Guid? Id { get; set; }
        public Guid UploadId { get; set; }
        public string Filename { get; set; }
    }
}
