using Microsoft.OpenApi.Models;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

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

builder.Services.AddMiniProfiler(o =>
{
    o.RouteBasePath = "/profiler";
})
.AddEntityFramework();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
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
