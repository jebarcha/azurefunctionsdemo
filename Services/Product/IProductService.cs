using Microsoft.AspNetCore.Http;
using ProductOrderManagement.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductOrderManagement.Services
{
    public interface IProductService
    {
        Task<Guid> CreateProduct(ProductDto product);

        Task UploadProductImages(IFormFileCollection files, Guid productId);
    }
}
