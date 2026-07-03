using Microsoft.AspNetCore.Mvc;
using StorePro.Core.Services;
using StorePro.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StorePro.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        /// <summary>
        /// الحصول على جميع المنتجات
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetAll()
        {
            var products = await _productService.GetAllProductsAsync();
            return Ok(products);
        }

        /// <summary>
        /// الحصول على منتج برقم المعرف
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDto>> GetById(Guid id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
                return NotFound(new { message = "المنتج غير موجود" });

            return Ok(product);
        }

        /// <summary>
        /// البحث عن منتج برمز المخزون
        /// </summary>
        [HttpGet("barcode/{barcode}")]
        public async Task<ActionResult<ProductDto>> GetByBarcode(string barcode)
        {
            var product = await _productService.GetProductByBarcodeAsync(barcode);
            if (product == null)
                return NotFound(new { message = "المنتج غير موجود" });

            return Ok(product);
        }

        /// <summary>
        /// البحث عن منتجات
        /// </summary>
        [HttpGet("search/{searchTerm}")]
        public async Task<ActionResult<IEnumerable<SearchProductDto>>> Search(string searchTerm)
        {
            var products = await _productService.SearchProductsAsync(searchTerm);
            return Ok(products);
        }

        /// <summary>
        /// الحصول على المنتجات التي تحت الحد الأدنى للمخزون
        /// </summary>
        [HttpGet("low-stock")]
        public async Task<ActionResult<IEnumerable<LowStockReportDto>>> GetLowStock()
        {
            var products = await _productService.GetLowStockProductsAsync();
            return Ok(products);
        }

        /// <summary>
        /// إنشاء منتج جديد
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<ProductDto>> Create([FromBody] CreateProductDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var product = await _productService.CreateProductAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = product.ProductID }, product);
        }

        /// <summary>
        /// تحديث منتج
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult<ProductDto>> Update(Guid id, [FromBody] UpdateProductDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var product = await _productService.UpdateProductAsync(id, dto);
            if (product == null)
                return NotFound(new { message = "المنتج غير موجود" });

            return Ok(product);
        }

        /// <summary>
        /// حذف منتج
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _productService.DeleteProductAsync(id);
            if (!result)
                return NotFound(new { message = "المنتج غير موجود" });

            return NoContent();
        }
    }
}