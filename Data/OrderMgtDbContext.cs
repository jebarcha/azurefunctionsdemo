using Microsoft.EntityFrameworkCore;
using ProductOrderManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductOrderManagement.Data
{
    public class OrderMgtDbContext : DbContext
    {
        public OrderMgtDbContext(DbContextOptions<OrderMgtDbContext> options) : base(options)
        {
        }
        public DbSet<Product> Product { get; set; }
        public DbSet<ProductOrderManagement.Models.Order> Orders { get; set; }
        public DbSet<ProductOrderManagement.Models.OrderItem> OrderItems { get; set; }
    }
}
