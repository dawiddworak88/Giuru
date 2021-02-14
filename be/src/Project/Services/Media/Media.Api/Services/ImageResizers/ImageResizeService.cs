using Media.Api.Definitions;
using SkiaSharp;
using System;
using System.IO;

namespace Media.Api.Services.ImageResizers
{
    public class ImageResizeService : IImageResizeService
    {
        public byte[] Resize(byte[] fileContents, int maxWidth, int maxHeight, bool optimize, string mimeType)
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

            if (optimize)
            {
                return this.OptimizeImage(mimeType, scaledImage);
            }

            using SKData data = scaledImage.Encode();
            return data.ToArray();
        }

        public byte[] Optimize(byte[] fileContents, string mimeType)
        {
            using MemoryStream ms = new MemoryStream(fileContents);
            using SKBitmap sourceBitmap = SKBitmap.Decode(ms);
            using SKImage image = SKImage.FromBitmap(sourceBitmap);

            return this.OptimizeImage(mimeType, image);
        }

        private byte[] OptimizeImage(string mimeType, SKImage image)
        {
            switch (mimeType)
            {
                case MediaConstants.MimeTypes.Jpeg:
                {
                    return image.Encode(SKEncodedImageFormat.Jpeg, MediaConstants.ImageConversion.ImageQuality).ToArray();
                }

                case MediaConstants.MimeTypes.Png:
                {
                    return image.Encode(SKEncodedImageFormat.Png, MediaConstants.ImageConversion.ImageQuality).ToArray();
                }

                default:
                {
                    return image.Encode().ToArray();
                }
            }
        }
    }
}
