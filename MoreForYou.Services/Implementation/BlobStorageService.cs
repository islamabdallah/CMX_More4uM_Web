using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Configuration;
using MoreForYou.Services.Contracts;
using MoreForYou.Services.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace MoreForYou.Services.Implementation
{
    public class BlobStorageService : IBlobStorageService
    {
        private readonly IConfiguration _configuration;

        public BlobStorageService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<string> UploadAsync(Stream fileStream, string fileName, string contentType, string containerName)
        {

            string _storageConnectionString = _configuration["AzureStorage:conStr"];

            var container = new BlobContainerClient(_storageConnectionString, containerName);
            var blob = container.GetBlobClient(fileName);
            await blob.DeleteIfExistsAsync(DeleteSnapshotsOption.IncludeSnapshots);
            await blob.UploadAsync(fileStream, new BlobHttpHeaders { ContentType = contentType });
            return blob.Uri.ToString();
        }
        private async Task<string> GetBlobSASTOkenByFile(string fileName, string blobContainerName)
        {
            try
            {
                var azureStorageAccount = _configuration["AzureStorage:AzureAccount"];
                var azureStorageAccessKey = _configuration["AzureStorage:AccessKey"];
                Azure.Storage.Sas.BlobSasBuilder blobSasBuilder = new Azure.Storage.Sas.BlobSasBuilder()
                {
                    BlobContainerName = blobContainerName,
                    BlobName = fileName,
                    ExpiresOn = DateTimeOffset.Now.AddDays(1),
                    Resource = "b"
                };
                blobSasBuilder.SetPermissions(Azure.Storage.Sas.BlobSasPermissions.Read);
                var sasToken = blobSasBuilder.ToSasQueryParameters(new StorageSharedKeyCredential(azureStorageAccount,
                    azureStorageAccessKey)).ToString();
                return sasToken;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string retriveDocuementURLFromAzureBlob(string fileName, string blobContainerName)
        {
            try
            {
                var sasToken = GetBlobSASTOkenByFile(fileName, blobContainerName).Result;
                string documentURL = null;
                if (sasToken != null)
                {
                    documentURL = CommanData.AzureBlobUrl + CommanData.AzureDocumentsFolder + "/" + fileName + "?" + sasToken;
                }

                return documentURL;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
