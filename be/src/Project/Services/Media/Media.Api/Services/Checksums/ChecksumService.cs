using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Security.Cryptography;

namespace Media.Api.Services.Checksums
{
    public class ChecksumService : IChecksumService
    {
        public string GetMd5(IFormFile file)
        {
            using (var md5 = MD5.Create())
            {
                using (var stream = file.OpenReadStream())
                {
                    var hash = md5.ComputeHash(stream);

                    return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
                }
            }
        }

        public string GetMd5(Stream stream)
        {
            using (var md5 = MD5.Create())
            {
                var hash = md5.ComputeHash(stream);

                return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
            }
        }
    }
}
