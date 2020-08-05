using Foundation.GenericRepository.Entities;
using System.ComponentModel.DataAnnotations;

namespace Catalog.Api.Infrastructure.Sellers.Entities
{
    public class Seller : Entity
    {
        [Required]
        public string Name { get; set; }
    }
}
