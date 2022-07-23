using Microsoft.AspNetCore.Mvc;
using System.Collections.Concurrent;
using System.Net.WebSockets;

namespace BaseCommon
{
    public class ControllerHandler : ControllerBase
    {
        public static ConcurrentDictionary<int, WebSocket> WebSockets; // 放置連線資訊的容器，可以讓"多個執行緒併發呼叫"
    }
}