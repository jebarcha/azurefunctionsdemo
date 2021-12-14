using AutoMapper;
using Microsoft.AspNetCore.Http;
using ProductOrderManagement.Data;
using ProductOrderManagement.Dtos;
using ProductOrderManagement.Models;
using ProductOrderManagement.Services.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductOrderManagement.Services
{
    public class ProductService : IProductService
    {
        private readonly OrderMgtDbContext _orderManagementDbContext;
        private readonly IMapper _mapper;
        private readonly ICloudStorageService _cloudStorageService;
        private static string productContaierName = "products";
        public ProductService(OrderMgtDbContext orderManagementDbContext,
                IMapper mapper,
                ICloudStorageService cloudStorageService)
        {
            _orderManagementDbContext = orderManagementDbContext;
            _mapper = mapper;
            _cloudStorageService = cloudStorageService;
        }

        public async Task<Guid> CreateProduct(ProductDto productDto)
        {
            productDto.ProductId = Guid.NewGuid();
            Product product = _mapper.Map<ProductDto, Product>(productDto);
            product.CreatedBy = Guid.NewGuid();
            product.CreatedDate = DateTime.UtcNow;
            await _orderManagementDbContext.Product.AddAsync(product);
            await _orderManagementDbContext.SaveChangesAsync();
            return product.ProductId;
        }

        public async Task UploadProductImages(IFormFileCollection files, Guid productId)
        {
            await _cloudStorageService.UploadFileToBlob(files, productId, productContaierName);   
        }
    }
}
