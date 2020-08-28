using Foundation.GenericRepository.Entities;
using System.ComponentModel.DataAnnotations;

namespace Identity.Api.Infrastructure.Clients.Entities
{
    public class Client : Entity
    {
        [Required]
        public string Name { get; set; }

        public string Domain { get; set; }

        public string Key { get; set; }

        [Required]
        public string Language { get; set; }
    }
}
