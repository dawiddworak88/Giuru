using System;
using System.Collections.Generic;

namespace Ordering.Api.IntegrationEventsModels
{
    public class BasketEventModel
    {
        public Guid? Id { get; set; }
        public IEnumerable<BasketItemEventModel> Items { get; set; }
    }
}
