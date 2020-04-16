using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Foundation.GenericRepository.Entities
{
    public interface IEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        Guid Id { get; set; }
    }
}
