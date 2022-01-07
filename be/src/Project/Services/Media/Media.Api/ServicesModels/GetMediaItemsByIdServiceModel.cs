using Foundation.Extensions.Models;
using System;

namespace Media.Api.ServicesModels
{
    public class GetMediaItemsByIdServiceModel : BaseServiceModel
    {
        public Guid? Id { get; set; }
    }
}
