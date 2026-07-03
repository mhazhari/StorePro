using Microsoft.AspNetCore.Mvc;
using StorePro.Web.Models;
using StorePro.Web.Services;

namespace StorePro.Web.Controllers;

/// <summary>
/// متحكم المنتجات
/// </summary>
public class ProductsController : Controller
{
    private readonly IProductApiService _productService;
    private readonly ILogger<ProductsController> _logger;

    public ProductsController(
        IProductApiService productService,
        ILogger<ProductsController> logger)
    {
        _productService = productService;
        _logger = logger;
    }

    /// <summary>
    /// عرض قائمة المنتجات
    /// </summary>
    public async Task<IActionResult> Index()
    {
        try
        {
            var products = await _productService.GetAllProductsAsync();
            return View(products);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error loading products: {ex.Message}");
            TempData["ErrorMessage"] = "خطأ في تحميل المنتجات";
            return RedirectToAction("Index", "Dashboard");
        }
    }

    /// <summary>
    /// عرض تفاصيل منتج
    /// </summary>
    public async Task<IActionResult> Details(Guid id)
    {
        try
        {
            var product = await _productService.GetProductByIdAsync(id);
            return View(product);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error loading product {id}: {ex.Message}");
            return NotFound();
        }
    }

    /// <summary>
    /// صفحة إضافة منتج جديد
    /// </summary>
    public IActionResult Create()
    {
        return View();
    }

    /// <summary>
    /// حفظ منتج جديد
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateProductApiDto model)
    {
        if (!ModelState.IsValid)
            return View(model);

        try
        {
            await _productService.CreateProductAsync(model);
            TempData["SuccessMessage"] = "تم إضافة المنتج بنجاح";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error creating product: {ex.Message}");
            ModelState.AddModelError("", "خطأ في إضافة المنتج");
            return View(model);
        }
    }

    /// <summary>
    /// صفحة تعديل منتج
    /// </summary>
    public async Task<IActionResult> Edit(Guid id)
    {
        try
        {
            var product = await _productService.GetProductByIdAsync(id);
            var editModel = new UpdateProductApiDto
            {
                ItmName = product.ItmName,
                Price = product.Price,
                Quantity = product.Quantity,
                IsBlocked = product.IsBlocked
            };
            ViewData["ProductId"] = id;
            return View(editModel);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error loading product for edit {id}: {ex.Message}");
            return NotFound();
        }
    }

    /// <summary>
    /// حفظ تعديل المنتج
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, UpdateProductApiDto model)
    {
        if (!ModelState.IsValid)
        {
            ViewData["ProductId"] = id;
            return View(model);
        }

        try
        {
            await _productService.UpdateProductAsync(id, model);
            TempData["SuccessMessage"] = "تم تحديث المنتج بنجاح";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error updating product {id}: {ex.Message}");
            ModelState.AddModelError("", "خطأ في تحديث المنتج");
            return View(model);
        }
    }

    /// <summary>
    /// حذف منتج
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            await _productService.DeleteProductAsync(id);
            TempData["SuccessMessage"] = "تم حذف المنتج بنجاح";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error deleting product {id}: {ex.Message}");
            TempData["ErrorMessage"] = "خطأ في حذف المنتج";
            return RedirectToAction(nameof(Index));
        }
    }
}
