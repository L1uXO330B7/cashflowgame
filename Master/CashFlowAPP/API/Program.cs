using API;
using API.Filter;
using API.Hubs;
using BLL.IServices;
using BLL.Services.AdminSide;
using BLL.Services.ClientSide;
using Common.Methods;
using Common.Model.AdminSide;
using DPL.EF;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Events;
using System.Reflection;


// Serilog 程式註冊 ( 改到共用函式庫撰寫 )
Serilog.Log.Logger = Method.LogInit();

try
{
    // 建立 WebApplicationBuilder 物件
    var builder = WebApplication.CreateBuilder(args);

    // Serilog 組態 appsettings 註冊
    // Serilog.Log.Logger = new LoggerConfiguration()
    //        .ReadFrom.Configuration(builder.Configuration)
    //        .CreateLogger();

    builder.Host.UseSerilog(); // 啟動 Serilog
    Serilog.Log.Information("建立 WebApplicationBuilder 物件");

    // 註冊 DbContext

    var RootPath = System.IO.Directory.GetCurrentDirectory();
    string Conn;
    if (RootPath.ToUpper().Contains("DESK") || RootPath.ToUpper().Contains("WWW")|| RootPath.ToUpper().Contains("CODE"))
    {
        Conn = "OnlineCashFlow";
    }
    else
    {
        Conn = "LocalMDF";
    }

    builder.Services.AddDbContext<CashFlowDbContext>(options =>
           options.UseSqlServer(builder.Configuration.GetConnectionString(Conn)));

    // 加入 SignalR
    builder.Services.AddSignalR();

    // 註冊 Cors 策略
    //[CorsPolicy 指定來源](https://www.cnblogs.com/myzony/p/10511492.html)
    builder.Services.AddCors(options =>
    {
        options.AddPolicy("CorsPolicy", policy =>
        {
            policy.AllowAnyHeader()
                  .AllowAnyMethod()
                  .SetIsOriginAllowed((host) => true)
                  .AllowCredentials();
        });
    });

    // 註冊 AOP Filters
    builder.Services.AddMvc(config =>
    {
        config.Filters.Add(new ExceptionFilter());
        config.Filters.Add(new MiniProfilerActionFilter());
        config.Filters.Add(new ModelStateErrorActionFilter());
    });

    // Add services to the container.
    builder.Services.AddControllers()
        // 回傳資料大寫開頭 ( 預設小寫 )
        .AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy = null);

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

    // 註冊 Services

    builder.Services.AddScoped<IClientSideService, ClientSideService>();
    builder.Services.AddScoped<IClientHubService, ClientHubService>();

    // 註冊 Mapping CRUD Services
    new ServicesMapping(builder);

    // 註冊 MiniProfiler 如果更改設定需產生 MiniProfiler Script 貼於於 Swagger Index 內
    builder.Services.AddMiniProfiler(options =>
    {
        options.RouteBasePath = "/profiler";
        options.SqlFormatter = new StackExchange.Profiling.SqlFormatters.SqlServerFormatter();
        options.ColorScheme = StackExchange.Profiling.ColorScheme.Auto;
        options.EnableServerTimingHeader = true;
    })
    .AddEntityFramework(); // 監控 EntityFrameworkCore 生成的 SQL

    // 建立 WebApplication 物件
    var app = builder.Build();
    Serilog.Log.Information("建立 WebApplication 物件");

    // 該方法必須在 app.UseEndpoints 以前
    app.UseMiniProfiler();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment()) // 開發者模式
    {
        // 開發人員例外狀況頁面：在 Request Headers 中加入 Accept: text/html
        app.UseDeveloperExceptionPage();
    }

    app.UseSwagger();

    var SwaggerSettingRoot = builder.Configuration.GetSection("SwaggerSettingRoot").Value;

    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint(
            // 需配合 SwaggerDoc 的 name。 "/swagger/{SwaggerDoc name}/swagger.json"
            url: SwaggerSettingRoot,
            // 於 Swagger UI 右上角選擇不同版本的 SwaggerDocument 顯示名稱使用。
            name: "API v1.0.0"
        );

        c.IndexStream = () => typeof(Program).GetTypeInfo()
                                              .Assembly
                                              .GetManifestResourceStream("API.index.html");
    });

    // 參考 https://ithelp.ithome.com.tw/m/articles/10243891

    // 重寫 Url => AddRewrite 情境 1：隱藏參數降低資安風險
    // 讓 http://www.somebloghost.com/Blogs/Posts.aspx?Year=2006&Month=12&Day=10
    // 轉 http://www.somebloghost.com/Blogs/2006/Date/12/10/

    // Url 重新導向 => AddRedirect 情境 2：SEO 301 永久性轉址、302 暫時性轉址

    // 通過設定301重新導向，你可以告訴Google你的舊網址已經無效了，你希望搜尋引擎能夠將舊網址的流量轉移到新的URL頁面。此時搜尋引擎就會將舊網址的所有流量與排名一併的轉移到新的網址。

    // 302重新導向常用於A/B測試或是有短期的活動頁面而使用的。他會將原本的URL導向另一個URL，但是不會讓google的排名效果下降。不過如果時間拉長，長期使用302做導向的話，SEO的分數還是會受到影響！

    // 情境 3：搭配 Regex 做規則

    //var options = new RewriteOptions()
    //        .AddRewrite("Post.aspx", "api/Post", skipRemainingRules: true)
    //        .AddRedirect("Post.php", "api/Post");
    //app.UseRewriter(options);

    //app.UseRouting();

    app.UseSerilogRequestLogging(); // Log Level Information Request 的詳細資訊

    app.UseAuthorization();
    app.UseCors("CorsPolicy");
    app.MapControllers();

    // 配對 ChaHub
    app.MapHub<ChatHub>("/chatHub");

    // 啟動 ASP.NET Core 應用程式
    Serilog.Log.Information("啟動 ASP.NET Core 應用程式");
    app.Run();
}
catch (Exception ex)
{
    // 紀錄應用程式中未被捕捉的例外 (Unhandled Exception)
    Serilog.Log.Fatal(ex, "啟動 ASP.NET Core 應用程式，意外終止");
}
finally
{
    // 確保應用程式意外結束時，最後幾筆 Log 紀錄確實寫入 Sinks 中
    Serilog.Log.CloseAndFlush();
}