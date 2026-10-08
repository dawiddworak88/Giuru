using System;
using System.Collections.Generic;

namespace Basket.Api.RepositoriesModels
{
    public class BasketRepositoryModel
    {
        public Guid? Id { get; set; }
        public string DiscountCode { get; set; }

        /// <summary>
        /// The client the stored code was last verified for. Null when there is no code, or for a basket saved before this
        /// existed. It guards client switches; it is metadata set by the trusted web callers, not authorisation evidence.
        /// </summary>
        public Guid? DiscountCodeClientId { get; set; }

        /// <summary>
        /// Generated on every write and stored in the same value as the code, the client and the lines, so that a checkout
        /// can prove it is ordering the exact snapshot that was validated. Null for a basket saved before this existed.
        /// </summary>
        public Guid? BasketVersion { get; set; }
        public IEnumerable<BasketItemRepositoryModel> Items { get; set; }
    }
}
