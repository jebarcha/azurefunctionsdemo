using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductOrderManagement.Services.Storage
{
    public interface ICloudStorageService
    {
        Task UploadFileToBlob(IFormFileCollection formFiles, Guid folderId, string containerName);

        Task SendMessageToQueue(string queueName, string message);

    }
}
