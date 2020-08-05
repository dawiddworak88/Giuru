using Foundation.GenericRepository.Entities;
using System;
using System.ComponentModel.DataAnnotations;

namespace Catalog.Api.Infrastructure.Media.Entities
{
    public class MediaItem : Entity
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string FileName { get; set; }

        [Required]
        public string Url { get; set; }

        [Required]
        public int SizeInBytes { get; set; }

        public int WidthInPixels { get; set; }

        public int HeightInPixels { get; set; }

        public int LengthInSeconds { get; set; }

        [Required]
        public string Extension { get; set; }

        public string ContentType { get; set; }

        public Guid? SchemaId { get; set; }

        public string FormData { get; set; }
    }
}
