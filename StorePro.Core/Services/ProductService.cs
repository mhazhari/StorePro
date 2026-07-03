using AutoMapper;
using StorePro.Data.Repositories;
using StorePro.Models.Dtos;
using StorePro.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StorePro.Core.Services
{
    public class ProductService : IProductService
    {
        private readonly IRepository<TblItm> _productRepository;
        private readonly IRepository<TblItmOnUnit> _unitRepository;
        private readonly IMapper _mapper;

        public ProductService(
            IRepository<TblItm> productRepository,
            IRepository<TblItmOnUnit> unitRepository,
            IMapper mapper)
        {
            _productRepository = productRepository;
            _unitRepository = unitRepository;
            _mapper = mapper;
        }

        public async Task<ProductDto> GetProductByIdAsync(Guid productId)
        {
            var product = await _productRepository.GetByIdAsync(productId);
            if (product == null) return null;

            return _mapper.Map<ProductDto>(product);
        }

        public async Task<ProductDto> GetProductByBarcodeAsync(string barcode)
        {
            var product = await _productRepository.SingleOrDefaultAsync(p => p.PrivateCode == barcode);
            if (product == null) return null;

            return _mapper.Map<ProductDto>(product);
        }

        public async Task<IEnumerable<ProductDto>> GetAllProductsAsync()
        {
            var products = await _productRepository.FindAsync(p => p.IsBlocked != true);
            return _mapper.Map<IEnumerable<ProductDto>>(products);
        }

        public async Task<IEnumerable<SearchProductDto>> SearchProductsAsync(string searchTerm)
        {
            var products = await _productRepository.FindAsync(p =>
                p.IsBlocked != true &&
                (p.ITm_Name.Contains(searchTerm) ||
                 p.PrivateCode.Contains(searchTerm) ||
                 p.CategoryName.Contains(searchTerm))
            );

            return _mapper.Map<IEnumerable<SearchProductDto>>(products);
        }

        public async Task<IEnumerable<LowStockReportDto>> GetLowStockProductsAsync()
        {
            var products = await _productRepository.FindAsync(p => p.IsBlocked != true);

            var lowStockProducts = products
                .Where(p => p.StorSalesStatic != null && p.StorMsgMin != null && p.StorSalesStatic < p.StorMsgMin)
                .Select(p => new LowStockReportDto
                {
                    ProductID = p.ProductID,
                    ITm_Name = p.ITm_Name,
                    CurrentStock = p.StorSalesStatic,
                    MinimumStock = p.StorMsgMin,
                    MaximumStock = p.StorMsgMax,
                    Status = p.StorSalesStatic < p.StorMsgMin ? "تحت الحد الأدنى" : "آمن"
                })
                .ToList();

            return lowStockProducts;
        }

        public async Task<ProductDto> CreateProductAsync(CreateProductDto dto)
        {
            var product = _mapper.Map<TblItm>(dto);
            product.ProductID = Guid.NewGuid();
            product.Ins_Date = DateTime.Now;

            await _productRepository.AddAsync(product);
            await _productRepository.SaveChangesAsync();

            return _mapper.Map<ProductDto>(product);
        }

        public async Task<ProductDto> UpdateProductAsync(Guid productId, UpdateProductDto dto)
        {
            var product = await _productRepository.GetByIdAsync(productId);
            if (product == null) return null;

            _mapper.Map(dto, product);
            product.Upd_Date = DateTime.Now;

            _productRepository.Update(product);
            await _productRepository.SaveChangesAsync();

            return _mapper.Map<ProductDto>(product);
        }

        public async Task<bool> DeleteProductAsync(Guid productId)
        {
            var product = await _productRepository.GetByIdAsync(productId);
            if (product == null) return false;

            product.IsBlocked = true;
            _productRepository.Update(product);

            return await _productRepository.SaveChangesAsync();
        }
    }
}