using BLL.IServices;
using Common.Model;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using System.Text;

namespace API.Hubs
{
    /// <summary>
    /// 實作 WebSocket through SignalR ( 這裡應該只實作 Hub Like Controller 業務邏輯請交給 Service )
    /// </summary>
    public class Hubs : Hub
    {
        private IClientHubService _ClientHubService;
        /// <summary>
        /// 建構子
        /// </summary>
        public Hubs(IClientHubService ClientHubService)
        {
            _ClientHubService = ClientHubService;
        }

        /// <summary>
        /// 連線ID清單
        /// </summary>
        public static List<string> ConnIDList = new List<string>();

        /// <summary>
        /// 連線User清單
        /// </summary>
        public static List<UserInfo> UserList = new List<UserInfo>();

        /// <summary>
        /// 使用者物件 Id 信箱 姓名 ...
        /// </summary>
        public static UserInfo _UserObject = new UserInfo();

        /// <summary>
        /// 連線事件
        /// </summary>
        /// <returns></returns>
        public override async Task OnConnectedAsync()
        {
            var Token = Context.GetHttpContext().Request.Query["token"];
            if (Token.Count == 0)
            {
                var Stranger = Context.GetHttpContext().Request.Query["stranger"];
                _UserObject.Name = (string)Stranger + "$$$";
            }
            else
            {
                //string value = !string.IsNullOrEmpty(Token.ToString()) ? Token.ToString() : "default";
                _UserObject = Jose.JWT.Decode<UserInfo>(
                       Token, Encoding.UTF8.GetBytes("錢董"),
                       Jose.JwsAlgorithm.HS256);
            }

            if (ConnIDList.Where(p => p == Context.ConnectionId).FirstOrDefault() == null)
            {
                ConnIDList.Add(Context.ConnectionId);
            }

            if (_UserObject != null)
            {
                UserList.Add(_UserObject);
            }

            // 更新連線 ID 列表
            string jsonString = JsonConvert.SerializeObject(ConnIDList);
            await Clients.All.SendAsync("UpdList", jsonString, UserList.Select(x => x.Name).ToList());

            // 更新個人 ID
            await Clients.Client(Context.ConnectionId).SendAsync("UpdSelfID", Context.ConnectionId, UserList.Where(x => x.Id == _UserObject.Id).FirstOrDefault());

            // 更新聊天內容
            await Clients.All.SendAsync("UpdContent", "新連線玩家: " + _UserObject.Name);

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
            var User = UserList.Where(user => user.Id == _UserObject.Id).FirstOrDefault();

            if (id != null)
            {
                UserList.Remove(User);
                ConnIDList.Remove(id);
            }

            // 更新連線 ID 列表
            string jsonString = JsonConvert.SerializeObject(ConnIDList);
            await Clients.All.SendAsync("UpdList", jsonString, UserList.Select(x => x.Name).ToList());

            // 更新聊天內容
            await Clients.All.SendAsync("UpdContent", "已離線玩家: " + _UserObject.Name);

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

        /// <summary>
        /// 抽卡
        /// </summary>
        /// <returns></returns>
        public async Task DrawCard()
        { 

            // 寫一隻 Service 執行抽卡
        }

        /// <summary>
        /// 抽卡結果回傳
        /// </summary>
        /// <returns></returns>
        public async Task CardResult()
        {
            // 寫一隻 Service 執行抽卡結果判斷與影響
        }
    }
}
