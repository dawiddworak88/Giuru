using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Basket.Api.Infrastructure.Entities
{
    public class BasketItems
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [Timestamp]
        public byte[] RowVersion { get; set; }
        [Required]
        public Guid? ProductId { get; set; }
        [Required]
        public string ProductSku { get; set; }
        [Required]
        public string ProductName { get; set; }
        public string PictureUrl { get; set; }
        [Required]
        public int Quantity { get; set; }
        public string ExternalReference { get; set; }
        public DateTime? DeliveryFrom { get; set; }
        public DateTime? DeliveryTo { get; set; }
        public string MoreInfo { get; set; }
        [Required]
        public Guid? BasketId { get; set; }
        [Required]
        public Guid? OwnerId { get; set; }
    }
}
