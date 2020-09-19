using System.IO;

namespace Media.Api.Shared.Checksums
{
    public interface IChecksumService
    {
        string GetMd5(Stream stream);
    }
}
