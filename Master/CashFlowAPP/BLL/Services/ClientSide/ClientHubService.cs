using BLL.IServices;
using Common.Enum;
using Common.Methods;
using Common.Model;
using DPL.EF;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace BLL.Services.ClientSide
{
    public class ClientHubService : ServiceBase, IClientHubService
    {
        private readonly IMemoryCache _MemoryCache;
        public ClientHubService(
            IMemoryCache memoryCache
        )
        {
            _MemoryCache = memoryCache;
        }

        public async Task<ApiResponse> TopUserInBoard(List<UserInfo> UsersInfos)
        {
            var Res = new ApiResponse();

            // 取得所有用戶快取
            var FiInfos = new List<FiInfo>();
            foreach (var UserInfos in UsersInfos)
            {
                FiInfo UserFiInfo = new FiInfo();


                if (UserInfos.UserId != 0)
                {
                    UserFiInfo = _MemoryCache.Get<FiInfo?>(UserInfos.UserId);
                }
                if (UserFiInfo != null)
                {
                    FiInfos.Add(UserFiInfo);
                }

            }

            var TotoalNetProfitUser = FiInfos
                .Where(x => x.TotoalNetProfit == FiInfos.Max(c => c.TotoalNetProfit))
                .FirstOrDefault();

            TopTotoalNetProfit TopTotoalNetProfitUser = new TopTotoalNetProfit();
            TopTotoalNetProfitUser.UserId = TotoalNetProfitUser.UserId;
            TopTotoalNetProfitUser.UserName = TotoalNetProfitUser.UserName;
            TopTotoalNetProfitUser.UserValue = (decimal)TotoalNetProfitUser.TotoalNetProfit;


            var DebtUser = FiInfos
                .FirstOrDefault(x => x.Debt == FiInfos.Min(x => x.Debt));

            TopDebt TopDebtUser = new TopDebt();
            TopDebtUser.UserId = DebtUser.UserId;
            TopDebtUser.UserName = DebtUser.UserName;
            TopDebtUser.UserValue = (decimal)DebtUser.Debt;


            var RevenueUser = FiInfos
           .FirstOrDefault(x => x.Revenue == FiInfos.Max(x => x.Revenue));

            TopRevenue TopRevenueUser = new TopRevenue();
            TopRevenueUser.UserId = RevenueUser.UserId;
            TopRevenueUser.UserName = RevenueUser.UserName;
            TopRevenueUser.UserValue = (decimal)RevenueUser.Revenue;




            var NetProfitUser = FiInfos
           .FirstOrDefault(x => x.NetProfit == FiInfos.Max(x => x.NetProfit));

            TopNetProfit TopNetProfitUserUser = new TopNetProfit();
            TopNetProfitUserUser.UserId = NetProfitUser.UserId;
            TopNetProfitUserUser.UserName = NetProfitUser.UserName;
            TopNetProfitUserUser.UserValue = (decimal)NetProfitUser.NetProfit;


            TopUser TopUsers = new TopUser();

            TopUsers.TotoalNetProfitUser = TopTotoalNetProfitUser;
            TopUsers.DebtUser = TopDebtUser;
            TopUsers.RevenueUser = TopRevenueUser;
            TopUsers.NetProfitUser = TopNetProfitUserUser;

            Res.Data = TopUsers;

            return Res;
        }

        public async Task<CardInfo> ProcessCardInfo(Card YourCard, List<UserInfo> UsersInfos, int YourUserId, string YourConnectId)
        {
            var Result = new CardInfo();
            FiInfo YourFiInfo = new FiInfo();

            try
            {
                // 因為 GetDbContext 是 protected，所以要使用基底類別呼叫
                using (var Db = base.GetDbContext())
                {
                    var YourCardEffects = Db
                        .CardEffects
                        .Where(x => x.CardId == YourCard.Id)
                        .ToList();

                    // 取得所有人的快取
                    var FiInfos = new List<FiInfo>();
                    foreach (var UserInfos in UsersInfos)
                    {
                        FiInfo UserFiInfo = new FiInfo();


                        if (UserInfos.UserId == 0)
                        {
                            UserFiInfo = _MemoryCache.Get<FiInfo?>(UserInfos.ConnectionId);
                        }
                        else
                        {
                            UserFiInfo = _MemoryCache.Get<FiInfo?>(UserInfos.UserId);
                        }
                        if (UserFiInfo != null)
                        {
                            UserFiInfo.ConnectId = UserInfos.ConnectionId;
                            UserFiInfo.UserId = UserInfos.UserId;
                            FiInfos.Add(UserFiInfo);
                        }
                    }

                    if (FiInfos.Count > 0)
                    {
                        // 抽卡人的快取
                        if (YourUserId == 0)
                        {
                            YourFiInfo = FiInfos.FirstOrDefault(x => x.ConnectId == YourConnectId);
                        }
                        else
                        {
                            YourFiInfo = FiInfos.FirstOrDefault(x => x.UserId == YourUserId);
                        }
                        var YourJob = YourFiInfo.CashFlowIncome.FirstOrDefault(x => x.CashFlowCategoryName == "工作薪水");
                        var DailyExpenese = YourFiInfo.CashFlowExpense.FirstOrDefault(x => x.CashFlowCategoryName == "生活花費");

                        // Todo 影響交易紀錄快取
                        YourFiInfo.NowCardAsset = null;
                        YourFiInfo.ValueInterest = null;


                        foreach (var YourCardEffect in YourCardEffects)
                        {
                            var _CashFlow = new CashFlow();
                            var _Asset = new Asset();
                            var _CashFlowCategory = new CashFlowCategory();
                            var _AssetCategory = new AssetCategory();

                            if (YourCardEffect.EffectTableId == (int)Common.Enum.EffectTables.CashFlow)
                            {
                                _CashFlow = Db.CashFlows
                                     .FirstOrDefault(x => x.Id == YourCardEffect.TableId);
                                if (YourCard.Type == "交易機會") // 給抽卡人選擇
                                {
                                }
                                if (YourCard.Type == "強迫中獎") // 強迫中獎 直接影響目前資料
                                {
                                }
                                if (YourCard.Type == "天選之人") // 直接影響當前抽卡人資料
                                {
                                    if (YourCard.Id == 86 || YourCard.Id == 89)
                                    {
                                        // 工作薪水影響
                                        YourJob.Value
                                         = Math.Round(YourJob.Value * (decimal)YourCardEffect.Value, 0);
                                        Result.Value = $"調為: {Math.Round(YourJob.Value, 0)}";

                                    }
                                }
                            }
                            if (YourCardEffect.EffectTableId == (int)Common.Enum.EffectTables.Asset)
                            {
                                _Asset = Db.Assets
                                    .FirstOrDefault(x => x.Id == YourCardEffect.TableId);
                                if (YourCard.Type == "交易機會") // 給抽卡人選擇
                                {
                                    if (_Asset.AssetCategoryId == 48) // 基金
                                    {
                                        var Category =
                                        Db.AssetCategories
                                            .FirstOrDefault(X => X.Id == _Asset.AssetCategoryId);



                                        var NewAsset = new AssetAndCategoryModel();
                                        var GuidCode = System.Guid.NewGuid().ToString("N");
                                        var Value = new MathMethodService()
                                         .FoundationCount(YourJob.Value, DailyExpenese.Value);

                                        NewAsset.Id = _Asset.Id;
                                        NewAsset.Name = _Asset.Name;
                                        NewAsset.ParentId = Category.ParentId;
                                        NewAsset.Value = Math.Round(Value);
                                        NewAsset.GuidCode = GuidCode;
                                        NewAsset.Weight = (decimal)_Asset.Weight;
                                        NewAsset.AssetCategoryId = _Asset.AssetCategoryId;
                                        NewAsset.AssetCategoryName = Category.Name;
                                        NewAsset.Description = _Asset.Description;


                                        // YourFiInfo.Asset.Add(NewAsset);

                                        Result.NowCardAsset = NewAsset;

                                        Result.Value = $"需花費:{Math.Round(-1 * Value, 0)}";
                                    }
                                    if (_Asset.AssetCategoryId == 47)
                                    {
                                        var Category =
                                     Db.AssetCategories
                                         .FirstOrDefault(X => X.Id == _Asset.AssetCategoryId);



                                        var NewAsset = new AssetAndCategoryModel();
                                        var GuidCode = System.Guid.NewGuid().ToString("N");
                                        var Value = new MathMethodService()
                                         .FoundationCount(YourJob.Value, DailyExpenese.Value);

                                        NewAsset.Id = _Asset.Id;
                                        NewAsset.Name = _Asset.Name;
                                        NewAsset.ParentId = Category.ParentId;
                                        NewAsset.Value = Math.Round(Value, 0);
                                        NewAsset.GuidCode = GuidCode;
                                        NewAsset.Weight = (decimal)_Asset.Weight;
                                        NewAsset.AssetCategoryId = _Asset.AssetCategoryId;
                                        NewAsset.AssetCategoryName = Category.Name;
                                        NewAsset.Description = _Asset.Description;

                                        // YourFiInfo.Asset.Add(NewAsset)


                                        // 定存利息

                                        var NewCashFlow = new CashFlowAndCategoryModel();
                                        var CashFlowValue = new MathMethodService()
                                            .FoundationInterest(Value);

                                        var CashFlow = Db.CashFlows
                                            .FirstOrDefault(x => x.Name == "定存利息");
                                        var CashFlowCategory = Db.CashFlowCategories
                                            .FirstOrDefault(x => x.Id == CashFlow.CashFlowCategoryId);


                                        NewCashFlow.Id = CashFlow.Id;
                                        NewCashFlow.Name = CashFlow.Name;
                                        NewCashFlow.ParentId = CashFlowCategory.ParentId;
                                        NewCashFlow.Value = Math.Round(CashFlowValue);
                                        NewCashFlow.GuidCode = GuidCode;
                                        NewCashFlow.Weight = (decimal)CashFlow.Weight;
                                        NewCashFlow.CashFlowCategoryId = CashFlow.CashFlowCategoryId;
                                        NewCashFlow.CashFlowCategoryName = CashFlowCategory.Name;
                                        NewCashFlow.Description = CashFlow.Description;

                                        Result.Value = $"需花費:{Math.Round(-1 * Value, 0)}";
                                        Result.NowCardAsset = NewAsset;
                                        Result.ValueInterest = NewCashFlow;

                                    }
                                }
                                if (YourCard.Type == "強迫中獎") // 強迫中獎 直接影響目前資料
                                {

                                }
                                if (YourCard.Type == "天選之人") // 直接影響當前抽卡人資料
                                {

                                    // 車貸
                                    if (_Asset.Id == 10)
                                    {
                                        var NewAsset = new AssetAndCategoryModel();
                                        var Value = new MathMethodService()
                                            .CarLoanCount(YourJob.Value);

                                        var Category =
                                      Db.AssetCategories
                                          .FirstOrDefault(X => X.Id == _Asset.AssetCategoryId);

                                        var GuidCode = System.Guid.NewGuid().ToString("N");


                                        NewAsset.Id = _Asset.Id;
                                        NewAsset.Name = _Asset.Name;
                                        NewAsset.ParentId = Category.ParentId;
                                        NewAsset.Value = Math.Round(Value);
                                        NewAsset.GuidCode = GuidCode;
                                        NewAsset.Weight = (decimal)_Asset.Weight;
                                        NewAsset.AssetCategoryId = _Asset.AssetCategoryId;
                                        NewAsset.AssetCategoryName = Category.Name;
                                        NewAsset.Description = _Asset.Description;


                                        YourFiInfo.Liabilities.Add(NewAsset);


                                        // 車貸利息
                                        var NewCashFlow = new CashFlowAndCategoryModel();
                                        var CashFlowValue = new MathMethodService()
                                            .CarInterest(Value);

                                        var CashFlow = Db.CashFlows
                                            .FirstOrDefault(x => x.Name == "車貸利息");
                                        var CashFlowCategory = Db.CashFlowCategories
                                            .FirstOrDefault(x => x.Id == CashFlow.CashFlowCategoryId);


                                        NewCashFlow.Id = CashFlow.Id;
                                        NewCashFlow.Name = CashFlow.Name;
                                        NewCashFlow.ParentId = CashFlowCategory.ParentId;
                                        NewCashFlow.Value = Math.Round(CashFlowValue);
                                        NewCashFlow.GuidCode = GuidCode;
                                        NewCashFlow.Weight = (decimal)CashFlow.Weight;
                                        NewCashFlow.CashFlowCategoryId = CashFlow.CashFlowCategoryId;
                                        NewCashFlow.CashFlowCategoryName = CashFlowCategory.Name;
                                        NewCashFlow.Description = CashFlow.Description;


                                        YourFiInfo.CashFlowExpense.Add(NewCashFlow);
                                        Result.Value = $"車貸本金:{Math.Round(Value, 0)}，及利息${Math.Round(NewCashFlow.Value, 0)}";

                                    }
                                }
                            }
                            if (YourCardEffect.EffectTableId == (int)Common.Enum.EffectTables.CashFlowCategories)
                            {
                                _CashFlowCategory = Db.CashFlowCategories
                                    .FirstOrDefault(x => x.Id == YourCardEffect.TableId);
                                if (YourCard.Type == "交易機會") // 給抽卡人選擇
                                {
                                }
                                if (YourCard.Type == "強迫中獎") // 強迫中獎 直接影響目前資料
                                {
                                }
                                if (YourCard.Type == "天選之人") // 直接影響當前抽卡人資料
                                {
                                    if (YourCard.Id == 90 || YourCard.Id == 93)
                                    {
                                        // 現金扣錢
                                        YourFiInfo.CurrentMoney = Math.Round((decimal)YourFiInfo.CurrentMoney +
                                          ((decimal)YourJob.Value * (decimal)YourCardEffect.Value), 0);
                                        Result.Value = $"{Math.Round(YourJob.Value * (decimal)YourCardEffect.Value, 0)}";
                                    }
                                }
                            }
                            if (YourCardEffect.EffectTableId == (int)Common.Enum.EffectTables.AssetCategories)
                            {
                                _AssetCategory = Db.AssetCategories
                                    .FirstOrDefault(x => x.Id == YourCardEffect.TableId);
                                if (YourCard.Type == "交易機會") // 給抽卡人選擇
                                {
                                }
                                if (YourCard.Type == "強迫中獎") // 強迫中獎 直接影響目前資料
                                {

                                    // 影響全部

                                    foreach (var UserInfos in FiInfos)
                                    {

                                        var AllAssets = UserInfos.Asset.Where(x => x.AssetCategoryId == YourCardEffect.TableId).ToList();
                                        foreach (var Asset in AllAssets)
                                        {
                                            Asset.Value = Math.Round(Asset.Value * (decimal)YourCardEffect.Value);
                                        }

                                        if (UserInfos.UserId == 0)
                                        {
                                            _MemoryCache.Set(UserInfos.ConnectId, UserInfos,
                                               new MemoryCacheEntryOptions().SetPriority(CacheItemPriority.NeverRemove));
                                        }
                                        else
                                        {
                                            _MemoryCache.Set(UserInfos.UserId, UserInfos,
                                             new MemoryCacheEntryOptions().SetPriority(CacheItemPriority.NeverRemove));
                                        }
                                    }
                                }
                                if (YourCard.Type == "天選之人") // 直接影響當前抽卡人資料
                                {
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            YourFiInfo.NowCardId = YourCard.Id;
            YourFiInfo.NowCardAsset = Result.NowCardAsset;
            YourFiInfo.ValueInterest = Result.ValueInterest;

            YourFiInfo.CurrentMoney = YourFiInfo.CurrentMoney + YourFiInfo.TotalEarnings;

            YourFiInfo = FiInfoAccounting(YourFiInfo);

            if (YourUserId == 0)
            {
                _MemoryCache.Set(YourConnectId, YourFiInfo,
                   new MemoryCacheEntryOptions().SetPriority(CacheItemPriority.NeverRemove));
            }
            else
            {
                _MemoryCache.Set(YourUserId, YourFiInfo,
                 new MemoryCacheEntryOptions().SetPriority(CacheItemPriority.NeverRemove));
            }
            return Result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<ApiResponse> ChoiceOfCard(int UserId, string ConnectId)
        {
            FiInfo YourFiInfo = new FiInfo();


            var Res = new ApiResponse();
            if (UserId == 0)
            {
                YourFiInfo = _MemoryCache.Get<FiInfo?>(ConnectId);
            }
            else
            {
                YourFiInfo = _MemoryCache.Get<FiInfo?>(UserId);
            }
            if (YourFiInfo.NowCardAsset != null)
            {
                YourFiInfo.Asset.Add(YourFiInfo.NowCardAsset);
            }
            if (YourFiInfo.ValueInterest != null)
            {
                YourFiInfo.CashFlowIncome.Add(YourFiInfo.ValueInterest);
            }
            YourFiInfo.CurrentMoney = YourFiInfo.CurrentMoney - YourFiInfo.NowCardAsset.Value;

            YourFiInfo = FiInfoAccounting(YourFiInfo);


            if (UserId == 0)
            {
                _MemoryCache.Set(ConnectId, YourFiInfo,
                   new MemoryCacheEntryOptions().SetPriority(CacheItemPriority.NeverRemove));
            }
            else
            {
                _MemoryCache.Set(UserId, YourFiInfo,
                 new MemoryCacheEntryOptions().SetPriority(CacheItemPriority.NeverRemove));
            }

            Res.Data = YourFiInfo;
            return Res;
        }

        /// <summary>
        /// 賣資產
        /// </summary>
        /// <returns></returns>
        public async Task<ApiResponse> AssetSale(int UserId, string ConnectId, SaleAsset Asset)
        {
            // 負資產

            var AssetTransactionList = _MemoryCache.Get<List<AssetForTrading>>("AssetTransactionList");
            if (AssetTransactionList == null)
            {
                AssetTransactionList = new List<AssetForTrading>();
            }

            var Res = new ApiResponse();
            FiInfo YourFiInfo = new FiInfo();
            if (UserId == 0)
            {
                YourFiInfo = _MemoryCache.Get<FiInfo?>(ConnectId);
            }
            else
            {
                YourFiInfo = _MemoryCache.Get<FiInfo?>(UserId);
            }

            // 找資產出來賣，如果是房地產或股票則上架到商品清單

            var YourAsset = YourFiInfo.Asset.FirstOrDefault(x => x.GuidCode == Asset.GuidCode);

            if (
                YourAsset.AssetCategoryId == 25 || // 股票
                YourAsset.ParentId == 17 || // 房地產
                YourAsset.AssetCategoryId == 28 || // 產業
                YourAsset.ParentId == 28 // 產業
            )
            {   // 已賣過，就要先取消掛單
                var IsBuied = AssetTransactionList.Any(x => x.BuyAsset.GuidCode == Asset.GuidCode);
                if (!IsBuied)
                {
                    var AssetTransaction = new AssetForTrading();

                    var NewAssetTransaction = new AssetAndCategoryModel();
                    NewAssetTransaction.Id = YourAsset.Id;
                    NewAssetTransaction.Name = YourAsset.Name;
                    NewAssetTransaction.Value = Asset.Value;
                    NewAssetTransaction.Weight = YourAsset.Weight;
                    NewAssetTransaction.Description = YourAsset.Description;
                    NewAssetTransaction.AssetCategoryName = YourAsset.AssetCategoryName;
                    NewAssetTransaction.AssetCategoryId = YourAsset.AssetCategoryId;
                    NewAssetTransaction.ParentId = YourAsset.ParentId;
                    NewAssetTransaction.GuidCode = YourAsset.GuidCode;
                    AssetTransaction.BuyAsset = NewAssetTransaction;

                    AssetTransaction.ConnectId = ConnectId;
                    AssetTransaction.UserId = UserId;

                    var ValueInterest = YourFiInfo.CashFlowIncome
                        .FirstOrDefault(x => x.GuidCode == YourAsset.GuidCode);
                    AssetTransaction.ValueInterest = ValueInterest;

                    AssetTransactionList.Add(AssetTransaction);
                    _MemoryCache.Set("AssetTransactionList", AssetTransactionList);
                    Res.Success = true;
                    Res.Message = "掛單成功，等待賣出";
                }
                else
                {
                    Res.Success = false;
                    Res.Message = "請先取消掛單，再執行";
                }
            }
            else
            {
                YourFiInfo.CurrentMoney = Math.Round((decimal)YourFiInfo.CurrentMoney + (decimal)YourAsset.Value, 0);

                if (YourAsset.AssetCategoryId == 47)
                {
                    var SavingInterest = YourFiInfo.CashFlowIncome
                        .FirstOrDefault(x => x.GuidCode == Asset.GuidCode);
                    YourFiInfo.CashFlowIncome.Remove(SavingInterest);
                }

                YourFiInfo.Asset.Remove(YourAsset);

                YourFiInfo = FiInfoAccounting(YourFiInfo);

                if (UserId == 0)
                {
                    _MemoryCache.Set(ConnectId, YourFiInfo,
                       new MemoryCacheEntryOptions().SetPriority(CacheItemPriority.NeverRemove));
                }
                else
                {
                    _MemoryCache.Set(UserId, YourFiInfo,
                       new MemoryCacheEntryOptions().SetPriority(CacheItemPriority.NeverRemove));
                }
            }

            Res.Data = YourFiInfo;

            return Res;
        }
        public async Task<ApiResponse> LiabilitieSale(int UserId, string ConnectId, AssetAndCategoryModel Liabilitie)
        {
            // 負資產

            var Res = new ApiResponse();
            FiInfo YourFiInfo = new FiInfo();
            if (UserId == 0)
            {
                YourFiInfo = _MemoryCache.Get<FiInfo?>(ConnectId);
            }
            else
            {
                YourFiInfo = _MemoryCache.Get<FiInfo?>(UserId);
            }

            // 找資產出來賣

            var YourAsset = YourFiInfo.Liabilities.FirstOrDefault(x => x.GuidCode == Liabilitie.GuidCode);

            YourFiInfo.CurrentMoney = Math.Round((decimal)YourFiInfo.CurrentMoney + (decimal)YourAsset.Value, 0);

            if (YourAsset.AssetCategoryId == 24)
            {
                var Interest = YourFiInfo.CashFlowExpense
                    .FirstOrDefault(x => x.GuidCode == Liabilitie.GuidCode);
                YourFiInfo.CashFlowExpense.Remove(Interest);

            }

            YourFiInfo.Liabilities.Remove(YourAsset);

            YourFiInfo = FiInfoAccounting(YourFiInfo);

            if (UserId == 0)
            {
                _MemoryCache.Set(ConnectId, YourFiInfo,
                   new MemoryCacheEntryOptions().SetPriority(CacheItemPriority.NeverRemove));
            }
            else
            {
                _MemoryCache.Set(UserId, YourFiInfo,
                 new MemoryCacheEntryOptions().SetPriority(CacheItemPriority.NeverRemove));
            }

            Res.Data = YourFiInfo;
            return Res;
        }

        /// <summary>
        /// 計算 FiInfo 月收支
        /// </summary>
        /// <param name="YourFiInfo"></param>
        /// <returns></returns>
        public FiInfo FiInfoAccounting(FiInfo YourFiInfo)
        {
            try
            {
                decimal TotalIncomce = 0;
                decimal TotalExpense = 0;
                decimal TotalEarnings = 0;

                foreach (var Income in YourFiInfo.CashFlowIncome)
                {
                    TotalIncomce += Math.Round(Income.Value, 0);
                }
                foreach (var Expense in YourFiInfo.CashFlowExpense)
                {
                    TotalExpense += Math.Round(Expense.Value, 0);
                }

                TotalEarnings = TotalIncomce + TotalExpense;

                YourFiInfo.TotalIncomce = TotalIncomce;
                YourFiInfo.TotalExpense = TotalExpense;
                YourFiInfo.TotalEarnings = TotalEarnings;


                if (YourFiInfo.UserId != 0)
                {
                    // 最高現金
                    YourFiInfo.TotoalNetProfit = YourFiInfo.TotoalNetProfit <= YourFiInfo.CurrentMoney ? YourFiInfo.CurrentMoney : YourFiInfo.TotoalNetProfit;

                    // 最高支出  -1>-2 
                    YourFiInfo.Debt = YourFiInfo.Debt > YourFiInfo.TotalExpense ? YourFiInfo.TotalExpense : YourFiInfo.Debt;

                    // 最高收入
                    YourFiInfo.Revenue = YourFiInfo.Revenue <= YourFiInfo.TotalIncomce ?
                        YourFiInfo.TotalIncomce : YourFiInfo.Revenue;
                    // 最高淨收入
                    YourFiInfo.NetProfit = YourFiInfo.NetProfit <= YourFiInfo.TotalEarnings ?
                        YourFiInfo.TotalEarnings : YourFiInfo.NetProfit;
                }



                return YourFiInfo;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async void SavingBoard(int UserId)
        {
            var YourFiInfo = _MemoryCache.Get<FiInfo>(UserId);
            using (var _CashFlowDbContext = base.GetDbContext())
            {
                var YourBoard = _CashFlowDbContext.UserBoards.FirstOrDefault(x => x.UserId == UserId);

                if (YourBoard == null)
                {
                    YourBoard = new UserBoard();
                    YourBoard.UserId = UserId;
                    YourBoard.NetProfit = YourFiInfo.NetProfit;
                    YourBoard.Revenue = YourFiInfo.Revenue;
                    YourBoard.Debt = YourFiInfo.Debt;
                    YourBoard.TotoalNetProfit = YourFiInfo.TotoalNetProfit;
                    _CashFlowDbContext.UserBoards.Add(YourBoard);
                }
                else
                {
                    YourBoard.NetProfit = YourFiInfo.NetProfit;
                    YourBoard.Revenue = YourFiInfo.Revenue;
                    YourBoard.Debt = YourFiInfo.Debt;
                    YourBoard.TotoalNetProfit = YourFiInfo.TotoalNetProfit;
                }
                _CashFlowDbContext.SaveChanges();
            }
        }
        public async Task<ApiResponse> ReadFiInfo(int UserId, string ConnectId)
        {
            var Res = new ApiResponse();

            // 取快取
            if (UserId == 0)
            {
                Res.Data = _MemoryCache.Get<FiInfo>(ConnectId);
            }
            else
            {
                Res.Data = _MemoryCache.Get<FiInfo>(UserId);
            }
            using (var _CashFlowDbContext = base.GetDbContext())
            {
                // 沒資料就初始化
                if (Res.Data == null)
                {




                    var Assets = _CashFlowDbContext.Assets
                    .Where(x => x.Status == (int)StatusCode.Enable)
                    .AsNoTracking()
                    .ToList();
                    var AssetCategories = _CashFlowDbContext.AssetCategories
                        .Where(x => x.Status == (int)StatusCode.Enable)
                        .AsNoTracking()
                        .ToList();
                    var AssetAndCategory =
                         Assets.Join(
                          AssetCategories,
                           a => a.AssetCategoryId,
                           ac => ac.Id,
                           (a, ac) =>
                           new AssetAndCategoryModel { Id = a.Id, Name = a.Name, Value = a.Value, Weight = (decimal)a.Weight, Description = a.Description, AssetCategoryName = ac.Name, AssetCategoryId = a.AssetCategoryId, ParentId = ac.ParentId })
                         .ToList();

                    var CashFlows = _CashFlowDbContext.CashFlows
                        .Where(x => x.Status == (int)StatusCode.Enable)
                        .AsNoTracking()
                        .ToList();
                    var CashFlowCategories = _CashFlowDbContext.CashFlowCategories
                      .Where(x => x.Status == (int)StatusCode.Enable)
                      .AsNoTracking()
                      .ToList();
                    var CashFlowAndCategory =
                         CashFlows.Join(
                          CashFlowCategories,
                           c => c.CashFlowCategoryId,
                           cc => cc.Id,
                           (c, cc) =>
                           new CashFlowAndCategoryModel { Id = c.Id, Name = c.Name, Value = c.Value, Weight = (decimal)c.Weight, Description = c.Description, CashFlowCategoryName = cc.Name, CashFlowCategoryId = c.CashFlowCategoryId, ParentId = cc.ParentId })
                         .ToList();



                    var CashFlowResult = new List<CashFlowAndCategoryModel>();
                    var AssetResult = new List<AssetAndCategoryModel>();
                    var _Random = StaticRandom();
                    // if (Req.Args == null) TODO: UserId問卷會影響初始值
                    {
                        // 先抽職業
                        var Jobs =
                              CashFlowAndCategory
                             .Where(c => c.CashFlowCategoryName == "工作薪水")
                             .Select(x => new RandomItem<int>
                             {
                                 SampleObj = x.Id,
                                 Weight = (decimal)x.Weight
                             })
                             .ToList();
                        var Job = Method.RandomWithWeight(Jobs);
                        var YourJob = CashFlowAndCategory.FirstOrDefault(c => c.Id == Job);
                        CashFlowResult.Add(YourJob);

                        // 如果是老闆
                        var CompanysCategoryId = AssetCategories.Where(x => x.ParentId == 28).Select(x => x.Id).ToList();
                        if (YourJob.Name == "老闆")
                        {
                            var GuidCode = System.Guid.NewGuid().ToString("N");

                            var Companies = AssetAndCategory
                                .Where(c =>
                                       c.AssetCategoryName == "公司行號" || CompanysCategoryId.Contains(c.AssetCategoryId)
                                )
                                .Select(x => new RandomItem<int>
                                {
                                    SampleObj = x.Id,
                                    Weight = (decimal)x.Weight
                                })
                                .ToList();

                            var CompanyId = Method.RandomWithWeight(Companies);
                            var Company = GetAssetNewModel(AssetAndCategory.FirstOrDefault(x => x.Id == CompanyId));
                            Company.GuidCode = GuidCode;
                            AssetResult.Add(Company);

                            var CompanyCashFlow = GetCashFlowNewModel(CashFlowAndCategory.FirstOrDefault(c => c.Name == "公司紅利收入"));
                            var Dice = _Random.Next(1, 10);
                            CompanyCashFlow.Value = Math.Round(((Company.Value / 3) / Dice) / 12, 0); // 資產的 3 年除與 1-10 在除 12 月
                            CompanyCashFlow.GuidCode = GuidCode;
                            CashFlowResult.Add(CompanyCashFlow);
                        }

                        // 生活花費
                        // var _Random = new Random(Guid.NewGuid().GetHashCode()); // 讓隨機機率離散

                        var DailyExpenese = CashFlowAndCategory
                            .FirstOrDefault(c => c.CashFlowCategoryName == "生活花費");
                        DailyExpenese.Value = DailyExpenese.Value * _Random.Next(1, 5);
                        CashFlowResult.Add(DailyExpenese);

                        // 抽資產隨便取
                        var Max = 2; // 基數少時只抽兩筆
                        if (AssetAndCategory.Count() > 10)
                        {
                            Max = 5;
                        }

                        var DrawCounts = _Random.Next(1, Max);
                        var AssetDices =
                             AssetAndCategory
                             .Where(c =>
                                    c.AssetCategoryName != "公司行號" && // 初始化抽到老闆才能有公司
                                    !CompanysCategoryId.Contains(c.AssetCategoryId) &&
                                    c.Name != "房貸" // 有房子才有房貸，先排除
                             )
                             .Select(x => new RandomItem<int>
                             {
                                 SampleObj = x.Id,
                                 Weight = (decimal)x.Weight
                             })
                             .ToList();

                        // 先抽再根據抽到類別做特殊邏輯處理
                        // 可重複抽取
                        for (var i = 0; i < DrawCounts; i++)
                        {
                            var AssetFromDice = Method.RandomWithWeight(AssetDices);
                            var YourAssets = GetAssetNewModel(AssetAndCategory.FirstOrDefault(a => a.Id == AssetFromDice));

                            // 有房子才有房貸
                            if (YourAssets.ParentId == 17) // 房地產
                            {
                                // https://www.796t.com/content/1549376313.html
                                // 資產與利息需要對應才會有 Guid
                                var GuidCode = System.Guid.NewGuid().ToString("N");

                                float ratio = _Random.Next(1, 8);
                                float MortgageRatio = ratio / 10; // 新成屋最高八成
                                var MortgageRatioAsset = GetAssetNewModel(AssetAndCategory.FirstOrDefault(x => x.Name == "房貸"));
                                MortgageRatioAsset.Value = ((decimal)YourAssets.Value) * ((decimal)MortgageRatio) * -1;
                                MortgageRatioAsset.GuidCode = GuidCode;
                                AssetResult.Add(MortgageRatioAsset);

                                // 房貸利息
                                float Ratio = 0.15F;
                                var MortgageMonthRate = GetCashFlowNewModel(CashFlowAndCategory.FirstOrDefault(x => x.Name == "房貸利息"));
                                MortgageMonthRate.Value = Math.Round((MortgageRatioAsset.Value + (MortgageRatioAsset.Value * (decimal)Ratio)) / 360);
                                MortgageMonthRate.GuidCode = GuidCode;
                                CashFlowResult.Add(MortgageMonthRate);
                            }

                            // 車貸是隨機value
                            if (YourAssets.Id == 10) // 車貸車價8成
                            {
                                var GuidCode = System.Guid.NewGuid().ToString("N");
                                var CarValue = Math.Round((YourJob.Value * 8), 0); // 車子價格大約是薪水*10
                                var Ratio = _Random.Next((int)(YourJob.Value * 2), (int)CarValue);
                                YourAssets.Value = (decimal)Ratio * -1;
                                YourAssets.GuidCode = GuidCode;

                                // 車貸利息 => (貸款總金額 + 貸款總利息) / 貸款總期數 = 每月月付金
                                var CarInterest = GetCashFlowNewModel(CashFlowAndCategory.FirstOrDefault(c => c.Name == "車貸利息"));
                                CarInterest.Value = Math.Round((((decimal)Ratio * 28 / 1000 / 12) + ((decimal)Ratio / 60)), 0) * -1; // 2.8% 除與五年
                                CarInterest.GuidCode = GuidCode;
                                CashFlowResult.Add(CarInterest);
                            }

                            // 定存
                            if (YourAssets.AssetCategoryId == 47)
                            {
                                var GuidCode = System.Guid.NewGuid().ToString("N");
                                // 金融商品筆數
                                var InvestCount = AssetAndCategory.Where(x => x.ParentId == 46).ToList().Count;
                                // 定存價值=薪水*隨機數字*
                                var Disc = _Random.Next(1, InvestCount);
                                YourAssets.Value = (YourJob.Value - DailyExpenese.Value) * Disc;
                                YourAssets.GuidCode = GuidCode;

                                var SavingInterest = GetCashFlowNewModel(CashFlowAndCategory.FirstOrDefault(c => c.Name == "定存利息"));
                                SavingInterest.Value = Math.Round(YourAssets.Value / 1200, 0);
                                SavingInterest.GuidCode = GuidCode;
                                CashFlowResult.Add(SavingInterest);
                            }

                            // 基金
                            if (YourAssets.AssetCategoryId == 48)
                            {
                                var GuidCode = System.Guid.NewGuid().ToString("N");
                                //// 金融商品筆數
                                //var InvestCount = AssetAndCategory.Where(x => x.ParentId == 46).ToList().Count;
                                //// 定存價值=薪水*隨機數字*
                                //var Dice = _Random.Next(1, InvestCount);
                                //YourAssets.Value = (YourJob.Value - DailyExpenese.Value) * Dice;
                                YourAssets.Value = new MathMethodService()
                                    .FoundationCount(YourJob.Value, DailyExpenese.Value);
                                YourAssets.GuidCode = GuidCode;
                            }

                            // 學貸
                            if (YourAssets.Id == 17)
                            {
                                var GuidCode = System.Guid.NewGuid().ToString("N");
                                YourAssets.Value = _Random.Next(160000, 400000);
                                YourAssets.GuidCode = GuidCode;
                            }
                            if (YourAssets.GuidCode == null)
                            {
                                var GuidCode = System.Guid.NewGuid().ToString("N");
                                YourAssets.GuidCode = GuidCode;
                            }
                            AssetResult.Add(YourAssets);
                        }

                        // 不重複抽取
                        //var NotRepeat = new List<int>();
                        //while(NotRepeat.Count<DrawCounts)
                        //{
                        //    var AssetFromDice = Method.RandomWithWeight(AssetDices);
                        //    if (!NotRepeat.Contains(AssetFromDice))
                        //    {
                        //        var YourAssets = AssetAndCategory
                        //        .FirstOrDefault(a => a.Id == AssetFromDice);
                        //        AssetResult.Add(YourAssets);
                        //        NotRepeat.Add(AssetFromDice);
                        //    }
                        //}
                    }

                    Decimal CurrentMoney = _Random.Next(1, 20000);
                    var CashFlowIncome = new List<CashFlowAndCategoryModel>();
                    var CashFlowExpense = new List<CashFlowAndCategoryModel>();
                    var Asset = new List<AssetAndCategoryModel>();
                    var Liabilities = new List<AssetAndCategoryModel>();

                    CashFlowIncome = CashFlowResult.Where(c => c.Value >= 0).ToList();
                    CashFlowExpense = CashFlowResult.Where(c => c.Value < 0).ToList();
                    Asset = AssetResult.Where(c => c.Value >= 0).ToList();
                    Liabilities = AssetResult.Where(c => c.Value < 0).ToList();

                    var UserName = _CashFlowDbContext.Users
                        .Where(x => x.Id == UserId)
                        .Select(x => x.Name)
                        .FirstOrDefault();

                    Res.Data = new FiInfo
                    {
                        UserId = UserId,
                        UserName = UserName,
                        CurrentMoney = 0,
                        CashFlowIncome = CashFlowIncome,
                        CashFlowExpense = CashFlowExpense,
                        Asset = Asset,
                        Liabilities = Liabilities,
                        NowCardId = null,
                        NowCardAsset = null,
                    };

                    Res.Success = true;
                    Res.Code = (int)ResponseStatusCode.Success;
                }

                // 從資料庫取出個人排行版放快取

                FiInfo Data = (FiInfo)Res.Data;
                Data.UserId = UserId;
                Data.ConnectId = ConnectId;

                if (UserId != 0)
                {
                    var YourBoard = _CashFlowDbContext.UserBoards
                       .FirstOrDefault(x => x.UserId == UserId);

                    //is this condition true ? yes : no
                    if (YourBoard != null)
                    {
                        Data.TotoalNetProfit = YourBoard.TotoalNetProfit == null ? 0 : YourBoard.TotoalNetProfit;

                        Data.Debt = YourBoard.Debt == null ? 0 : YourBoard.Debt;

                        Data.Revenue = YourBoard.Revenue == null ? 0 : YourBoard.Revenue;

                        Data.NetProfit = YourBoard.NetProfit == null ? 0 : YourBoard.NetProfit;
                    }

                }

                // 計算收支
                Data = FiInfoAccounting(Data);

                // UserId = 0 時，為遊客， key 改為 ConnectId
                if (UserId == 0)
                {
                    _MemoryCache.Set(ConnectId, Data,
                       new MemoryCacheEntryOptions().SetPriority(CacheItemPriority.NeverRemove));
                }
                else
                {
                    _MemoryCache.Set(UserId, Data,
                     new MemoryCacheEntryOptions().SetPriority(CacheItemPriority.NeverRemove));
                }

                return Res;
            }
        }

        /// <summary>
        /// https://stackoverflow.com/questions/1654887/random-next-returns-always-the-same-values
        /// </summary>
        /// <returns></returns>
        private static Random StaticRandom()
        {
            return new Random(Guid.NewGuid().GetHashCode());
        }

        private CashFlowAndCategoryModel GetCashFlowNewModel(CashFlowAndCategoryModel Data)
        {
            var New = new CashFlowAndCategoryModel();
            New.Id = Data.Id;
            New.Name = Data.Name;
            New.Value = Data.Value;
            New.Weight = Data.Weight;
            New.Description = Data.Description;
            New.CashFlowCategoryName = Data.CashFlowCategoryName;
            New.CashFlowCategoryId = Data.CashFlowCategoryId;
            New.ParentId = Data.ParentId;
            return New;
        }

        private AssetAndCategoryModel GetAssetNewModel(AssetAndCategoryModel Data)
        {
            var New = new AssetAndCategoryModel();
            New.Id = Data.Id;
            New.Name = Data.Name;
            New.Value = Data.Value;
            New.Weight = Data.Weight;
            New.Description = Data.Description;
            New.AssetCategoryName = Data.AssetCategoryName;
            New.AssetCategoryId = Data.AssetCategoryId;
            New.ParentId = Data.ParentId;
            return New;
        }

        public async Task<ApiResponse> GetAssetTransactionList()
        {
            var Res = new ApiResponse();
            var AssetTransactionList = _MemoryCache.Get<List<AssetForTrading>>("AssetTransactionList");
            if (AssetTransactionList == null)
            {
                AssetTransactionList = new List<AssetForTrading>();
            }

            Res.Data = AssetTransactionList;
            return Res;
        }

        public async Task<BuyAsset> AssetBuy(AssetForTrading Asset, UserInfo BuyerUserInfo)
        {
            var Res = new BuyAsset();

            // 從商品清單移除
            var AssetTransactionList = _MemoryCache.Get<List<AssetForTrading>>("AssetTransactionList");
            var BuyAsset = AssetTransactionList.FirstOrDefault(x => x.BuyAsset.GuidCode == Asset.BuyAsset.GuidCode);
            AssetTransactionList.Remove(BuyAsset);
            _MemoryCache.Set("AssetTransactionList", AssetTransactionList);

            // 取出兩者的 FiInfo 做交易
            FiInfo seller = new FiInfo();
            if (Asset.UserId == 0)
            {
                seller = _MemoryCache.Get<FiInfo?>(Asset.ConnectId);
            }
            else
            {
                seller = _MemoryCache.Get<FiInfo?>(Asset.UserId);
            }

            FiInfo buyer = new FiInfo();
            if (BuyerUserInfo.UserId == 0)
            {
                buyer = _MemoryCache.Get<FiInfo?>(BuyerUserInfo.ConnectionId);
            }
            else
            {
                buyer = _MemoryCache.Get<FiInfo?>(BuyerUserInfo.UserId);
            }

            buyer.Asset.Add(Asset.BuyAsset);
            if (Asset.ValueInterest != null)
            {
                buyer.CashFlowIncome.Add(Asset.ValueInterest);
            }

            var RemoveAsset = seller.Asset.FirstOrDefault(x => x.GuidCode == Asset.BuyAsset.GuidCode);
            seller.Asset.Remove(RemoveAsset);
            if (Asset.ValueInterest != null)
            {
                var RemoveValueInterest = seller.CashFlowIncome.FirstOrDefault(x => x.GuidCode == Asset.BuyAsset.GuidCode);
                seller.CashFlowIncome.Remove(RemoveValueInterest);
            }

            // 計算現金 buyer 扣 seller 加

            buyer.CurrentMoney = buyer.CurrentMoney - Asset.BuyAsset.Value;
            seller.CurrentMoney = seller.CurrentMoney + Asset.BuyAsset.Value;

            // 重設快取
            if (Asset.UserId == 0) // seller
            {
                _MemoryCache.Set(Asset.ConnectId, seller);
            }
            else
            {
                _MemoryCache.Set(Asset.UserId, seller);
            }

            if (BuyerUserInfo.UserId == 0)
            {
                _MemoryCache.Set(BuyerUserInfo.ConnectionId, buyer);
            }
            else
            {
                _MemoryCache.Set(BuyerUserInfo.UserId, buyer);
            }

            Res.SellerFiInfo = seller;
            Res.BuyerFiInfo = buyer;

            return Res;
        }
    }

}
