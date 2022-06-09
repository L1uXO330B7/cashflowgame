using System.Net.WebSockets;
using WebSocketDemo;

var builder = WebApplication.CreateBuilder(args);

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

// 參考 https://blog.darkthread.net/blog/aspnet-core-websocket-chatroom/
// WebSocket

// 加入WebSocket處理服務
builder.Services.AddSingleton<WebSocketHandler>();

// 加入 WebSocket 功能
app.UseWebSockets(new WebSocketOptions
{
    KeepAliveInterval = TimeSpan.FromSeconds(30)
});

// 覺得用 Controller 接收 WebSocket 太複雜，改在 Middleware 層處理
// 可以參考 https://stackoverflow.com/questions/67768461/how-to-connect-to-websocket-through-controller
app.Use(async (context, next) =>
{
    if (context.Request.Path == "/ws")
    {
        if (context.WebSockets.IsWebSocketRequest)
        {
            using (WebSocket ws = await context.WebSockets.AcceptWebSocketAsync())
            {
                var wsHandler = context.RequestServices.GetRequiredService<WebSocketHandler>();
                await wsHandler.ProcessWebSocket(ws);
            }
        }
        else
            context.Response.StatusCode = (int)System.Net.HttpStatusCode.BadRequest;
    }
    else await next();
});

// 應用程式啟動
app.Run();
