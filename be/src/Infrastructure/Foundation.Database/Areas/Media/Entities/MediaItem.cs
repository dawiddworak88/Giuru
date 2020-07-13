using Foundation.GenericRepository.Entities;
using System.ComponentModel.DataAnnotations;

namespace Foundation.Database.Areas.Media.Entities
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
        public int Size { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }

        [Required]
        public string Extension { get; set; }

        public string ContentType { get; set; }
    }
}
