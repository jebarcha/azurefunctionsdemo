using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ProductOrderManagement.Dtos;
using ProductOrderManagement.Services;

namespace ProductOrderManagement
{
    public class Products
    {
        private readonly IProductService _productService;
        public Products(IProductService productService)
        {
            _productService = productService;
        }

        [FunctionName("CreateProduct")]
        public async Task<IActionResult> CreateProduct(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "products")] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            ProductDto data = JsonConvert.DeserializeObject<ProductDto>(requestBody);
            Guid productId = await _productService.CreateProduct(data);

            return new CreatedResult(String.Empty, productId);
        }

        [FunctionName("UploadProductImage")]
        public async Task<IActionResult> UploadProductImage(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "products/{productId}/upload")] HttpRequest req,
            ILogger log, Guid productId)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            IFormFileCollection formFiles = req.Form.Files;
            await _productService.UploadProductImages(formFiles, productId);

            return new OkResult();
        }

        [FunctionName("ProductBlobCreated")]
        public static void ProductBlobCreated([BlobTrigger("products/{name}", Connection = "AzureWebJobsStorage")] Stream myBlob, string name, ILogger log)
        {
            string[] blobName = name.Split('/');
            log.LogInformation("C# HTTP trigger function processed a request.");


        }


    }
}
