using System.Net.WebSockets;
using WebSocketDemo;

var Builder = WebApplication.CreateBuilder(args);

// Add services to the container.

Builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
Builder.Services.AddEndpointsApiExplorer();
Builder.Services.AddSwaggerGen();
// 加入WebSocket處理服務
Builder.Services.AddSingleton<WebSocketHandler>();

Builder.Services.AddSignalR();

var App = Builder.Build();
// 中介層參考 https://blog.darkthread.net/blog/aspnetcore-middleware-lab/

// Configure the HTTP request pipeline.
if (App.Environment.IsDevelopment())
{
    App.UseSwagger();
    App.UseSwaggerUI();
}

App.UseAuthorization();

App.MapControllers();

//連接 SignalR 的路由與配對的 Hub
App.MapHub<SignalRHub>("/SignalRHub"); 

// WebSocket 參考 https://blog.darkthread.net/blog/aspnet-core-websocket-chatroom/

// 加入 WebSocket 功能
App.UseWebSockets(new WebSocketOptions
{
    KeepAliveInterval = TimeSpan.FromSeconds(30)
});

// 覺得用 Controller 接收 WebSocket 太複雜，改在 Middleware 層處理
// 可以參考 https://stackoverflow.com/questions/67768461/how-to-connect-to-websocket-through-controller
// 感覺應該再透過一個中介層處理，或者不會被 swagger 吃到的 controller ，畢竟 swagger 本來就是為了 restfull 而生
App.Use(async (context, next) =>
{
    // 對應到這個路徑就是 for WebSocket
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
App.Run();
