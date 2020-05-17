using Foundation.Extensions.Models;
using System;

namespace Feature.Client.Models.GetClient
{
    public class GetClientModel : BaseServiceModel
    {
        public Guid? Id { get; set; }
    }
}
