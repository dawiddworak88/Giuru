using Media.Api.Definitions;
using SkiaSharp;
using System;
using System.IO;

namespace Media.Api.Services.ImageResizers
{
    public class ImageResizeService : IImageResizeService
    {
        public byte[] Compress(byte[] fileContents, string mimeType, int quality, int? maxWidth, int? maxHeight, string? extension)
        {
            using var ms = new MemoryStream(fileContents);
            using var sourceBitmap = SKBitmap.Decode(ms);

            if (maxWidth.HasValue || maxHeight.HasValue)
            {
                var ratioX = (double)maxWidth / sourceBitmap.Width;
                var ratioY = (double)maxHeight / sourceBitmap.Height;
                var ratio = Math.Min(ratioX, ratioY);

                var newWidth = (int)(sourceBitmap.Width * ratio);
                var newHeight = (int)(sourceBitmap.Height * ratio);

                using SKBitmap scaledBitmap = sourceBitmap.Resize(new SKImageInfo(newWidth, newHeight), SKFilterQuality.High);
                using var scaledImage = SKImage.FromBitmap(scaledBitmap);

                return this.Encode(scaledBitmap, mimeType, quality, extension);
            }

            return this.Encode(sourceBitmap, mimeType, quality, extension);
        }

        private byte[] Encode(SKBitmap bitmap, string mimeType, int quality, string extension)
        {
            if (string.IsNullOrWhiteSpace(extension) is false)
            {
                switch (extension)
                {
                    case MediaConstants.Extensions.Jpeg:
                        {
                            return bitmap.Encode(SKEncodedImageFormat.Jpeg, quality).ToArray();
                        }

                    case MediaConstants.Extensions.Jpg:
                        {
                            return bitmap.Encode(SKEncodedImageFormat.Jpeg, quality).ToArray();
                        }

                    case MediaConstants.Extensions.Png:
                        {
                            return bitmap.Encode(SKEncodedImageFormat.Png, quality).ToArray();
                        }

                    case MediaConstants.Extensions.Webp:
                        {
                            return bitmap.Encode(SKEncodedImageFormat.Webp, quality).ToArray();
                        }

                    default:
                        {
                            throw new NotSupportedException();
                        }
                }
            }
            else
            {
                switch (mimeType)
                {
                    case MediaConstants.MimeTypes.Jpeg:
                        {
                            return bitmap.Encode(SKEncodedImageFormat.Jpeg, quality).ToArray();
                        }

                    case MediaConstants.MimeTypes.Png:
                        {
                            return bitmap.Encode(SKEncodedImageFormat.Png, quality).ToArray();
                        }

                    case MediaConstants.MimeTypes.Webp:
                        {
                            return bitmap.Encode(SKEncodedImageFormat.Webp, quality).ToArray();
                        }

                    default:
                        {
                            throw new NotSupportedException();
                        }
                }
            }
        }
    }
}
