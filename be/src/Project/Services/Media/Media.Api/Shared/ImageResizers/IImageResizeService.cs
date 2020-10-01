using SkiaSharp;

namespace Media.Api.Shared.ImageResizers
{
    public interface IImageResizeService
    {
        byte[] Resize(byte[] fileContents, int maxWidth, int maxHeight, SKFilterQuality quality = SKFilterQuality.Medium);
    }
}
