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
using ProductOrderManagement.Services.Order;

namespace ProductOrderManagement
{
    public class Order
    {
        private readonly IOrderService _orderService;
        public Order(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [FunctionName("CreateOrderToQueue")]
        public async Task<IActionResult> CreateOrderToQueue(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "orders/{customerId}/queue")] HttpRequest req,
            ILogger log, Guid customerId)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            OrderDto data = JsonConvert.DeserializeObject<OrderDto>(requestBody);
            data.CustomerId = customerId;
            await _orderService.SendMessageToQueueStorage(data);

            return new AcceptedResult();
        }

        [FunctionName("ProcessOrderQueue")]
        public async Task Run([QueueTrigger("orders", Connection = "StorageAccountConnectionString")] string myQueueItem, ILogger log)
        {
            log.LogInformation($"C# Queue trigger function processed: {myQueueItem}");

            OrderDto data = JsonConvert.DeserializeObject<OrderDto>(myQueueItem);
            await _orderService.CreateOrder(data);
        }


    }


}
