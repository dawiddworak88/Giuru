using Foundation.GenericRepository.Entities;
using System.ComponentModel.DataAnnotations;

namespace Foundation.TenantDatabase.Areas.Clients.Entities
{
    public class Client : Entity
    {
        [Required]
        public string Host { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Language { get; set; }
    }
}
