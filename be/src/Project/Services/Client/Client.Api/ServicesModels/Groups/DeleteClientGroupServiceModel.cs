using Foundation.Extensions.Models;
using System;

namespace Client.Api.ServicesModels.Groups
{
    public class DeleteClientGroupServiceModel : BaseServiceModel
    {
        public Guid? Id { get; set; }
    }
}
