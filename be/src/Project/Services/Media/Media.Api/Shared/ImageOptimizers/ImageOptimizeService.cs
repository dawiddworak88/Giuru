using ImageMagick;
using System.IO;

namespace Media.Api.Shared.ImageOptimizers
{
    public class ImageOptimizeService : IImageOptimizeService
    {
        public byte[] Optimize(byte[] image)
        {
            using (var stream = new MemoryStream(image))
            {
                var optimizer = new ImageOptimizer();
                optimizer.LosslessCompress(stream);
                return stream.ToArray();
            }            
        }
    }
}
