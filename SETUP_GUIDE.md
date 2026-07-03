# 📖 دليل الإعداد الكامل لـ StorePro

## 🎯 نظرة عامة

هذا الدليل يشرح كيفية إعداد وتشغيل مشروع StorePro الكامل الذي يتكون من:
- **StorePro.Api**: واجهة برمجية REST
- **StorePro.Web**: تطبيق ويب MVC

---

## 📋 المتطلبات

- **.NET 8.0 SDK** أو أعلى
- **Visual Studio 2022** (Community أو أعلى)
- **SQL Server** (LocalDB أو Enterprise)
- **Git** للنسخ والتحديثات

---

## 🔧 خطوات الإعداد

### 1️⃣ استنساخ المشروع

```bash
git clone https://github.com/mhazhari/StorePro.git
cd StorePro
```

### 2️⃣ فتح المشروع

```bash
# باستخدام Visual Studio
start StorePro.sln

# أو باستخدام الكود
code .
```

### 3️⃣ استعادة الحزم

```bash
dotnet restore
```

### 4️⃣ إعداد قاعدة البيانات

في **Package Manager Console**:

```powershell
# تحديد المشروع الافتراضي
Set-DefaultProject StorePro.Data

# تطبيق الهجرات
Update-Database
```

أو بالـ CLI:

```bash
dotnet ef database update --project StorePro.Data
```

### 5️⃣ تشغيل API

```bash
# من المجلد الجذري
dotnet run --project StorePro.Api

# أو من خلال Visual Studio
# اختر StorePro.Api كمشروع بدء واضغط F5
```

API سيعمل على: `https://localhost:5001`

### 6️⃣ تشغيل Web

في **نافذة terminal جديدة**:

```bash
dotnet run --project StorePro.Web

# أو من خلال Visual Studio
# اختر StorePro.Web كمشروع بدء واضغط F5
```

Web سيعمل على: `https://localhost:5002`

---

## ✅ التحقق من التشغيل السليم

### اختبار API

```bash
# زيارة Swagger UI
https://localhost:5001/swagger

# أو اختبر GET request
curl https://localhost:5001/api/products
```

### اختبار Web

```bash
# زيارة الصفحة الرئيسية
https://localhost:5002

# يجب أن ترى لوحة التحكم
```

---

## 🌐 إعدادات CORS

تأكد من وجود CORS مفعل في **StorePro.Api/Program.cs**:

```csharp
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowWebClient", builder =>
    {
        builder.WithOrigins("https://localhost:5002", "http://localhost:5003")
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

app.UseCors("AllowWebClient");
```

---

## 📝 إعدادات اتصال البيانات

في **appsettings.json** للـ API:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=.;Database=StorePro;Integrated Security=true;"
  }
}
```

في **appsettings.json** للـ Web:

```json
{
  "ApiSettings": {
    "BaseUrl": "https://localhost:5001"
  }
}
```

---

## 🚀 الاستخدام الأساسي

### إضافة منتج جديد

1. افتح `https://localhost:5002`
2. انقر على **📦 المنتجات**
3. انقر على **➕ إضافة منتج جديد**
4. أدخل البيانات واضغط **✅ حفظ**

### إنشاء فاتورة

1. انقر على **📋 الفواتير**
2. انقر على **➕ فاتورة جديدة**
3. أضف البنود واختر المنتجات
4. انقر **💾 حفظ**

---

## 🔍 استكشاف الأخطاء

### خطأ: "لا يمكن الاتصال بـ API"

✅ **الحل:**
- تأكد من تشغيل API على `https://localhost:5001`
- تحقق من رابط API في `appsettings.json`
- تحقق من firewall الخاص بك

### خطأ: "قاعدة البيانات غير موجودة"

✅ **الحل:**
```bash
# أعد التهيئة
dotnet ef database drop --project StorePro.Data
dotnet ef database update --project StorePro.Data
```

### خطأ: "CORS Error"

✅ **الحل:**
- تأكد من تفعيل CORS في API
- تأكد من أن رابط الويب مدرج في `WithOrigins()`

---

## 📦 هيكل المشروع

```
StorePro/
├── StorePro.Api/          # API
│   ├── Controllers/
│   ├── Program.cs
│   └── appsettings.json
├── StorePro.Web/          # Web MVC
│   ├── Controllers/
│   ├── Views/
│   ├── Services/
│   ├── Program.cs
│   └── appsettings.json
├── StorePro.Core/         # Business Logic
│   └── Services/
├── StorePro.Data/         # Data Access
│   ├── ApplicationDbContext.cs
│   └── Repositories/
├── StorePro.Models/       # Entities & DTOs
│   ├── Entities/
│   └── DTOs/
└── StorePro.sln
```

---

## 🔄 التطوير والاختبار

### تشغيل الاختبارات

```bash
dotnet test
```

### إضافة هجرة جديدة

```bash
dotnet ef migrations add MigrationName --project StorePro.Data
dotnet ef database update --project StorePro.Data
```

---

## 🌍 النشر للإنتاج

### إعدادات الإنتاج

قم بإنشاء `appsettings.Production.json`:

```json
{
  "ApiSettings": {
    "BaseUrl": "https://your-api-domain.com"
  },
  "ConnectionStrings": {
    "DefaultConnection": "Server=production-server;Database=StorePro;User Id=sa;Password=****;"
  }
}
```

### النشر على Azure

```bash
# إنشاء ملف نشر
dotnet publish -c Release -o ./publish

# أو استخدم Azure DevOps/GitHub Actions
```

---

## 📚 الموارد الإضافية

- [ASP.NET Core Docs](https://learn.microsoft.com/en-us/aspnet/core/)
- [Entity Framework Core](https://learn.microsoft.com/en-us/ef/core/)
- [AutoMapper](https://automapper.org/)

---

## 💬 المساعدة والدعم

إذا واجهت مشاكل:

1. تحقق من [GitHub Issues](https://github.com/mhazhari/StorePro/issues)
2. أنشئ issue جديد مع وصف المشكلة
3. شارك السجلات والرسائل الخطأ

---

**آخر تحديث:** 2026-07-03 ✅
