using Foundation.GenericRepository.Entities;
using System.ComponentModel.DataAnnotations;

namespace Identity.Api.Infrastructure.Organisations.Entities
{
    public class Organisation : Entity
    {
        [Required]
        public string Key { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string ContactEmail { get; set; }

        [Required]
        public string Domain { get; set; }

        [Required]
        public bool IsSeller { get; set; }

        [Required]
        public string Language { get; set; }
    }
}
