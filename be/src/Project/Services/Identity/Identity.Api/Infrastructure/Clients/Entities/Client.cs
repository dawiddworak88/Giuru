using Foundation.GenericRepository.Entities;
using System.ComponentModel.DataAnnotations;

namespace Identity.Api.Infrastructure.Clients.Entities
{
    public class Client : Entity
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Language { get; set; }

        [Required]
        public bool IsWorkflowEnabled { get; set; }
    }
}
