using StorePro.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StorePro.Core.Services
{
    public interface IProductService
    {
        Task<ProductDto> GetProductByIdAsync(Guid productId);
        Task<ProductDto> GetProductByBarcodeAsync(string barcode);
        Task<IEnumerable<ProductDto>> GetAllProductsAsync();
        Task<IEnumerable<SearchProductDto>> SearchProductsAsync(string searchTerm);
        Task<IEnumerable<LowStockReportDto>> GetLowStockProductsAsync();
        Task<ProductDto> CreateProductAsync(CreateProductDto dto);
        Task<ProductDto> UpdateProductAsync(Guid productId, UpdateProductDto dto);
        Task<bool> DeleteProductAsync(Guid productId);
    }
}