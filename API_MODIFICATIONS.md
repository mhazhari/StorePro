# 🔧 تعديلات مطلوبة في StorePro.Api

## ⚠️ ملخص التعديلات

هذا الدليل يشرح التعديلات **الضرورية والبسيطة** المطلوبة في **StorePro.Api** لتشغيل **StorePro.Web** بنجاح.

---

## ✅ التعديل الأساسي: تفعيل CORS

### 📍 الملف: `StorePro.Api/Program.cs`

أضف الكود التالي **قبل `var app = builder.Build();`**:

```csharp
// ===== CORS Configuration =====
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowWebClient", builder =>
    {
        builder.WithOrigins(
            "https://localhost:5002",  // Web HTTPS
            "http://localhost:5003"    // Web HTTP
        )
        .AllowAnyMethod()              // GET, POST, PUT, DELETE
        .AllowAnyHeader()              // Any headers
        .AllowCredentials();           // Allow cookies/authentication
    });
});
```

ثم أضف **بعد `var app = builder.Build();`** وقبل `app.Run();`**:

```csharp
// ===== Use CORS =====
app.UseCors("AllowWebClient");
```

### 📝 مثال كامل:

```csharp
var builder = WebApplication.CreateBuilder(args);

// ... باقي الإعدادات ...

// ===== CORS Configuration =====
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowWebClient", builder =>
    {
        builder.WithOrigins(
            "https://localhost:5002",
            "http://localhost:5003"
        )
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials();
    });
});

var app = builder.Build();

// ===== Middleware =====
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/api/error");
    app.UseHsts();
}

app.UseHttpsRedirection();

// ===== Use CORS (يجب أن يكون قبل UseAuthorization) =====
app.UseCors("AllowWebClient");

app.UseAuthorization();

app.MapControllers();

app.Run();
```

---

## 🚀 خطوات التطبيق

### الخطوة 1: فتح `StorePro.Api/Program.cs`

```bash
# في Visual Studio Code أو Visual Studio
# افتح الملف StorePro.Api/Program.cs
```

### الخطوة 2: إضافة CORS Configuration

انسخ الكود أعلاه والصقه في `Program.cs`

### الخطوة 3: التحقق من الترتيب

تأكد من أن الترتيب صحيح:

1. ✅ `AddCors()` - في مرحلة البناء
2. ✅ `app.UseCors()` - بعد `builder.Build()` مباشرة

### الخطوة 4: إعادة تشغيل API

```bash
# إيقاف API الحالية
Ctrl + C

# إعادة تشغيل
dotnet run --project StorePro.Api

# أو من Visual Studio: F5
```

---

## ✨ هذا كل شيء!

**لا توجد أي تعديلات أخرى مطلوبة** ✅

### لماذا؟

- ✅ المنتجات والفواتير مُعرّفة بالفعل
- ✅ Controllers مُعرّفة
- ✅ DTOs مُعرّفة
- ✅ Services مُعرّفة
- ✅ فقط نحتاج CORS للسماح للويب بالاتصال

---

## 🧪 اختبار الاتصال

### 1. تشغيل API

```bash
dotnet run --project StorePro.Api
# API working on: https://localhost:5001
```

### 2. اختبر Swagger

```
https://localhost:5001/swagger
```

يجب أن ترى جميع المتحكمات (Products, Orders, إلخ)

### 3. تشغيل Web

```bash
# في terminal جديد
dotnet run --project StorePro.Web
# Web working on: https://localhost:5002
```

### 4. اختبر الويب

```
https://localhost:5002
```

يجب أن تظهر الصفحة بدون أخطاء CORS

---

## 🔍 التحقق من CORS

إذا رأيت خطأ CORS، تحقق من:

1. **✅ هل CORS مُفعّل في Program.cs؟**
   - ابحث عن `AddCors`
   - ابحث عن `app.UseCors`

2. **✅ هل الـ URLs صحيحة؟**
   - تأكد من `localhost:5002` و `localhost:5003`
   - تأكد من HTTPS و HTTP

3. **✅ هل الترتيب صحيح؟**
   - `UseCors()` يجب أن يكون **قبل** `UseAuthorization()`

4. **✅ أعد تشغيل API**
   ```bash
   Ctrl + C
   dotnet run --project StorePro.Api
   ```

---

## 📚 معلومات إضافية

### ماذا يفعل CORS؟

CORS تسمح للمتصفح بقبول الطلبات من نطاقات مختلفة:

- **بدون CORS**: الويب على `localhost:5002` لا يستطيع طلب البيانات من `localhost:5001`
- **مع CORS**: يسمح بالاتصال بأمان

### الخيارات المستخدمة:

```csharp
.AllowAnyMethod()      // GET, POST, PUT, DELETE, PATCH
.AllowAnyHeader()      // Content-Type, Authorization, إلخ
.AllowCredentials()    // Cookies, Auth tokens
```

---

## ❓ أسئلة شائعة

### س: هل هذا آمن للإنتاج؟

**الإجابة**: لا، هذا **فقط للتطوير المحلي**. للإنتاج:

```csharp
// للإنتاج - حدد النطاق المحدد فقط
.WithOrigins("https://yourdomain.com")
```

### س: هل يمكن حذف `AllowCredentials()`؟

**الإجابة**: نعم، إذا لم تستخدم:
- Cookies
- JWT Tokens
- Session State

### س: هل أحتاج Auth/Security؟

**الإجابة**: في الوقت الحالي لا، يمكن إضافتها لاحقاً.

---

## 📞 الدعم والمساعدة

إذا واجهت مشاكل:

1. تحقق من السجلات: `logs/storepro-web-*.txt`
2. اختبر Swagger API: `https://localhost:5001/swagger`
3. افتح Developer Tools في المتصفح (F12) وتحقق من Console

---

**آخر تحديث:** 2026-07-03 ✅
