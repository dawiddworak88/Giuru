using System;

namespace Foundation.Extensions.Models
{
    public class BaseServiceModel
    {
        public string Language { get; set; }
        public string Username { get; set; }
        public Guid? OrganisationId { get; set; }
    }
}
