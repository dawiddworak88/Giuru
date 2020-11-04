namespace Media.Api.Shared.ImageResizers
{
    public interface IImageResizeService
    {
        byte[] Resize(byte[] fileContents, int maxWidth, int maxHeight);
    }
}
