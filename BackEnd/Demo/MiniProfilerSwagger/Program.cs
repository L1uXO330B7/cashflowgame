using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using MiniProfilerSwagger.EF;
using MiniProfilerSwagger.Filter;
using StackExchange.Profiling.Storage;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);
// 註冊db
var ConnectionString = builder.Configuration.GetConnectionString("MiniProfilerDb");
builder.Services.AddDbContext<MiniProfilerDbContext>(options =>
       options.UseSqlServer(ConnectionString));

// AOP 篩選器
builder.Services.AddMvc(config =>
{
    config.Filters.Add(new MiniProfilerActionFilter());
});

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc(
                    // 攸關 SwaggerDocument 的 URL 位置。
                    name: "v1",
                    // 是用於 SwaggerDocument 版本資訊的顯示 ( 內容非必填 )。
                    info: new OpenApiInfo
                    {
                    }
    );
});

// 注入MiniProfiler
builder.Services.AddMiniProfiler(o =>
{
    // 訪問地址路由根目錄；預設為：/mini-profiler-resources
    o.RouteBasePath = "/profiler";
    // 資料快取時間
    (o.Storage as MemoryCacheStorage).CacheDuration = TimeSpan.FromMinutes(60);
    // sql 格式化設定
    o.SqlFormatter = new StackExchange.Profiling.SqlFormatters.InlineFormatter();
    // 跟蹤連線開啟關閉
    o.TrackConnectionOpenClose = true;
    // 介面主題顏色方案;預設淺色
    o.ColorScheme = StackExchange.Profiling.ColorScheme.Dark;
    // .net core 3.0以上：對MVC過濾器進行分析
    //o.EnableMvcFilterProfiling = true;
    // 對檢視進行分析
    //o.EnableMvcViewProfiling = true;

    // 控制訪問頁面授權，預設所有人都能訪問
    //o.ResultsAuthorize;
    // 要控制分析哪些請求，預設說有請求都分析
    //o.ShouldProfile;

    // 內部異常處理
    o.OnInternalError = e => Console.WriteLine(e);
})
// 監控 EntityFrameworkCore 生成的 SQL
.AddEntityFramework();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // 該方法必須在app.UseEndpoints以前
    app.UseMiniProfiler();

    app.UseSwagger();
    app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint(
                // 需配合 SwaggerDoc 的 name。 "/swagger/{SwaggerDoc name}/swagger.json"
                url: "/swagger/v1/swagger.json",
                // 於 Swagger UI 右上角選擇不同版本的 SwaggerDocument 顯示名稱使用。
                name: "RESTful API v1.0.0"
            );

            c.IndexStream = () => typeof(Program).GetTypeInfo()
                                                  .Assembly
                                                  .GetManifestResourceStream("MiniProfilerSwagger.index.html");
        });
}

app.UseAuthorization();

app.MapControllers();

app.Run();
