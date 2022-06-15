using Foundation.GenericRepository.Entities;
using System.ComponentModel.DataAnnotations;

namespace Client.Api.Infrastructure.Managers.Entities
{
    public class ClientAccountManager : Entity
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string Email { get; set; }

        public string PhoneNumber { get; set; }
    }
}
