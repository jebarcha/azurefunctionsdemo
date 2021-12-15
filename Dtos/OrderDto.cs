using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductOrderManagement.Dtos
{
    public class OrderDto
    {
        public Guid OrderId { get; set; }
        public string OrderNumber { get; set; }
        public Guid CustomerId { get; set; }
        public IEnumerable<OrderItemDto> OrderItems { get; set; }
    }
}
