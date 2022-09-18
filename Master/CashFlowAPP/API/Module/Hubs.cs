using BLL.IServices;
using Common.Methods;
using Common.Model;
using DPL.EF;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using System.Text;
using System.Timers;
namespace API.Hubs
{
    /// <summary>
    /// 實作 WebSocket through SignalR ( 這裡應該只實作 Hub Like Controller 業務邏輯請交給 Service )
    /// </summary>
    public class Hubs : Hub
    {
        private IClientHubService _ClientHubService;
        private readonly IHubContext<Hubs> _hubContext;
        private readonly CashFlowDbContext _CashFlowDbContext;
        private System.Timers.Timer timer;
        public static Boolean TimerTrigger = false;
        public static List<UserInfo> _UserInfos = new List<UserInfo>(); // 連線 User 清單
        public static List<RandomItem<int>> CardList = new List<RandomItem<int>>();
        public static List<Card> Cards = new List<Card>();

        /// <summary>
        /// 建構子
        /// </summary>
        public Hubs(
            IClientHubService ClientHubService,
            IHubContext<Hubs> hubContext,
            CashFlowDbContext cashFlowDbContext,
            IMemoryCache memoryCache
        )
        {
            _ClientHubService = ClientHubService;
            _CashFlowDbContext = cashFlowDbContext;
            _hubContext = hubContext;

            Cards = _CashFlowDbContext.Cards.AsNoTracking().ToList();
            CardList = Cards.Select(x => new RandomItem<int>
            {
                SampleObj = x.Id,
                Weight = (decimal)x.Weight
            }).ToList();

            if (!TimerTrigger)
            {
                TimerInit();
                TimerTrigger = true;
            }
        }

        /// <summary>
        /// 連線事件
        /// </summary>
        /// <returns></returns>
        public override async Task OnConnectedAsync()
        {

            if (!_UserInfos.Select(x => x.ConnectionId).Any(x => x == Context.ConnectionId)) // 第一次連線
            {

                // 做 _MemoryCache FiInfo Init
                // FiInfo UserId 改 string
                // 遊客塞 ConnectId，用戶塞 UserId
                // 一建立連線就返回資料
                // iis 啟動只有第一個位連線受影響，倒數完才看得到資料
                // 

                var _UserInfo = new UserInfo();


                var Token = Context.GetHttpContext().Request.Query["token"];
                if (Token.Count == 0) // 遊客
                {
                    var Stranger = Context.GetHttpContext().Request.Query["stranger"]; // 暱稱
                    _UserInfo.Name = (string)Stranger + "$$$";
                }
                else // 用戶
                {
                    _UserInfo = Jose.JWT.Decode<UserInfo>(
                           Token, Encoding.UTF8.GetBytes("錢董"),
                           Jose.JwsAlgorithm.HS256);
                }



                _UserInfo.ConnectionId = Context.ConnectionId;

                // 同個 UserId 不能同時連線，會將前者離線在連後者
                var RepeatUserId = _UserInfos.Select(x => x.UserId).FirstOrDefault(x => x == _UserInfo.UserId);
                if (RepeatUserId != 0)
                {
                    var RepeatUserInfo = _UserInfos.FirstOrDefault(x => x.UserId == RepeatUserId);
                    // 前端離線
                    await _hubContext.Clients.Client(RepeatUserInfo.ConnectionId).SendAsync("Relogin", $"此帳號已被從別處重複登入");
                }

                // 拿到玩家財務報表，並初始化快取
                var UserFiInfo = await _ClientHubService.ReadFiInfo(_UserInfo.UserId, _UserInfo.ConnectionId);





                // 個人財報回傳前端
                await _hubContext.Clients.Client(_UserInfo.ConnectionId)
                    .SendAsync("ReadFiInfo", UserFiInfo);

                _UserInfos.Add(_UserInfo);
                // 更新聊天內容
                await Clients.All.SendAsync("UpdContent", DateTime.Now.ToString("[yyyy/MM/dd HH:mm:ss]") + "新連線玩家: " + _UserInfo.Name);

                // 取得排行榜最高玩家

                var TopUserInBoard = await _ClientHubService.TopUserInBoard(_UserInfos);
                await _hubContext.Clients.All.SendAsync("TopUserInBoard", TopUserInBoard);

                // 取得交易所清單
                var AssetTransactionList = await _ClientHubService.GetAssetTransactionList();
                await _hubContext.Clients.All.SendAsync("AssetTransactionList", AssetTransactionList);
            }

            await base.OnConnectedAsync();
        }

        /// <summary>
        /// 離線事件
        /// https://www.cnblogs.com/monster17/p/13537129.html
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        public override async Task OnDisconnectedAsync(Exception ex)
        {
            if (_UserInfos.Select(x => x.ConnectionId).Any(x => x == Context.ConnectionId))
            {
                var _UserInfo = _UserInfos.FirstOrDefault(x => x.ConnectionId == Context.ConnectionId);

                if (_UserInfo.UserId != 0)
                {
                    _ClientHubService.SavingBoard(_UserInfo.UserId);
                }

                _UserInfos.Remove(_UserInfo);

                // 更新聊天內容
                await Clients.All.SendAsync("UpdContent", $"{GetNowSrring()} 已離線玩家:{_UserInfo.Name}");
            }

            await base.OnDisconnectedAsync(ex);
        }

        #region Invoke

        /// <summary>
        /// 傳遞訊息
        /// </summary>
        /// <returns></returns>
        public async Task SendMessage(FromClientChat package)
        {
            await Clients.All.SendAsync("UpdContent", DateTime.Now.ToString("[yyyy/MM/dd HH:mm:ss]") + package.UserName + $"說: " + package.message);
        }

        /// <summary>
        /// 計時器初始化
        /// </summary>
        public void TimerInit()
        {
            var flag = true;
            var Count = 0;
            // Create a timer with a two second interval.
            while (flag)
            {
                var time1 = DateTime.Now.Second;

                if (DateTime.Now.Second % 60 == 0)
                {
                    var time = DateTime.Now.Second;
                    timer = new System.Timers.Timer(60000);
                    Count++;
                    flag = false;
                }
            }
            var test = Count;

            // Hook up the Elapsed event for the timer. 
            timer.Elapsed += OnTimedEvent;
            timer.Enabled = true;
        }

        /// <summary>
        /// 計時器觸發抽卡
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        public async void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            try
            {
                timer.Stop();

                foreach (var _UserInfo in _UserInfos)
                {
                    // 透過後端儲存抽到的卡片，前端只負責顯示
                    var CardByRandom = Method.RandomWithWeight(CardList);
                    var YourCard = Cards.FirstOrDefault(x => x.Id == CardByRandom);
                    var CardInfo = await _ClientHubService.ProcessCardInfo(YourCard, _UserInfos, _UserInfo.UserId, _UserInfo.ConnectionId);







                    dynamic CardValue = CardInfo.Value;
                    if (YourCard.Type == "強迫中獎")
                    {
                        await _hubContext.Clients.All.SendAsync("UpdContent", $"玩家: {_UserInfo.Name} 抽到 {YourCard.Name}");
                        CardValue = "恭喜你造成蝴蝶效應";
                    }

                    await _hubContext.Clients.Client(_UserInfo.ConnectionId).SendAsync("DrawCard", YourCard, $"{CardValue}");

                    var UserFiInfo = await _ClientHubService.ReadFiInfo(_UserInfo.UserId, _UserInfo.ConnectionId);

                    await _hubContext.Clients.Client(_UserInfo.ConnectionId)
                   .SendAsync("ReadFiInfo", UserFiInfo);


                    // 取得排行榜最高玩家

                    var TopUserInBoard = await _ClientHubService.TopUserInBoard(_UserInfos);

                    await _hubContext.Clients.All.SendAsync("TopUserInBoard", TopUserInBoard);
                }

                timer.Start();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// 卡片抉擇，結果回傳
        /// </summary>
        /// <returns></returns>
        /// 
        public async Task ChoiceOfCard(int okay)
        {
            var UserId = _UserInfos
                .FirstOrDefault(x => x.ConnectionId == Context.ConnectionId).UserId;

            var YourFiInfo = await _ClientHubService.ChoiceOfCard(UserId, Context.ConnectionId);

            await _hubContext.Clients.Client(Context.ConnectionId)
          .SendAsync("ReadFiInfo", YourFiInfo);


            // 取得排行榜最高玩家

            var TopUserInBoard = await _ClientHubService.TopUserInBoard(_UserInfos);

            await _hubContext.Clients.All.SendAsync("TopUserInBoard", TopUserInBoard);
        }

        /// <summary>
        /// 賣或上架資產
        /// </summary>
        /// <param name="Asset"></param>
        /// <returns></returns>
        public async Task AssetSales(SaleAsset Asset)
        {
            // 賣正資產
            var UserId = _UserInfos
               .FirstOrDefault(x => x.ConnectionId == Context.ConnectionId).UserId;

            var YourFiInfo = await _ClientHubService.AssetSale(UserId, Context.ConnectionId, Asset);

            await _hubContext.Clients.Client(Context.ConnectionId)
   .SendAsync("ReadFiInfo", YourFiInfo);

            // 取得排行榜最高玩家
            var TopUserInBoard = await _ClientHubService.TopUserInBoard(_UserInfos);
            await _hubContext.Clients.All.SendAsync("TopUserInBoard", TopUserInBoard);

            // 取得交易所清單
            var AssetTransactionList = await _ClientHubService.GetAssetTransactionList();
            await _hubContext.Clients.All.SendAsync("AssetTransactionList", AssetTransactionList);
        }

        /// <summary>
        /// 清償負債
        /// </summary>
        /// <param name="Liabilitie"></param>
        /// <returns></returns>
        public async Task LiabilitieSales(AssetAndCategoryModel Liabilitie)
        {
            // 賣負資產
            var UserId = _UserInfos
               .FirstOrDefault(x => x.ConnectionId == Context.ConnectionId).UserId;

            var YourFiInfo = await _ClientHubService.LiabilitieSale(UserId, Context.ConnectionId, Liabilitie);

            await _hubContext.Clients.Client(Context.ConnectionId)
   .SendAsync("ReadFiInfo", YourFiInfo);

            // 取得排行榜最高玩家

            var TopUserInBoard = await _ClientHubService.TopUserInBoard(_UserInfos);

            await _hubContext.Clients.All.SendAsync("TopUserInBoard", TopUserInBoard);

        }

        /// <summary>
        /// 取得現在時間字串
        /// </summary>
        /// <returns></returns>
        public string GetNowSrring()
        {
            return DateTime.Now.ToString("[yyyy/MM/dd HH:mm:ss]");
        }

        /// <summary>
        /// 購買資產
        /// </summary>
        /// <param name="Asset"></param>
        /// <returns></returns>
        public async Task AssetBuy(AssetForTrading Asset)
        {
            var BuyerUserInfo = _UserInfos.FirstOrDefault(x => x.ConnectionId == Context.ConnectionId);
            var ResAssetBuy = await _ClientHubService.AssetBuy(Asset, BuyerUserInfo);

            var Buyer = new ApiResponse();
            Buyer.Data = ResAssetBuy.BuyerFiInfo;
            var Seller = new ApiResponse();
            Seller.Data = ResAssetBuy.SellerFiInfo;

            // 更新 buyer
            await _hubContext.Clients.Client(Context.ConnectionId)
                         .SendAsync("ReadFiInfo", Buyer);
            // 更新 seller
            await _hubContext.Clients.Client(ResAssetBuy.SellerFiInfo.ConnectId)
                         .SendAsync("ReadFiInfo", Seller);

            // 回傳訊息 buyer
            await _hubContext.Clients.Client(Context.ConnectionId)
                    .SendAsync("UpdContent", $"$$$ 您已購入 {Asset.BuyAsset.Name} 資產");
            // 回傳訊息 seller
            await _hubContext.Clients.Client(ResAssetBuy.SellerFiInfo.ConnectId)
                    .SendAsync("UpdContent", $"-$$$ 您已賣出 {Asset.BuyAsset.Name} 資產");

            // 取得交易所清單
            var AssetTransactionList = await _ClientHubService.GetAssetTransactionList();
            await _hubContext.Clients.All.SendAsync("AssetTransactionList", AssetTransactionList);
        }

        #endregion
    }
}
