using System;
using System.Collections.Generic;

namespace Basket.Api.RepositoriesModels
{
    public class BasketRepositoryModel
    {
        public Guid? Id { get; set; }
        public IEnumerable<BasketItemRepositoryModel> Items { get; set; }
    }
}
