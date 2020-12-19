using Foundation.Extensions.Models;
using System;

namespace Media.Api.v1.Area.Media.Models
{
    public class GetMediaItemsByIdModel : BaseServiceModel
    {
        public Guid? Id { get; set; }
    }
}
