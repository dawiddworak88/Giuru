using Foundation.GenericRepository.Entities;
using System;

namespace Foundation.Catalog.Infrastructure.Categories.Entites
{
    public class CategoryImage : EntityMedia
    {
        public Guid CategoryId { get; set; }
    }
}
