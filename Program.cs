using Hangfire;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using OfficeOpenXml;
using Payment.Applications.Features.Merchant.Commands;
using Payment.Applications.Interface;
using Payment.Persistence.Persist;
using Payment.Service.VnPay.Config;
using Payment.Service.Zalopay.Config;
using System.Reflection;
using System.Text;
using thuongmaidientus1.AccountService;
using thuongmaidientus1.EmailConfig;
using thuongmaidientus1.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#region CORS
var corsBuilder = new CorsPolicyBuilder();
corsBuilder.AllowAnyHeader();
corsBuilder.AllowAnyMethod();
corsBuilder.AllowAnyOrigin();
corsBuilder.WithOrigins("http://localhost:3000", "http://localhost:5173"); // Đây là Url bên frontEnd
corsBuilder.AllowCredentials();
builder.Services.AddCors(options =>
{
    options.AddPolicy("SiteCorsPolicy", corsBuilder.Build());
});

#endregion

#region JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Issuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});
#endregion

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "CoreJwtExample",
        Version = "v1",
        Description = "Hello Anh Em",
        Contact = new Microsoft.OpenApi.Models.OpenApiContact()
        {
            Name = "Thanh Toán Online",
            Url = new Uri("https://github.com/")
        }
    });



    // Phần xác thực người dùng(JWT)
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = @"JWT Authorization header using the Bearer scheme. Enter 'Bearer' [space] and then your token in the text input below.
                        Example: 'Bearer asddvsvs123'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Scheme = "Bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header
            },
            new List<string>()
        }
    });

    var xmlFileName = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var path = Path.Combine(AppContext.BaseDirectory, xmlFileName);
    c.IncludeXmlComments(path);
});

var emailSettings = new EmailSetting
{
    SmtpServer = "smtp.gmail.com",
    Port = 587,
    UseSsl = true,
    Username = "vantruong08062002@gmail.com",
    Password = "ijzaeymzhfpwpftd",
    SenderName = "ABC",
    SenderEmail = "vantruong0862002@gmail.com"
};

var connection = builder.Configuration.GetConnectionString("MyDB");
builder.Services.AddDbContext<DBContexts>(option =>
{
    option.UseSqlServer(connection); // "ThuongMaiDienTu" đây là tên của project, vì tách riêng model khỏi project sang 1 lớp khác nên phải để câu lệnh này "b => b.MigrationsAssembly("ThuongMaiDienTu")"
});

// Khai báo môi trường Excel để lưu dữ liệu vào file Excel
ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

builder.Services.AddAuthentication(); // Sử dụng phân quyền
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.Configure<Jwt>(builder.Configuration.GetSection("Jwt"));
builder.Services.Configure<EmailSetting>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.Configure<VnpayConfig>(builder.Configuration.GetSection("Vnpay"));
//builder.Services.Configure<ZaloPayConfig>(builder.Configuration.GetSection("ZaloPay"));
builder.Services.AddScoped<EmailSetting>();
builder.Services.Configure<ZaloPayConfig>(builder.Configuration.GetSection(ZaloPayConfig.ConfigName));
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IShopService, ShopService>();
builder.Services.AddScoped<IExcelService, ExcelService>();
builder.Services.AddScoped<IShopVanChuyenService, ShopVanChuyenService>();
builder.Services.AddScoped<IVanChuyenService, VanChuyenService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<ISqlService, SqlService>();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
builder.Services.AddScoped<IConnectionService, ConnectionService>();
builder.Services.AddScoped<ICommentService, CommentService>();
builder.Services.AddScoped<IDanhGiaService, DanhGiaService>();

// Sử dụng Lazy này để gọi các service với nhau, ở đây đang gọi Service "ICommentService" trong Service "IDanhGiaService" và ngược lại gọi Service "IDanhGiaService" trong Service "ICommentService", nếu muốn gọi lại các Service khác với nhau thì khai báo thêm
builder.Services.AddScoped(provider => new Lazy<ICommentService>(() => provider.GetService<ICommentService>()));
builder.Services.AddScoped(provider => new Lazy<IDanhGiaService>(() => provider.GetService<IDanhGiaService>()));


builder.Services.AddHttpContextAccessor();
builder.Services.AddControllersWithViews();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddHttpContextAccessor();
builder.Services.AddMvc();
builder.Services.AddHangfire(configuration => configuration
                    .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                    .UseSimpleAssemblyNameTypeSerializer()
                    .UseRecommendedSerializerSettings()
                    .UseSqlServerStorage(builder.Configuration.GetConnectionString("MyDB"),
                    new Hangfire.SqlServer.SqlServerStorageOptions()
                    {
                        // TODO: Change hangfire sql server option
                    }));
builder.Services.AddHangfireServer();
builder.Services.AddMediatR(r =>
{
    r.RegisterServicesFromAssembly(typeof(CreateMerchant).Assembly);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseHangfireDashboard();
app.UseCookiePolicy();
app.UseRouting();
app.UseCors("SiteCorsPolicy");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
