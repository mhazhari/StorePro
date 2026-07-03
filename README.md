# StorePro - نظام إدارة المتاجر

## 📋 الوصف

تطبيق ASP.NET Core 10 شامل لإدارة المتاجر والمخازن مع:
- ✅ إدارة المنتجات والأصناف
- ✅ إدارة الفواتير والمبيعات
- ✅ تتبع المخزون
- ✅ إدارة العمليات النقدية
- ✅ التقارير والإحصائيات

## 🏗️ هيكل المشروع

```
StorePro/
├── StorePro.Api/              # Web API
├── StorePro.Models/           # Entity Models و DTOs
├── StorePro.Data/             # Database Context و Repositories
├── StorePro.Core/             # Business Services و Mapping
└── StorePro.sln               # Solution File
```

## 🔧 المتطلبات

- .NET 10.0 SDK
- SQL Server 2019+
- Visual Studio 2022 (اختياري)

## 📦 الحزم المستخدمة

- Microsoft.EntityFrameworkCore 10.0.0
- Microsoft.EntityFrameworkCore.SqlServer 10.0.0
- AutoMapper 13.0.1
- Swashbuckle.AspNetCore 6.4.6

## ⚙️ الإعداد والتشغيل

### 1. تحديث سلسلة الاتصال

قم بتحديث `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_SERVER;Database=StoreDB;User Id=YOUR_USER;Password=YOUR_PASSWORD;Encrypt=false;TrustServerCertificate=true;"
  }
}
```

### 2. تنفيذ الترحيلات

```bash
dotnet ef database update --project StorePro.Data --startup-project StorePro.Api
```

### 3. تشغيل التطبيق

```bash
dotnet run --project StorePro.Api
```

سيكون التطبيق متاحاً على: `https://localhost:5001`

## 📚 API Documentation

التوثيق الكامل متاح على:
`https://localhost:5001/swagger`

## 🗄️ جداول قاعدة البيانات

### 1. جداول الأصناف والمنتجات
- `Tbl_ITm` - المنتجات
- `Tbl_ITm_OnUnit` - وحدات المنتجات

### 2. جداول الفواتير والمبيعات
- `Order_Main` - الفواتير الرئيسية
- `Order_Products` - تفاصيل الفواتير

### 3. جداول الحسابات والعمليات النقدية
- `ChartAcc_Main` - دليل الحسابات
- `Voucher_Main` - الإيصالات والعمليات النقدية

## 🔌 API Endpoints

### المنتجات
- `GET /api/products` - جميع المنتجات
- `GET /api/products/{id}` - منتج محدد
- `GET /api/products/barcode/{barcode}` - البحث برمز
- `GET /api/products/search/{term}` - البحث
- `GET /api/products/low-stock` - المخزون الناقص
- `POST /api/products` - إنشاء منتج
- `PUT /api/products/{id}` - تحديث منتج
- `DELETE /api/products/{id}` - حذف منتج

### الفواتير
- `GET /api/orders` - جميع الفواتير
- `GET /api/orders/{id}` - فاتورة محددة
- `GET /api/orders/date-range` - الفواتير في نطاق تاريخي
- `POST /api/orders` - إنشاء فاتورة
- `PUT /api/orders/{id}` - تحديث فاتورة
- `DELETE /api/orders/{id}` - إلغاء فاتورة
- `GET /api/orders/{id}/print` - الفاتورة للطباعة

### العمليات النقدية
- `GET /api/cashtransactions` - جميع الإيصالات
- `GET /api/cashtransactions/{id}` - إيصال محدد
- `GET /api/cashtransactions/date-range` - في نطاق تاريخي
- `GET /api/cashtransactions/daily-balance` - الرصيد اليومي
- `POST /api/cashtransactions` - إنشاء إيصال

## 📝 أمثلة الاستخدام

### إنشاء منتج
```bash
curl -X POST https://localhost:5001/api/products \
  -H "Content-Type: application/json" \
  -d '{
    "itm_Name": "منتج جديد",
    "privateCode": "SKU123",
    "categoryName": "فئة",
    "storMsgMin": 10
  }'
```

### إنشاء فاتورة
```bash
curl -X POST https://localhost:5001/api/orders \
  -H "Content-Type: application/json" \
  -d '{
    "orderType": 1,
    "storeGUID": "guid-value",
    "paymentMethod": "نقد",
    "orderDetails": []
  }'
```

## 🔐 الأمان

- استخدم HTTPS في الإنتاج
- حماية كلمات المرور في `appsettings.json`
- استخدم متغيرات البيئة للبيانات الحساسة

## 🤝 المساهمة

المشروع مفتوح للتطوير والتحسين. يرجى:
1. إنشاء فرع جديد
2. إجراء التغييرات
3. فتح Pull Request

## 📄 الترخيص

هذا المشروع مرخص تحت MIT License

## 📧 التواصل

للأسئلة والاستفسارات: mo7elsayed@example.com

---

**آخر تحديث:** 2026-07-03
