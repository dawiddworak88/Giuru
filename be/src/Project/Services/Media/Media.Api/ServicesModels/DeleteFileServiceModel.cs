using Foundation.Extensions.Models;
using System;

namespace Media.Api.ServicesModels
{
    public class DeleteFileServiceModel : BaseServiceModel
    {
        public Guid? MediaId { get; set; }
    }
}
