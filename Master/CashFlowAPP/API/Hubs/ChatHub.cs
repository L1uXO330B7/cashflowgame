using API.Filter;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using static Common.Model.ClientSideModel;

namespace API.Hubs
{
    /// <summary>
    /// 實作 WebSocket through SignalR
    /// </summary>

    public class ChatHub : Hub
    {
        /// <summary>
        /// 賤狗仔
        /// </summary>
        public ChatHub()
         {
            //var info = Context.GetHttpContext();
            //var Authorization = info.Request.Headers["Authorization"];
        }

        /// <summary>
        /// 建立連線
        /// </summary>
        public static List<string> ConnIDList = new List<string>();

        /// <summary>
        /// 連線事件
        /// </summary>
        /// <returns></returns>
        public override async Task OnConnectedAsync()
        {
            //var info = Context.GetHttpContext();
            //var Authorization = info.Request.Headers["Authorization"];
            //var JwtObject = Jose.JWT.Decode<UserInfo>(
            //           JwtToken, Encoding.UTF8.GetBytes("錢董"),
            //           Jose.JwsAlgorithm.HS256);
            if (ConnIDList.Where(p => p == Context.ConnectionId).FirstOrDefault() == null)
            {
                ConnIDList.Add(Context.ConnectionId);
            }

            // 更新連線 ID 列表
            string jsonString = JsonConvert.SerializeObject(ConnIDList);
            await Clients.All.SendAsync("UpdList", jsonString);

            // 更新個人 ID
            await Clients.Client(Context.ConnectionId).SendAsync("UpdSelfID", Context.ConnectionId);

            // 更新聊天內容
            await Clients.All.SendAsync("UpdContent", "新連線 ID: " + Context.ConnectionId);

            await base.OnConnectedAsync();
        }

        /// <summary>
        /// 離線事件
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        public override async Task OnDisconnectedAsync(Exception ex)
        {

            string? id = ConnIDList.Where(p => p == Context.ConnectionId).FirstOrDefault();

            if (id != null)
            {
                ConnIDList.Remove(id);
            }

            // 更新連線 ID 列表
            string jsonString = JsonConvert.SerializeObject(ConnIDList);
            await Clients.All.SendAsync("UpdList", jsonString);

            // 更新聊天內容
            await Clients.All.SendAsync("UpdContent", "已離線 ID: " + Context.ConnectionId);

            await base.OnDisconnectedAsync(ex);
        }

        /// <summary>
        /// 傳遞訊息
        /// </summary>
        /// <param name="user"></param>
        /// <param name="message"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task SendMessage(FromClientChat package)
        {
            //string selfID, string message, string sendToID
            if (string.IsNullOrEmpty(package.sendToID))
            {
                await Clients.All.SendAsync("UpdContent", package.selfID + " 說: " + package.message);
            }
            else
            {
                // 接收人
                await Clients.Client(package.sendToID).SendAsync("UpdContent", package.selfID + " 私訊向你說: " + package.message);

                // 發送人
                await Clients.Client(Context.ConnectionId).SendAsync("UpdContent", "你向 " + package.sendToID + " 私訊說: " + package.message);
            }
        }
    }
}
