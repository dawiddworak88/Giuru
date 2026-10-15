using System;
using System.Collections.Generic;

namespace Basket.Api.RepositoriesModels
{
    public class BasketRepositoryModel
    {
        public Guid? Id { get; set; }

        /// <summary>
        /// The client the basket was last saved, and so priced, for - null when the app saving it does not name one.
        /// </summary>
        public Guid? ClientId { get; set; }
        public string DiscountCode { get; set; }
        public IEnumerable<BasketItemRepositoryModel> Items { get; set; }
    }
}
