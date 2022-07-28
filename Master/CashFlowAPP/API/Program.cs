using API.Filter;
using BLL.IServices;
using BLL.Services.AdminSide;
using BLL.Services.ClientSide;
using Common.Model;
using DPL.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// 註冊 Cors 策略
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", policy =>
    {
        policy
        .AllowAnyOrigin()
        .AllowAnyHeader()
        .AllowAnyMethod();
    });
}
);
// 註冊 AOP Filters
builder.Services.AddMvc(config =>
{
    config.Filters.Add(new ExceptionFilter());
    config.Filters.Add(new MiniProfilerActionFilter());
});

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

// 自動產生 Swagger Xml 設定 https://www.tpisoftware.com/tpu/articleDetails/1372
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc(
                    // 攸關 SwaggerDocument 的 URL 位置。
                    name: "v1",
                    // 是用於 SwaggerDocument 版本資訊的顯示 ( 內容非必填 )。
                    info: new OpenApiInfo
                    {
                        Title = "API 文件",
                        Version = "1.0.0",
                        Description = "人生就像射箭，箭靶子都找不到努力拉弓有什麼意義",
                        TermsOfService = new Uri("https://editor.swagger.io/"),
                        Contact = new OpenApiContact
                        {
                            Name = "Swagger 編輯器",
                            Url = new Uri("https://editor.swagger.io/"),
                            Email = "carl123321@gmail.com"
                        },
                        License = new OpenApiLicense 
                        {
                            Name = "Furion 應用參考",
                            Url = new Uri("https://dotnetchina.gitee.io/furion/docs")
                        }
                    }
    );

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description =
        $@"JWT Authorization header 使用 Bearer 格式。\r\n\r\n 
           請輸入 'Bearer' [空白鍵] 和你的 Token 在輸入框內。\r\n\r\n
           Example: Bearer 12345abcdef",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
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
                In = ParameterLocation.Header,
             },
             new List<string>()
        }
    });

    var FilePath = Path.Combine(AppContext.BaseDirectory, "API.xml");
    c.IncludeXmlComments(FilePath);
});

// 註冊 DbContext
builder.Services.AddDbContext<CashFlowDbContext>(options =>
       options.UseSqlServer(builder.Configuration.GetConnectionString("OnlineCashFlow")));

// 註冊 Services
builder.Services.AddScoped<IUsersService<CreateUserArgs, int?, UpdateUserArgs, int?>, UsersService>();
builder.Services.AddScoped<IClientSideService, ClientSideService>();

// 註冊 MiniProfiler 如果更改設定需產生 MiniProfiler Script 貼於於 Swagger Index 內
builder.Services.AddMiniProfiler(options =>
{
    options.RouteBasePath = "/profiler";
    options.SqlFormatter = new StackExchange.Profiling.SqlFormatters.SqlServerFormatter();
    options.ColorScheme = StackExchange.Profiling.ColorScheme.Auto;
    options.EnableServerTimingHeader = true;
})
.AddEntityFramework(); // 監控 EntityFrameworkCore 生成的 SQL

var app = builder.Build();

// 該方法必須在 app.UseEndpoints 以前
app.UseMiniProfiler();

// Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment()) // 開發者模式
// {
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint(
        // 需配合 SwaggerDoc 的 name。 "/swagger/{SwaggerDoc name}/swagger.json"
        url: "/swagger/v1/swagger.json",
        // 於 Swagger UI 右上角選擇不同版本的 SwaggerDocument 顯示名稱使用。
        name: "API v1.0.0"
    );

    c.IndexStream = () => typeof(Program).GetTypeInfo()
                                          .Assembly
                                          .GetManifestResourceStream("API.index.html");
});
// }

app.UseAuthorization();
app.UseCors("CorsPolicy");
app.MapControllers();
app.Run();
