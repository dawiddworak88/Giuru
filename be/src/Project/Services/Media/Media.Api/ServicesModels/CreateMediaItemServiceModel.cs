using Foundation.Extensions.Models;
using Microsoft.AspNetCore.Http;

namespace Media.Api.ServicesModels
{
    public class CreateMediaItemServiceModel : BaseServiceModel
    {
        public IFormFile File { get; set; }
    }
}
