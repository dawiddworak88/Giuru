using Foundation.GenericRepository.Entities;
using System.ComponentModel.DataAnnotations;

namespace Client.Api.Infrastructure.Roles.Entities
{
    public class ClientRole : Entity
    {
        [Required]
        public string Name { get; set; }
    }
}
