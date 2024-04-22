using Foundation.Extensions.Models;
using System;

namespace Client.Api.ServicesModels.Fields
{
    public class DeleteClientFieldServiceModel : BaseServiceModel
    {
        public Guid? Id { get; set; }
    }
}
