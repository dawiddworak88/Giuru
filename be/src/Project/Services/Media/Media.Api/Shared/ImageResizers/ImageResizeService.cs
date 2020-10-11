using SkiaSharp;
using System;
using System.IO;

namespace Media.Api.Shared.ImageResizers
{
    public class ImageResizeService : IImageResizeService
    {
        public byte[] Resize(byte[] fileContents, int maxWidth, int maxHeight)
        {
            using MemoryStream ms = new MemoryStream(fileContents);
            using SKBitmap sourceBitmap = SKBitmap.Decode(ms);

            var ratioX = (double)maxWidth / sourceBitmap.Width;
            var ratioY = (double)maxHeight / sourceBitmap.Height;
            var ratio = Math.Min(ratioX, ratioY);

            var newWidth = (int)(sourceBitmap.Width * ratio);
            var newHeight = (int)(sourceBitmap.Height * ratio);

            using SKBitmap scaledBitmap = sourceBitmap.Resize(new SKImageInfo(newWidth, newHeight), SKFilterQuality.Medium);
            using SKImage scaledImage = SKImage.FromBitmap(scaledBitmap);
            using SKData data = scaledImage.Encode();

            return data.ToArray();
        }
    }
}
