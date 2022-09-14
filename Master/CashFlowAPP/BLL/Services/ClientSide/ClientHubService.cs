using BLL.IServices;
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

        public async Task<CardInfo> ProcessCardInfo(Card YourCard, List<UserInfo> UsersInfos, int YourUserId)
        {
            var Result = new CardInfo();

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
                        //var UserFiInfo = _MemoryCache.Get(UserInfos.Id);
                        //var UserFiInfo2 = _MemoryCache.Get<FiInfo>(UserInfos.Id);
                        var UserFiInfo = _MemoryCache.Get<FiInfo?>(UserInfos.UserId);
                        if (UserFiInfo != null)
                        {
                            UserFiInfo.UserId = UserInfos.UserId;
                            FiInfos.Add(UserFiInfo);
                        }
                    }

                    if (FiInfos.Count > 0)
                    {
                        // 抽卡人的快取
                        var YourFiInfo = FiInfos.FirstOrDefault(x => x.UserId == YourUserId);
                        var YourJob = YourFiInfo.CashFlowIncome.FirstOrDefault(x => x.CashFlowCategoryName == "工作薪水");
                        var DailyExpenese = YourFiInfo.CashFlowExpense.FirstOrDefault(x => x.CashFlowCategoryName == "生活花費");

                        // Todo 影響交易紀錄快取

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
                                        var Value = new MathMethodService()
                                         .FoundationCount(YourJob.Value, DailyExpenese.Value);
                                        Result.Value = -1 * Value;
                                    }
                                }
                                if (YourCard.Type == "強迫中獎") // 強迫中獎 直接影響目前資料
                                {
                                }
                                if (YourCard.Type == "天選之人") // 直接影響當前抽卡人資料
                                {
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

            return Result;
        }
    }
}
