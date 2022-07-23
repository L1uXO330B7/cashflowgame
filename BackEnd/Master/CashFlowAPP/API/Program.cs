using API.Filter;
using BLL.IServices;
using BLL.Services;
using Common.Model;
using DPL.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// ���U AOP Filters
builder.Services.AddMvc(config =>
{
    config.Filters.Add(new ExceptionFilter());
    config.Filters.Add(new MiniProfilerActionFilter());
});

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc(
                    // ���� SwaggerDocument �� URL ��m�C
                    name: "v1",
                    // �O�Ω� SwaggerDocument ������T����� ( ���e�D���� )�C
                    info: new OpenApiInfo
                    {
                    }
    );
});

// ���U DbContext
builder.Services.AddDbContext<CashFlowDbContext>(options =>
       options.UseSqlServer(builder.Configuration.GetConnectionString("OnlineCashFlow")));

// ���U Services
builder.Services.AddScoped<IUsersService<CreateUserArgs, int?, UpdateUserArgs, int?>, UsersService>();

// ���U MiniProfiler
builder.Services.AddMiniProfiler(options =>
{
    options.RouteBasePath = "/profiler";
    options.SqlFormatter = new StackExchange.Profiling.SqlFormatters.SqlServerFormatter();
    options.ColorScheme = StackExchange.Profiling.ColorScheme.Auto;
    options.EnableServerTimingHeader = true;
})
.AddEntityFramework(); // �ʱ� EntityFrameworkCore �ͦ��� SQL

var app = builder.Build();

// �Ӥ�k�����bapp.UseEndpoints�H�e
app.UseMiniProfiler();

// Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment()) // �}�o�̼Ҧ�
// {
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint(
        // �ݰt�X SwaggerDoc �� name�C "/swagger/{SwaggerDoc name}/swagger.json"
        url: "/swagger/v1/swagger.json",
        // �� Swagger UI �k�W����ܤ��P������ SwaggerDocument ��ܦW�٨ϥΡC
        name: "RESTful API v1.0.0"
    );

    c.IndexStream = () => typeof(Program).GetTypeInfo()
                                          .Assembly
                                          .GetManifestResourceStream("API.index.html");
});
// }

app.UseAuthorization();

app.MapControllers();

app.Run();
