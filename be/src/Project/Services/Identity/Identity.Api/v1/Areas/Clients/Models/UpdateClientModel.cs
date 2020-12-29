using Foundation.Extensions.Models;
using System;

namespace Identity.Api.v1.Areas.Clients.Models
{
    public class UpdateClientModel : BaseServiceModel
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string CommunicationLanguage { get; set; }
    }
}