using Foundation.Extensions.Models;
using System;

namespace Identity.Api.v1.Areas.Clients.Models
{
    public class DeleteClientModel : BaseServiceModel
    {
        public Guid? Id { get; set; }
    }
}
