namespace Media.Api.Services.ImageResizers
{
    public interface IImageResizeService
    {
        byte[] Resize(byte[] fileContents, int maxWidth, int maxHeight, bool optimize, string mimeType);
        byte[] Optimize(byte[] fileContents, string mimeType);

    }
}
