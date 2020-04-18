using System;

namespace Feature.Client.Models
{
    public class CreateClientModel
    {
        public string Username { get; set; }
        public Guid TenantId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Language { get; set; }
    }
}
