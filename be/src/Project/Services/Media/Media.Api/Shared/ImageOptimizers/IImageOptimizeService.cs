namespace Media.Api.Shared.ImageOptimizers
{
    public interface IImageOptimizeService
    {
        byte[] Optimize(byte[] image);
    }
}
