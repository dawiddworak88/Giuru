using Foundation.Extensions.Models;
using System;

namespace Client.Api.ServicesModels.Groups
{
    public class DeleteGroupServiceModel : BaseServiceModel
    {
        public Guid? Id { get; set; }
    }
}
