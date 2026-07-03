# 🏪 StorePro - نظام إدارة المتجر الذكي

![Status](https://img.shields.io/badge/Status-Active-brightgreen)
![Version](https://img.shields.io/badge/Version-1.0.0-blue)
![License](https://img.shields.io/badge/License-MIT-green)
![Language](https://img.shields.io/badge/Language-C%23-purple)

## 📌 نظرة عامة

**StorePro** هو نظام متكامل لإدارة المتاجر مبني على:
- **Backend**: ASP.NET Core 8.0 REST API
- **Frontend**: ASP.NET Core 8.0 MVC
- **Database**: SQL Server
- **Authentication**: JWT Tokens
- **Architecture**: Layered Architecture

---

## 🎯 الميزات الرئيسية

### 📦 إدارة المنتجات
- ✅ إضافة وتعديل وحذف المنتجات
- ✅ إدارة وحدات القياس المختلفة
- ✅ تتبع المخزون والأسعار
- ✅ تصنيفات المنتجات

### 📋 إدارة الفواتير
- ✅ إنشاء فواتير بيع جديدة
- ✅ إدارة تفاصيل البنود
- ✅ حساب الأرباح والخسائر
- ✅ تتبع الخصومات والرسوم

### 💰 إدارة النقدية والحسابات
- ✅ جدول الحسابات المحاسبي
- ✅ الإيصالات النقدية
- ✅ تتبع رصيد الحسابات
- ✅ التقارير المالية

### 🔐 المصادقة والتفويض
- ✅ نظام تسجيل الدخول آمن
- ✅ JWT Tokens
- ✅ صلاحيات المستخدمين (Admin, User)
- ✅ جلسات آمنة

### 🎨 واجهة المستخدم
- ✅ تصميم عصري وحديث (Bootstrap 5)
- ✅ دعم اللغة العربية (RTL)
- ✅ استجابة كاملة على جميع الأجهزة
- ✅ رسائل توضيحية وأخطاء واضحة

---

## 🏗️ البنية المعمارية

```
StorePro/
│
├── StorePro.Api/           # REST API Layer
│   ├── Controllers/
│   ├── Program.cs
│   └── appsettings.json
│
├── StorePro.Web/           # Presentation Layer (MVC)
│   ├── Controllers/
│   ├── Views/
│   ├── Services/
│   ├── Models/
│   └── Program.cs
│
├── StorePro.Core/          # Business Logic Layer
│   ├── Services/
│   └── Mapping/
│
├── StorePro.Data/          # Data Access Layer
│   ├── ApplicationDbContext.cs
│   └── Repositories/
│
└── StorePro.Models/        # Models & DTOs
    ├── Entities/
    └── DTOs/
```

---

## 🚀 البدء السريع

### المتطلبات
- **.NET 8.0 SDK** أو أعلى
- **Visual Studio 2022** (Community أو أعلى)
- **SQL Server** (LocalDB أو Enterprise)
- **Git**

### التثبيت والتشغيل

#### 1️⃣ استنساخ المستودع
```bash
git clone https://github.com/mhazhari/StorePro.git
cd StorePro
```

#### 2️⃣ فتح في Visual Studio
```bash
start StorePro.sln
```

#### 3️⃣ استعادة الحزم
```bash
dotnet restore
```

#### 4️⃣ إنشاء قاعدة البيانات
```bash
# في Package Manager Console
Update-Database
```

#### 5️⃣ تشغيل API
```bash
dotnet run --project StorePro.Api
# API: https://localhost:5001
```

#### 6️⃣ تشغيل Web (في terminal جديد)
```bash
dotnet run --project StorePro.Web
# Web: https://localhost:5002
```

#### 7️⃣ الوصول للتطبيق
```
https://localhost:5002
```

---

## 📚 الأدلة التفصيلية

### 🔧 دليل الإعداد الكامل
📖 **[SETUP_GUIDE.md](./SETUP_GUIDE.md)**
- خطوات التثبيت والإعداد
- حل المشاكل الشائعة
- إعدادات قاعدة البيانات

### 🌐 تعديلات API المطلوبة
📖 **[API_MODIFICATIONS.md](./API_MODIFICATIONS.md)**
- تفعيل CORS
- إعدادات الأمان
- الخطوات البسيطة المطلوبة

### 🔐 دليل JWT Authentication
📖 **[JWT_AUTHENTICATION.md](./JWT_AUTHENTICATION.md)**
- إضافة نظام المصادقة
- إنشاء جدول المستخدمين
- تطبيق JWT Tokens
- حماية الـ APIs

### 📖 دليل Web Project
📖 **[StorePro.Web/README.md](./StorePro.Web/README.md)**
- بنية المشروع
- الخدمات المتاحة
- طريقة الاستخدام

---

## 🔄 تدفق البيانات

```
المستخدم
   ↓
Web Controller
   ↓
Service Layer
   ↓
ApiClient (HttpClient)
   ↓
API Controller
   ↓
Service Layer
   ↓
Repository Pattern
   ↓
Entity Framework
   ↓
SQL Server Database
   ↓
JSON Response ← HTML Response
   ↓
المتصفح
```

---

## 🛠️ التكنولوجيات المستخدمة

### Backend
| التقنية | الإصدار | الغرض |
|---------|--------|-------|
| ASP.NET Core | 8.0 | Framework |
| Entity Framework Core | 8.0 | ORM |
| JWT Tokens | 7.0 | المصادقة |
| AutoMapper | 12.0 | تحويل البيانات |
| Serilog | 8.0 | Logging |

### Frontend
| التقنية | الإصدار | الغرض |
|---------|--------|-------|
| ASP.NET Core MVC | 8.0 | Framework |
| Bootstrap | 5.3 | CSS Framework |
| Razor Views | 8.0 | Template Engine |

### Database
| التقنية | الإصدار | الغرض |
|---------|--------|-------|
| SQL Server | 2019+ | Database |
| Entity Framework | 8.0 | Migration |

---

## 📊 هيكل قاعدة البيانات

### الجداول الرئيسية

#### 🛒 المنتجات والمخزون
- `Tbl_ITm` - المنتجات
- `Tbl_ITm_OnUnit` - وحدات المنتجات
- `ChartAcc_Main` - دليل الحسابات

#### 📋 المبيعات
- `Order_Main` - الفواتير الرئيسية
- `Order_Products` - تفاصيل بنود الفواتير

#### 💰 النقدية
- `Voucher_Main` - الإيصالات النقدية

#### 👥 المستخدمين
- `Users` - بيانات المستخدمين (JWT Authentication)

---

## 🔐 الأمان

### المصادقة
- ✅ JWT Tokens
- ✅ كلمات مرور مشفرة (BCrypt)
- ✅ جلسات آمنة

### التفويض
- ✅ Role-based Authorization
- ✅ Protected Endpoints
- ✅ CORS Configuration

### الحماية
- ✅ HTTPS/TLS
- ✅ CSRF Protection
- ✅ SQL Injection Prevention
- ✅ XSS Protection

---

## 📖 أمثلة الاستخدام

### الدخول
```bash
curl -X POST https://localhost:5001/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{
    "username": "admin",
    "password": "password123"
  }'
```

### الحصول على المنتجات
```bash
curl https://localhost:5001/api/products \
  -H "Authorization: Bearer YOUR_TOKEN"
```

### إضافة منتج
```bash
curl -X POST https://localhost:5001/api/products \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer YOUR_TOKEN" \
  -d '{
    "itmName": "منتج جديد",
    "price": 100,
    "quantity": 50
  }'
```

---

## 🧪 الاختبار

### Swagger UI
```
https://localhost:5001/swagger
```

### اختبار الويب
```
https://localhost:5002
```

---

## 📝 الترخيص

هذا المشروع مرخص تحت **MIT License**

---

## 👨‍💻 المساهمة

نرحب بالمساهمات! يرجى:

1. عمل Fork للمستودع
2. إنشاء فرع جديد (`git checkout -b feature/amazing-feature`)
3. عمل Commit للتغييرات (`git commit -m 'Add amazing feature'`)
4. Push إلى الفرع (`git push origin feature/amazing-feature`)
5. فتح Pull Request

---

## 🐛 الإبلاغ عن المشاكل

وجدت مشكلة؟ أنشئ [Issue جديد](https://github.com/mhazhari/StorePro/issues)

---

## 📞 الدعم والمساعدة

### المجتمع
- 💬 [GitHub Discussions](https://github.com/mhazhari/StorePro/discussions)
- 🐛 [GitHub Issues](https://github.com/mhazhari/StorePro/issues)

### الموارد
- 📚 [Microsoft Docs](https://learn.microsoft.com/en-us/aspnet/core/)
- 🔗 [Entity Framework Core](https://learn.microsoft.com/en-us/ef/core/)
- 🔐 [JWT Documentation](https://jwt.io/)

---

## 🗺️ خارطة الطريق

### النسخة 1.1 (قادمة)
- [ ] تقارير متقدمة
- [ ] نظام الإشعارات
- [ ] Multi-tenant Support

### النسخة 1.2
- [ ] تطبيق موبايل (Flutter/React Native)
- [ ] نظام الدفع المتكامل
- [ ] Real-time Notifications

---

## 📈 الإحصائيات

![GitHub Stars](https://img.shields.io/github/stars/mhazhari/StorePro?style=social)
![GitHub Forks](https://img.shields.io/github/forks/mhazhari/StorePro?style=social)
![GitHub Issues](https://img.shields.io/github/issues/mhazhari/StorePro)

---

## 👏 شكر وتقدير

شكر خاص للمساهمين والمستخدمين الذين يدعمون هذا المشروع!

---

## 📧 التواصل

**المطور**: Mohamed Azhari  
**البريد الإلكتروني**: [your-email@example.com](mailto:your-email@example.com)  
**GitHub**: [@mhazhari](https://github.com/mhazhari)

---

## 📄 آخر تحديث

**تاريخ**: 2026-07-03  
**الإصدار**: 1.0.0  
**الحالة**: ✅ جاهز للاستخدام

---

<div align="center">

### صُنع بـ ❤️ من أجلك

**StorePro** - نظام إدارة المتاجر الذكي

⭐ إذا أعجبك المشروع، لا تنسى النجمة!

</div>
