using BaseCommon;
using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Text;

namespace WebSocketDemo
{
    public class WebSocketHandler : ControllerHandler
    {
        private readonly ILogger<WebSocketHandler> _logger;

        // 參考 https://blog.darkthread.net/blog/aspnet-core-websocket-chatroom/

        ConcurrentDictionary<int, WebSocket> WebSockets; // 放置連線資訊的容器，可以讓"多個執行緒併發呼叫"

        public WebSocketHandler(ILogger<WebSocketHandler> logger)
        {
            _logger = logger;
            WebSockets = new ConcurrentDictionary<int, WebSocket>();
        }

        /// <summary>
        /// 處理連線，每次有連線就分別做一次，但容器只有一個
        /// </summary>
        /// <param name="webSocket"></param>
        /// <returns></returns>
        public async Task ProcessWebSocket(WebSocket webSocket)
        {
            // TryAdd 嘗試加入回傳布林直，可省去 TryCatch 判斷 ( 但也會不知道錯誤在哪 ? )
            // GetHashCode 確定一個物件的唯一值
            WebSockets.TryAdd(webSocket.GetHashCode(), webSocket);

            var buffer = new byte[1024 * 4];
            // ReceiveAsync( 所接收資料的緩衝區( 快取記憶體 ? ) , SocketFlags 值的位元組合，會在接收資料時使用 )
            // 開始非同步要求，以接收來自已連接的 Socket 物件的資料，傳回以收到的位元組數目完成的非同步工作 Task<Int32>
            var res = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            var userName = "anonymous";

            // WebSocketReceiveResult.CloseStatus.HasValue 偵測連線是否中斷
            while (!res.CloseStatus.HasValue)
            {
                // 將 Byte 編碼為中文字串
                var cmd = Encoding.UTF8.GetString(buffer, 0, res.Count);

                if (!string.IsNullOrEmpty(cmd))
                {
                    // 格式化並寫入資訊記錄訊息 Log
                    _logger.LogInformation(cmd);

                    // StartsWith 檢查字串是否是以指定子字符串開頭
                    if (cmd.StartsWith("/USER "))
                    {
                        // Substring 從這個執行個體擷取子字串
                        // 指定在這個連線中的 userName
                        userName = cmd.Substring(6);
                    }
                    else
                    {
                        // 推撥
                        Broadcast($"{userName}:\t{cmd}");
                    }
                }

                // 計算連線數量
                ConnectionsCount = WebSockets.Count();

                // While 前再次刷新非同步要求
                res = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            }

            // 關閉連線
            await webSocket.CloseAsync(res.CloseStatus.Value, res.CloseStatusDescription, CancellationToken.None);

            // 移出容器
            WebSockets.TryRemove(webSocket.GetHashCode(), out var removed);

            // 向容器其他連線推撥訊息
            Broadcast($"{userName} left the room.");
        }

        /// <summary>
        /// 推撥訊息
        /// </summary>
        /// <param name="message"></param>
        public void Broadcast(string message)
        {
            var buff = Encoding.UTF8.GetBytes($"{DateTime.Now:MM-dd HH:mm:ss}\t{message}");
            var data = new ArraySegment<byte>(buff, 0, buff.Length);

            // 平行處理
            Parallel.ForEach(WebSockets.Values, async (webSocket) =>
            {
                if (webSocket.State == WebSocketState.Open)
                {
                    // 推撥訊息
                    await webSocket.SendAsync(data, WebSocketMessageType.Text, true, CancellationToken.None);
                }
            });
        }
    }
}
