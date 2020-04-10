using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Foundation.GenericRepository
{
    public interface IEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        Guid Id { get; set; }
    }
}
