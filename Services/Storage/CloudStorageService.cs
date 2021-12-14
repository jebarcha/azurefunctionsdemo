using Microsoft.AspNetCore.Http;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductOrderManagement.Services.Storage
{
    public class CloudStorageService : ICloudStorageService
    {
        private readonly CloudStorageAccount _cloudStorageAccount;
        public CloudStorageService(string connectionString)
        {
            _cloudStorageAccount = CloudStorageAccount.Parse(connectionString);
        }
        public async Task UploadFileToBlob(IFormFileCollection formFiles, Guid folderId, string containerName)
        {
            foreach (IFormFile file in formFiles)
            {
                Stream stream = file.OpenReadStream();
                CloudBlobClient blobClient = _cloudStorageAccount.CreateCloudBlobClient();
                CloudBlobContainer cloudBlobContainer = blobClient.GetContainerReference(containerName);
                if (await cloudBlobContainer.CreateIfNotExistsAsync())
                {
                    await cloudBlobContainer.SetPermissionsAsync(new
                        BlobContainerPermissions
                    {
                        PublicAccess = BlobContainerPublicAccessType.Blob
                    });
                }
                await cloudBlobContainer.CreateIfNotExistsAsync();
                CloudBlockBlob cloudBlockBlob = cloudBlobContainer
                    .GetBlockBlobReference(Path.Combine(folderId.ToString(), file.FileName));
                await cloudBlockBlob.UploadFromStreamAsync(stream);
            }
        }
    }
}
