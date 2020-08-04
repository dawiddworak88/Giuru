using System;

namespace Foundation.GenericRepository.Entities
{
    public interface IEntity
    {
        Guid Id { get; set; }
        bool IsActive { get; set; }
        DateTime LastModifiedDate { get; set; }
        DateTime CreatedDate { get; set; }
    }
}
