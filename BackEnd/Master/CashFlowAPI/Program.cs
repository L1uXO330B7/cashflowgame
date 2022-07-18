using CashFlowAPI.Filter;

var builder = WebApplication.CreateBuilder(args);

// µù¥U Db
//var ConnectionString = builder.Configuration.GetConnectionString("");
//builder.Services.AddDbContext<>(options =>
//       options.UseSqlServer(ConnectionString));

// AOP ¿z¿ï¾¹
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

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
