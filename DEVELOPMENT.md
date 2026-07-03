# StorePro - دليل التطوير

## 🏗️ البنية المعمارية

### Layered Architecture

```
┌─────────────────────────┐
│   StorePro.Api          │ (Presentation Layer - واجهة برمجية)
├─────────────────────────┤
│   StorePro.Core         │ (Business Logic Layer - منطق العمل)
├─────────────────────────┤
│   StorePro.Data         │ (Data Access Layer - الوصول للبيانات)
├─────────────────────────┤
│   StorePro.Models       │ (Models & DTOs - نماذج البيانات)
├─────────────────────────┤
│   SQL Server            │ (Database - قاعدة البيانات)
└─────────────────────────┘
```

## 📦 مكونات المشروع

### 1. StorePro.Models
يحتوي على:
- **Entities**: نماذج قاعدة البيانات
  - `TblItm` - المنتجات
  - `TblItmOnUnit` - وحدات المنتجات
  - `OrderMain` - الفواتير
  - `OrderProducts` - تفاصيل الفواتير
  - `ChartAccMain` - دليل الحسابات
  - `VoucherMain` - الإيصالات

- **DTOs**: نماذج نقل البيانات
  - `ProductDto`, `CreateProductDto`, `UpdateProductDto`
  - `OrderDto`, `CreateOrderDto`, `UpdateOrderDto`
  - `VoucherDto`, `CreateVoucherDto`

### 2. StorePro.Data
يحتوي على:
- **ApplicationDbContext**: DbContext الرئيسي
  - تكوين جميع الجداول والعلاقات
  - استراتيجيات الحذف (Delete Behavior)

- **Repositories**: نمط Repository
  - `IRepository<T>`: الواجهة العام
  - `Repository<T>`: التطبيق العام

### 3. StorePro.Core
يحتوي على:
- **Services**: خدمات العمل
  - `IProductService` / `ProductService`
  - `IOrderService` / `OrderService`
  - `ICashTransactionService` / `CashTransactionService`

- **Mapping**: AutoMapper Profiles
  - `MappingProfile`: تعريفات التحويل

### 4. StorePro.Api
يحتوي على:
- **Controllers**: معالجات الطلبات
  - `ProductsController`
  - `OrdersController`
  - `CashTransactionsController`

- **Configuration Files**
  - `Program.cs`: إعداد التطبيق
  - `appsettings.json`: إعدادات الإنتاج
  - `appsettings.Development.json`: إعدادات التطوير

## 🔄 تدفق البيانات

```
HTTP Request
    ↓
Controller (StorePro.Api)
    ↓
Service (StorePro.Core)
    ↓
Repository (StorePro.Data)
    ↓
DbContext (StorePro.Data)
    ↓
SQL Server Database
    ↓
Response ← Auto Mapped DTOs ← Entities
```

## 🛠️ الأنماط المستخدمة

### 1. Repository Pattern
```csharp
var product = await _repository.GetByIdAsync(id);
var products = await _repository.FindAsync(p => p.IsBlocked != true);
await _repository.AddAsync(newProduct);
await _repository.SaveChangesAsync();
```

### 2. Service Layer Pattern
```csharp
public class ProductService : IProductService
{
    private readonly IRepository<TblItm> _repository;
    
    public async Task<ProductDto> GetProductByIdAsync(Guid id)
    {
        var entity = await _repository.GetByIdAsync(id);
        return _mapper.Map<ProductDto>(entity);
    }
}
```

### 3. Dependency Injection
```csharp
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddAutoMapper(typeof(MappingProfile));
```

### 4. AutoMapper Profiles
```csharp
CreateMap<TblItm, ProductDto>()
    .ForMember(dest => dest.Units, opt => opt.MapFrom(src => src.TblItmOnUnits))
    .ReverseMap();
```

## 📊 علاقات قاعدة البيانات

### One-to-Many
```
TblItm (1) ──→ (Many) TblItmOnUnit
OrderMain (1) ──→ (Many) OrderProducts
ChartAccMain (1) ──→ (Many) VoucherMain
```

### Foreign Keys
```csharp
// في ApplicationDbContext
entity.HasMany(e => e.OrderProducts)
    .WithOne(o => o.OrderMain)
    .HasForeignKey(o => o.OMainGuID)
    .OnDelete(DeleteBehavior.Cascade);
```

## 🔐 استراتيجيات الحذف

- **Cascade**: حذف الفواتير عند حذف الحساب
- **Restrict**: منع حذف المنتج إذا كان له فواتير
- **SetNull**: تعيين NULL عند حذف الحساب الأب

## 📝 تسمية الاتفاقيات

- **Entities**: تبدأ بـ `Tbl` أو اسم الجدول الأصلي
- **DTOs**: تنتهي بـ `Dto`
- **Services**: واجهات `IServiceName`، تطبيق `ServiceName`
- **Controllers**: تنتهي بـ `Controller`
- **Properties**: PascalCase
- **Methods**: Async تنتهي بـ `Async`

## 🚀 خطوات الإضافة ميزة جديدة

### 1. إضافة Entity (في StorePro.Models)
```csharp
public class YourEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    // Navigation properties
}
```

### 2. إضافة DTOs (في StorePro.Models)
```csharp
public class YourEntityDto { }
public class CreateYourEntityDto { }
public class UpdateYourEntityDto { }
```

### 3. تحديث DbContext (StorePro.Data)
```csharp
public DbSet<YourEntity> YourEntity { get; set; }

// في OnModelCreating
modelBuilder.Entity<YourEntity>(entity => {
    entity.HasKey(e => e.Id);
    entity.ToTable("Tbl_YourEntity");
});
```

### 4. إضافة Service (StorePro.Core)
```csharp
public interface IYourEntityService { }
public class YourEntityService : IYourEntityService { }
```

### 5. إضافة Mapping (StorePro.Core)
```csharp
CreateMap<YourEntity, YourEntityDto>().ReverseMap();
```

### 6. إضافة Controller (StorePro.Api)
```csharp
[ApiController]
[Route("api/[controller]")]
public class YourEntitiesController : ControllerBase { }
```

### 7. تسجيل في Program.cs
```csharp
builder.Services.AddScoped<IYourEntityService, YourEntityService>();
```

## 🧪 الاختبار

### اختبار API يدويًا
```bash
# الحصول على جميع المنتجات
curl https://localhost:5001/api/products

# إضافة منتج
curl -X POST https://localhost:5001/api/products \
  -H "Content-Type: application/json" \
  -d '{"itm_Name": "منتج"}'
```

### استخدام Swagger
زيارة: `https://localhost:5001/swagger`

## 📚 المراجع

- [Entity Framework Core](https://learn.microsoft.com/en-us/ef/core/)
- [ASP.NET Core](https://learn.microsoft.com/en-us/aspnet/core/)
- [AutoMapper](https://automapper.org/)
- [Repository Pattern](https://www.microsoft.com/en-us/research/publication/repository-patterns-data-access-layer-design/)

---

**آخر تحديث:** 2026-07-03
