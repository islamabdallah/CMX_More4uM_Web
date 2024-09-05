using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreForYou.Services.Contracts
{
    public interface IBlobStorageService
    {
        Task<string> UploadAsync(Stream fileStream, string fileName, string contentType, string blobContainerName);
      
        public string retriveDocuementURLFromAzureBlob(string fileName, string blobContainerName);

    }
}
