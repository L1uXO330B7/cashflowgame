using BaseCommon;
using Microsoft.AspNetCore.Mvc;

namespace WebSocketDemo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WebSocketController : ControllerHandler
    {
        private readonly ILogger<WebSocketController> _logger;

        /// <summary>
        /// 取得 WebSocket 連線數量
        /// </summary>
        /// <returns></returns>
        [Route("GetConnectionsCount")]
        [HttpGet]
        public int Get()
        {
            return WebSockets.Count;
        }

        /// <summary>
        /// Swagger 使用 WebSock 方式
        /// </summary>
        /// <returns></returns>
        [Route("ws")]
        [HttpGet]
        public async Task WebSocketGet()
        {
            if (HttpContext.WebSockets.IsWebSocketRequest)
            {
                using (var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync())
                {
                   await new WebSocketHandler(null).ProcessWebSocket(webSocket);
                }
            }
            else
            {
                HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
            }
        }
    }
}