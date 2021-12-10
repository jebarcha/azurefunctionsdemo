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
    }
}
