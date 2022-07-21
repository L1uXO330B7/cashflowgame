using API.Filter;
using BLL.IServices;
using BLL.Services;
using Common.Model;
using DPL.EF;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

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
builder.Services.AddSwaggerGen();

// 註冊 DbContext
builder.Services.AddDbContext<CashFlowDbContext>(options =>
       options.UseSqlServer(builder.Configuration.GetConnectionString("OnlineCashFlow")));

// 註冊 Services
builder.Services.AddScoped<IUsersService<CreateUserArgs, string, string, string, string>, UsersService>();

// 註冊 MiniProfiler
builder.Services.AddMiniProfiler(options =>
{
    // ALL of this is optional. You can simply call .AddMiniProfiler() for all defaults
    // Defaults: In-Memory for 30 minutes, everything profiled, every user can see

    // Path to use for profiler URLs, default is /mini-profiler-resources
    options.RouteBasePath = "/profiler";

    // Control storage - the default is 30 minutes
    //(options.Storage as MemoryCacheStorage).CacheDuration = TimeSpan.FromMinutes(60);
    //options.Storage = new SqlServerStorage("Data Source=.;Initial Catalog=MiniProfiler;Integrated Security=True;");

    // Control which SQL formatter to use, InlineFormatter is the default
    options.SqlFormatter = new StackExchange.Profiling.SqlFormatters.SqlServerFormatter();

    // To control authorization, you can use the Func<HttpRequest, bool> options:
    //options.ResultsAuthorize = _ => !Program.DisableProfilingResults;
    //options.ResultsListAuthorize = request => MyGetUserFunction(request).CanSeeMiniProfiler;
    //options.ResultsAuthorizeAsync = async request => (await MyGetUserFunctionAsync(request)).CanSeeMiniProfiler;
    //options.ResultsAuthorizeListAsync = async request => (await MyGetUserFunctionAsync(request)).CanSeeMiniProfilerLists;

    // To control which requests are profiled, use the Func<HttpRequest, bool> option:
    //options.ShouldProfile = request => MyShouldThisBeProfiledFunction(request);

    // Profiles are stored under a user ID, function to get it:
    //options.UserIdProvider =  request => MyGetUserIdFunction(request);

    // Optionally swap out the entire profiler provider, if you want
    // The default handles async and works fine for almost all applications
    //options.ProfilerProvider = new MyProfilerProvider();

    // Optionally disable "Connection Open()", "Connection Close()" (and async variants).
    //options.TrackConnectionOpenClose = false;

    // Optionally use something other than the "light" color scheme.
    options.ColorScheme = StackExchange.Profiling.ColorScheme.Auto;

    // Enabled sending the Server-Timing header on responses
    options.EnableServerTimingHeader = true;

    // Optionally disable MVC filter profiling
    //options.EnableMvcFilterProfiling = false;
    // Or only save filters that take over a certain millisecond duration (including their children)
    //options.MvcFilterMinimumSaveMs = 1.0m;

    // Optionally disable MVC view profiling
    //options.EnableMvcViewProfiling = false;
    // Or only save views that take over a certain millisecond duration (including their children)
    //options.MvcViewMinimumSaveMs = 1.0m;

    // This enables debug mode with stacks and tooltips when using memory storage
    // It has a lot of overhead vs. normal profiling and should only be used with that in mind
    //options.EnableDebugMode = true;

    // Optionally listen to any errors that occur within MiniProfiler itself
    //options.OnInternalError = e => MyExceptionLogger(e);

    //options.IgnoredPaths.Add("/lib");
    //options.IgnoredPaths.Add("/css");
    //options.IgnoredPaths.Add("/js");
})
.AddEntityFramework(); // 監控 EntityFrameworkCore 生成的 SQL

var app = builder.Build();

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
        name: "RESTful API v1.0.0"
    );

    c.IndexStream = () => typeof(Program).GetTypeInfo()
                                          .Assembly
                                          .GetManifestResourceStream("MiniProfilerSwagger.index.html");
});
// }

app.UseAuthorization();

app.MapControllers();

app.Run();
