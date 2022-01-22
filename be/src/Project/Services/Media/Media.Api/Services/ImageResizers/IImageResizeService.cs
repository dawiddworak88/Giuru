namespace Media.Api.Services.ImageResizers
{
    public interface IImageResizeService
    {
        byte[] Compress(byte[] fileContents, string mimeType, int quality, int? maxWidth, int? maxHeight, string? extension);

    }
}
