using System;
using System.ComponentModel.DataAnnotations;

namespace Foundation.GenericRepository
{
    public interface IEntity
    {
        [Required]
        public Guid Id { get; set; }
    }
}
