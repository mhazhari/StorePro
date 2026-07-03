# 🔐 تطبيق JWT Authentication في StorePro

## 📋 نظرة عامة

هذا الدليل يشرح كيفية إضافة نظام المصادقة المتقدم باستخدام **JWT Tokens** في StorePro.

---

## 🎯 ما الذي سنفعله؟

✅ إنشاء جدول المستخدمين (`User`)  
✅ إضافة خدمة المصادقة والتفويض  
✅ إنشاء API endpoints للدخول والتسجيل  
✅ حماية الـ Controllers بـ Authorization  
✅ إضافة صفحة Login في الويب  
✅ حفظ Token في المتصفح وإرساله مع الطلبات  

---

## 🏗️ الخطوة 1: إنشاء جدول المستخدمين

### أ) إنشاء Entity

**ملف جديد:** `StorePro.Models/Entities/User.cs`

```csharp
namespace StorePro.Models.Entities
{
    /// <summary>
    /// نموذج المستخدم
    /// </summary>
    public class User
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        
        /// <summary>
        /// اسم المستخدم (فريد)
        /// </summary>
        public string Username { get; set; } = string.Empty;
        
        /// <summary>
        /// البريد الإلكتروني
        /// </summary>
        public string Email { get; set; } = string.Empty;
        
        /// <summary>
        /// كلمة المرور (مشفرة)
        /// </summary>
        public string PasswordHash { get; set; } = string.Empty;
        
        /// <summary>
        /// الاسم الكامل
        /// </summary>
        public string FullName { get; set; } = string.Empty;
        
        /// <summary>
        /// الدور (Admin, User)
        /// </summary>
        public string Role { get; set; } = "User";
        
        /// <summary>
        /// هل المستخدم نشط
        /// </summary>
        public bool IsActive { get; set; } = true;
        
        /// <summary>
        /// تاريخ الإنشاء
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        
        /// <summary>
        /// آخر تحديث
        /// </summary>
        public DateTime? UpdatedAt { get; set; }
    }
}
```

### ب) تحديث DbContext

أضف هذا السطر في `StorePro.Data/ApplicationDbContext.cs`:

```csharp
// أضف هذا مع باقي DbSet
public DbSet<User> Users { get; set; }
```

وأضف هذا في `OnModelCreating`:

```csharp
// ========== جدول المستخدمين ==========
modelBuilder.Entity<User>(entity =>
{
    entity.HasKey(e => e.Id);
    entity.ToTable("Users");
    
    entity.Property(e => e.Username).IsRequired().HasMaxLength(50);
    entity.HasIndex(e => e.Username).IsUnique();
    
    entity.Property(e => e.Email).IsRequired().HasMaxLength(100);
    entity.HasIndex(e => e.Email).IsUnique();
    
    entity.Property(e => e.PasswordHash).IsRequired();
    entity.Property(e => e.FullName).HasMaxLength(100);
    entity.Property(e => e.Role).HasMaxLength(20);
});
```

---

## 🔐 الخطوة 2: إنشاء خدمة المصادقة

### أ) إضافة NuGet Packages

في `StorePro.Api/StorePro.Api.csproj`:

```xml
<ItemGroup>
    <PackageReference Include="System.IdentityModel.Tokens.Jwt" Version="7.0.3" />
    <PackageReference Include="Microsoft.IdentityModel.Tokens" Version="7.0.3" />
</ItemGroup>
```

### ب) إنشاء Auth DTOs

**ملف جديد:** `StorePro.Models/DTOs/AuthDto.cs`

```csharp
namespace StorePro.Models.DTOs
{
    /// <summary>
    /// DTO لطلب الدخول
    /// </summary>
    public class LoginRequest
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    /// <summary>
    /// DTO لطلب التسجيل
    /// </summary>
    public class RegisterRequest
    {
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
    }

    /// <summary>
    /// DTO لاستجابة المصادقة
    /// </summary>
    public class AuthResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public string? Token { get; set; }
        public UserDto? User { get; set; }
    }

    /// <summary>
    /// DTO لبيانات المستخدم
    /// </summary>
    public class UserDto
    {
        public Guid Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
    }
}
```

### ج) إنشاء Auth Service

**ملف جديد:** `StorePro.Core/Services/IAuthService.cs`

```csharp
using StorePro.Models.DTOs;

namespace StorePro.Core.Services
{
    public interface IAuthService
    {
        Task<AuthResponse> LoginAsync(LoginRequest request);
        Task<AuthResponse> RegisterAsync(RegisterRequest request);
        Task<AuthResponse> RefreshTokenAsync(string token);
    }
}
```

**ملف جديد:** `StorePro.Core/Services/AuthService.cs`

```csharp
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using StorePro.Data;
using StorePro.Models.DTOs;
using StorePro.Models.Entities;
using AutoMapper;

namespace StorePro.Core.Services
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public AuthService(ApplicationDbContext context, IConfiguration configuration, IMapper mapper)
        {
            _context = context;
            _configuration = configuration;
            _mapper = mapper;
        }

        public async Task<AuthResponse> LoginAsync(LoginRequest request)
        {
            try
            {
                // البحث عن المستخدم
                var user = _context.Users.FirstOrDefault(u => u.Username == request.Username);

                if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
                {
                    return new AuthResponse
                    {
                        Success = false,
                        Message = "خطأ في اسم المستخدم أو كلمة المرور"
                    };
                }

                if (!user.IsActive)
                {
                    return new AuthResponse
                    {
                        Success = false,
                        Message = "حساب المستخدم معطل"
                    };
                }

                var token = GenerateJwtToken(user);
                var userDto = _mapper.Map<UserDto>(user);

                return new AuthResponse
                {
                    Success = true,
                    Message = "تم الدخول بنجاح",
                    Token = token,
                    User = userDto
                };
            }
            catch (Exception ex)
            {
                return new AuthResponse
                {
                    Success = false,
                    Message = $"خطأ: {ex.Message}"
                };
            }
        }

        public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
        {
            try
            {
                // التحقق من وجود المستخدم
                if (_context.Users.Any(u => u.Username == request.Username))
                {
                    return new AuthResponse
                    {
                        Success = false,
                        Message = "اسم المستخدم موجود بالفعل"
                    };
                }

                if (_context.Users.Any(u => u.Email == request.Email))
                {
                    return new AuthResponse
                    {
                        Success = false,
                        Message = "البريد الإلكتروني موجود بالفعل"
                    };
                }

                // إنشاء مستخدم جديد
                var user = new User
                {
                    Username = request.Username,
                    Email = request.Email,
                    FullName = request.FullName,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
                    Role = "User",
                    IsActive = true
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                var token = GenerateJwtToken(user);
                var userDto = _mapper.Map<UserDto>(user);

                return new AuthResponse
                {
                    Success = true,
                    Message = "تم التسجيل بنجاح",
                    Token = token,
                    User = userDto
                };
            }
            catch (Exception ex)
            {
                return new AuthResponse
                {
                    Success = false,
                    Message = $"خطأ: {ex.Message}"
                };
            }
        }

        public Task<AuthResponse> RefreshTokenAsync(string token)
        {
            // سيتم تطبيقه لاحقاً
            throw new NotImplementedException();
        }

        /// <summary>
        /// توليد JWT Token
        /// </summary>
        private string GenerateJwtToken(User user)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]));
            var signingCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(int.Parse(jwtSettings["ExpirationHours"])),
                signingCredentials: signingCredentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
```

---

## 🌐 الخطوة 3: إنشاء Auth Controller

**ملف جديد:** `StorePro.Api/Controllers/AuthController.cs`

```csharp
using Microsoft.AspNetCore.Mvc;
using StorePro.Core.Services;
using StorePro.Models.DTOs;

namespace StorePro.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        /// <summary>
        /// الدخول
        /// </summary>
        [HttpPost("login")]
        public async Task<ActionResult<AuthResponse>> Login([FromBody] LoginRequest request)
        {
            var response = await _authService.LoginAsync(request);
            
            if (!response.Success)
                return Unauthorized(response);

            return Ok(response);
        }

        /// <summary>
        /// التسجيل
        /// </summary>
        [HttpPost("register")]
        public async Task<ActionResult<AuthResponse>> Register([FromBody] RegisterRequest request)
        {
            var response = await _authService.RegisterAsync(request);
            
            if (!response.Success)
                return BadRequest(response);

            return Ok(response);
        }
    }
}
```

---

## ⚙️ الخطوة 4: تحديث API Configuration

### تحديث `Program.cs`:

```csharp
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using StorePro.Core.Services;

var builder = WebApplication.CreateBuilder(args);

// ===== JWT Configuration =====
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]);

builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(secretKey),
            ValidateIssuer = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidateAudience = true,
            ValidAudience = jwtSettings["Audience"],
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };
    });

// ===== Services =====
builder.Services.AddScoped<IAuthService, AuthService>();

// ===== CORS =====
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowWebClient", builder =>
    {
        builder.WithOrigins("https://localhost:5002", "http://localhost:5003")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
    });
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/api/error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseCors("AllowWebClient");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
```

### تحديث `appsettings.json`:

```json
{
  "JwtSettings": {
    "SecretKey": "your-super-secret-key-min-32-characters-1234567890",
    "Issuer": "StorePro.Api",
    "Audience": "StorePro.Web",
    "ExpirationHours": 24
  }
}
```

---

## 🌐 الخطوة 5: إعداد Web Client

### أ) إضافة NuGet Package

في `StorePro.Web/StorePro.Web.csproj`:

```xml
<ItemGroup>
    <PackageReference Include="System.IdentityModel.Tokens.Jwt" Version="7.0.3" />
</ItemGroup>
```

### ب) إنشاء Auth Service في الويب

**ملف جديد:** `StorePro.Web/Services/IAuthClientService.cs`

```csharp
using StorePro.Web.Models;

namespace StorePro.Web.Services
{
    public interface IAuthClientService
    {
        Task<AuthResponse> LoginAsync(LoginRequest request);
        Task<AuthResponse> RegisterAsync(RegisterRequest request);
        Task LogoutAsync();
        Task<string> GetTokenAsync();
        Task<bool> IsAuthenticatedAsync();
    }
}
```

**ملف جديد:** `StorePro.Web/Services/AuthClientService.cs`

```csharp
using StorePro.Web.Models;

namespace StorePro.Web.Services
{
    public class AuthClientService : IAuthClientService
    {
        private readonly IApiClient _apiClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private const string TokenKey = "auth_token";

        public AuthClientService(IApiClient apiClient, IHttpContextAccessor httpContextAccessor)
        {
            _apiClient = apiClient;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<AuthResponse> LoginAsync(LoginRequest request)
        {
            try
            {
                var response = await _apiClient.PostAsync<AuthResponse>("api/auth/login", request);
                
                if (response.Success && !string.IsNullOrEmpty(response.Token))
                {
                    _httpContextAccessor.HttpContext?.Session.SetString(TokenKey, response.Token);
                }

                return response;
            }
            catch (Exception ex)
            {
                return new AuthResponse { Success = false, Message = ex.Message };
            }
        }

        public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
        {
            try
            {
                var response = await _apiClient.PostAsync<AuthResponse>("api/auth/register", request);
                
                if (response.Success && !string.IsNullOrEmpty(response.Token))
                {
                    _httpContextAccessor.HttpContext?.Session.SetString(TokenKey, response.Token);
                }

                return response;
            }
            catch (Exception ex)
            {
                return new AuthResponse { Success = false, Message = ex.Message };
            }
        }

        public Task LogoutAsync()
        {
            _httpContextAccessor.HttpContext?.Session.Remove(TokenKey);
            return Task.CompletedTask;
        }

        public Task<string> GetTokenAsync()
        {
            var token = _httpContextAccessor.HttpContext?.Session.GetString(TokenKey) ?? string.Empty;
            return Task.FromResult(token);
        }

        public async Task<bool> IsAuthenticatedAsync()
        {
            var token = await GetTokenAsync();
            return !string.IsNullOrEmpty(token);
        }
    }
}
```

### ج) تحديث ApiClient لإرسال Token

**تحديث:** `StorePro.Web/Services/ApiClient.cs`

أضف هذا المتغير والدالة:

```csharp
private readonly IAuthClientService _authService;

public ApiClient(HttpClient httpClient, ILogger<ApiClient> logger, IAuthClientService authService)
{
    _httpClient = httpClient;
    _logger = logger;
    _authService = authService;
    _jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
}

private async Task AddAuthorizationHeaderAsync()
{
    var token = await _authService.GetTokenAsync();
    if (!string.IsNullOrEmpty(token))
    {
        _httpClient.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
    }
}
```

وحدّث جميع الدوال (GetAsync, PostAsync, PutAsync, DeleteAsync) لإضافة Authorization:

```csharp
public async Task<T> GetAsync<T>(string endpoint)
{
    try
    {
        await AddAuthorizationHeaderAsync();
        // ... باقي الكود
    }
    // ...
}
```

---

## 🎨 الخطوة 6: إنشاء صفحة Login

**ملف جديد:** `StorePro.Web/Views/Auth/Login.cshtml`

```html
@model StorePro.Web.Models.LoginRequest

@{
    ViewData["Title"] = "تسجيل الدخول";
}

<div class="container mt-5">
    <div class="row justify-content-center">
        <div class="col-md-6">
            <div class="card">
                <div class="card-header bg-dark text-white text-center">
                    <h3>🔐 تسجيل الدخول</h3>
                </div>
                <div class="card-body p-5">
                    <form method="post" asp-action="Login">
                        <div class="mb-3">
                            <label asp-for="Username" class="form-label">اسم المستخدم</label>
                            <input asp-for="Username" class="form-control form-control-lg" 
                                   placeholder="أدخل اسم المستخدم" required />
                        </div>

                        <div class="mb-3">
                            <label asp-for="Password" class="form-label">كلمة المرور</label>
                            <input asp-for="Password" type="password" class="form-control form-control-lg" 
                                   placeholder="أدخل كلمة المرور" required />
                        </div>

                        <div class="d-grid">
                            <button type="submit" class="btn btn-primary btn-lg">
                                دخول
                            </button>
                        </div>
                    </form>

                    <hr />

                    <p class="text-center">
                        ليس لديك حساب؟ 
                        <a asp-action="Register">أنشئ حساباً جديداً</a>
                    </p>
                </div>
            </div>
        </div>
    </div>
</div>
```

---

## 🔒 حماية Controllers

أضف `[Authorize]` لحماية الـ Controllers:

```csharp
using Microsoft.AspNetCore.Authorization;

[ApiController]
[Route("api/[controller]")]
[Authorize]  // ← هذا يحتاج JWT Token
public class ProductsController : ControllerBase
{
    // ...
}
```

---

## ✅ خلاصة الخطوات

1. ✅ إنشاء جدول Users
2. ✅ إنشاء Auth Service في API
3. ✅ إنشاء Auth Controller
4. ✅ تفعيل JWT في API
5. ✅ إنشاء Auth Service في Web
6. ✅ تحديث ApiClient
7. ✅ إنشاء صفحة Login
8. ✅ حماية Controllers

---

**آخر تحديث:** 2026-07-03 ✅
