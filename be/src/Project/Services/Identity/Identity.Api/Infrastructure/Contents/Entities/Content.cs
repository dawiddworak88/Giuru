using Foundation.GenericRepository.Entities;
using System.ComponentModel.DataAnnotations;

namespace Identity.Api.Infrastructure.Contents.Entities
{
    public class Content : Entity
    {
        [Required]
        public string Text { get; set; }

        [Required]
        public string Language { get; set; }
    }
}
