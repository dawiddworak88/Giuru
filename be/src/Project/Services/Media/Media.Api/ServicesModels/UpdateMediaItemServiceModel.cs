using Foundation.Extensions.Models;
using Microsoft.AspNetCore.Http;
using System;

namespace Media.Api.ServicesModels
{
    public class UpdateMediaItemServiceModel : BaseServiceModel
    {
        public Guid? Id { get; set; }
        public IFormFile File { get; set; }
    }
}
