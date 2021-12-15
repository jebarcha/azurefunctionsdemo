using ProductOrderManagement.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductOrderManagement.Services.Order
{
    public interface IOrderService
    {
        Task SendMessageToQueueStorage(OrderDto data);
        Task CreateOrder(OrderDto orderDto);
    }
}
