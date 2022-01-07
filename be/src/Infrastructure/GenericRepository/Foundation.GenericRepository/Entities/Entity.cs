using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Foundation.GenericRepository.Entities
{
    public class Entity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        public bool IsActive { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }

        [Required]
        public DateTime LastModifiedDate { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }
    }
}
