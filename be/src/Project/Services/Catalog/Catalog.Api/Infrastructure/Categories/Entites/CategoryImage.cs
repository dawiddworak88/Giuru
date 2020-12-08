using Foundation.GenericRepository.Entities;
using System;

namespace Catalog.Api.Infrastructure.Categories.Entites
{
    public class CategoryImage : EntityMedia
    {
        public Guid CategoryId { get; set; }
    }
}
