using AutoMapper;
using Newtonsoft.Json;
using ProductOrderManagement.Data;
using ProductOrderManagement.Dtos;
using ProductOrderManagement.Services.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductOrderManagement.Services.Order
{
    public class OrderService : IOrderService
    {
        private readonly ICloudStorageService _cloudStorageService;
        private static string storageQueueName = "orders";
        private readonly IMapper _mapper;
        private readonly OrderMgtDbContext _orderMgtDbContext;
        public OrderService(ICloudStorageService cloudStorageService, IMapper mapper, OrderMgtDbContext orderMgtDbContext)
        {
            _cloudStorageService = cloudStorageService;
            _mapper = mapper;
            _orderMgtDbContext = orderMgtDbContext;
        }
        public async Task SendMessageToQueueStorage(OrderDto data)
        {
            data.OrderId = Guid.NewGuid();
            data.OrderNumber = Guid.NewGuid().ToString("N");
            var serielizedData = JsonConvert.SerializeObject(data);
            await _cloudStorageService.SendMessageToQueue(storageQueueName, serielizedData);
        }

        public async Task CreateOrder(OrderDto orderDto)
        {
            ProductOrderManagement.Models.Order order = _mapper.Map<OrderDto, ProductOrderManagement.Models.Order>(orderDto);
            order.CreatedBy = Guid.NewGuid();
            order.CreatedDate = DateTime.UtcNow;
            order.Status = "INPROCESS";

            foreach (ProductOrderManagement.Models.OrderItem item in order.OrderItems)
            {
                item.OrderId = order.OrderId;
                item.OrderItemId = Guid.NewGuid();
                item.CreatedBy = Guid.NewGuid();
                item.CreatedDate = DateTime.UtcNow;
            }

            await _orderMgtDbContext.Orders.AddAsync(order);
            await _orderMgtDbContext.SaveChangesAsync();
        }
    }
}
