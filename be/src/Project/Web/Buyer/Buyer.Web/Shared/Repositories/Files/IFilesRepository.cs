﻿using System;
using System.Threading.Tasks;

namespace Buyer.Web.Shared.Repositories.Files
{
    public interface IFilesRepository
    {
        Task<Guid> SaveAsync(string token, string language, byte[] file, string filename);
    }
}
