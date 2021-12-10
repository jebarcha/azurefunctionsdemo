using AutoMapper;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ProductOrderManagement.Data;
using ProductOrderManagement.Mapper;
using ProductOrderManagement.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[assembly: FunctionsStartup(typeof(ProductOrderManagement.Startup))]
namespace ProductOrderManagement
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            var mapperConfig = new MapperConfiguration(mapperConfig =>
            {
                mapperConfig.AddProfile(new MappingProfile());
            });

            IMapper mapper = mapperConfig.CreateMapper();
            builder.Services.AddSingleton(mapper);

            builder.Services.AddDbContext<OrderMgtDbContext>(options =>
                options.UseSqlServer(Environment.GetEnvironmentVariable("AzureDatabaseConnectionString")));

            builder.Services.AddScoped<IProductService, ProductService>();
        }

    }
}
