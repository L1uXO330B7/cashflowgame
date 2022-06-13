using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace WebSocketDemo
{
    public class SignalRHub : Hub
    {
        public Task SendMessage(string User, string Message)
        {
            // All 在所有連線的用戶端上呼叫方法 Caller、Others etc .. 
            return Clients.All.SendAsync("ReceiveMessage", User, Message);
        }
    }
}
