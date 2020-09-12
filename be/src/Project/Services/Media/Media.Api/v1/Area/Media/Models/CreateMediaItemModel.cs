using Foundation.Extensions.Models;
using Microsoft.AspNetCore.Http;

namespace Media.Api.v1.Area.Media.Models
{
    public class CreateMediaItemModel : BaseServiceModel
    {
        public IFormFile File { get; set; }
    }
}
