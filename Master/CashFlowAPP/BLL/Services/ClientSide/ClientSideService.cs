using BLL.IServices;
using Common.Enum;
using Common.Methods;
using Common.Model;
using Common.Model.AdminSide;
using DPL.EF;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace BLL.Services.ClientSide
{
    public class ClientSideService : IClientSideService
    {
        private readonly IMemoryCache _MemoryCache;
        private readonly CashFlowDbContext _CashFlowDbContext;
        public ClientSideService(
            CashFlowDbContext cashFlowDbContext,
            IMemoryCache memoryCache
        )
        {
            _CashFlowDbContext = cashFlowDbContext;
            _MemoryCache = memoryCache;
        }

        /// <summary>
        /// 取得數字驗證加密字串
        /// </summary>
        /// <returns></returns>
        public async Task<ApiResponse> GetJwtValidateCode()
        {
            // 獲取驗證碼
            var ValidateCode = Method.CreateValidateCode(4);
            // JWT 加密
            var JwtCode = Jose.JWT.Encode(
                    ValidateCode, Encoding.UTF8.GetBytes("錢董"),
                    Jose.JwsAlgorithm.HS256
                );
            var Res = new ApiResponse();
            Res.Success = true;
            Res.Code = (int)ResponseStatusCode.Success;
            Res.Message = "成功取得";
            Res.Data = new
            {
                JwtCode,
                ValidateCode
            };
            return Res;
        }

        /// <summary>
        /// 使用者註冊
        /// </summary>
        /// <param name="Req"></param>
        /// <returns></returns>
        public async Task<ApiResponse> UserSignUp(ApiRequest<UserSignUpDto> Req)
        {
            var Res = new ApiResponse();
            var IsCreated = _CashFlowDbContext.Users.Any(m => m.Email == Req.Args.Email);
            if (IsCreated)
            {
                Res.Success = false;
                Res.Code = (int)ResponseStatusCode.IsCreated;
                Res.Message = "此帳號已存在，無法重複建立";

                return Res;
            }
            // 驗證碼 token 解密
            var DeJWTcode = Jose.JWT.Decode(Req.Args.JwtCode, Encoding.UTF8.GetBytes("錢董"), Jose.JwsAlgorithm.HS256);
            if (Req.Args.ValidateCode != DeJWTcode)
            {
                Res.Success = false;
                Res.Code = (int)ResponseStatusCode.ValidateFail;
                Res.Message = "驗證碼錯誤";

                return Res;
            }

            var user = new User();
            user.Email = Req.Args.Email;
            user.Name = Req.Args.Name;
            user.Password = Req.Args.Password;//HashToDo
            user.RoleId = 1; //Todo
            user.Status = (byte)StatusCode.Enable;

            _CashFlowDbContext.Users.Add(user);
            _CashFlowDbContext.SaveChanges();

            Res.Success = true;
            Res.Code = (int)ResponseStatusCode.Success;
            Res.Message = "註冊成功";
            return Res;
        }

        /// <summary>
        /// 使用者登陸
        /// </summary>
        /// <param name="Req"></param>
        /// <returns></returns>
        public async Task<ApiResponse> UserLogin(ApiRequest<ClientUserLogin> Req)
        {
            var Res = new ApiResponse();
            var user = _CashFlowDbContext.Users
                .FirstOrDefault(m => m.Email == Req.Args.Email);

            if (user == null)
            {
                Res.Success = false;
                Res.Code = (int)ResponseStatusCode.CannotFind;
                Res.Message = "尚未註冊此帳號";
                return Res;
            }
            else
            {
                if (Req.Args.Password != user.Password)
                {
                    Res.Success = false;
                    Res.Code = (int)ResponseStatusCode.CannotFind;
                    Res.Message = "帳號或密碼錯誤";
                    return Res;
                }
                else
                {
                    var UserInfo = new UserInfo();

                    UserInfo.Id = user.Id;
                    UserInfo.Name = user.Name;
                    UserInfo.Email = user.Email;
                    UserInfo.RoleId = user.RoleId;

                    // 將 User 資料，以 UserInfo 塞進 token，方便取用
                    var JwtCode = Jose.JWT.Encode(UserInfo, Encoding.UTF8.GetBytes("錢董"), Jose.JwsAlgorithm.HS256);

                    Res.Data = new { JwtCode, UserId = user.Id }; // 匿名物件
                    Res.Success = true;
                    Res.Code = (int)ResponseStatusCode.Success;
                    Res.Message = "成功登入";

                    return Res;
                }
            }
        }

        /// <summary>
        /// 使用者抽卡
        /// </summary>
        /// <param name="Req"></param>
        /// <returns></returns>
        public async Task<ApiResponse> DrawCard(ApiRequest<string> Req)
        {
            var Res = new ApiResponse();
            return Res;
        }

        /// <summary>
        /// 全刪全建
        /// </summary>
        /// <param name="Req"></param>
        /// <returns></returns>
        public async Task<ApiResponse> UserAnswersUpdate(ApiRequest<List<CreateAnswerQuestionArgs>> Req)
        {
            var answerQuestions = new List<AnswerQuestion>();

            var SussList = new List<int>();
            var UserId = Req.Args[0].UserId;
            var UserAnswerList = _CashFlowDbContext.AnswerQuestions.Where(x => x.UserId == UserId).ToList();
            _CashFlowDbContext.AnswerQuestions.RemoveRange(UserAnswerList);
            _CashFlowDbContext.SaveChanges();

            foreach (var Arg in Req.Args)
            {
                var answerQuestion = new AnswerQuestion();
                answerQuestion.Id = Arg.Id;
                answerQuestion.Answer = Arg.Answer;
                answerQuestion.QusetionId = Arg.QusetionId;
                answerQuestion.UserId = Arg.UserId;

                answerQuestions.Add(answerQuestion);
            }

            _CashFlowDbContext.AddRange(answerQuestions);
            if (Req.Args[0].UserId != -1)
            {
                _CashFlowDbContext.SaveChanges();
            }

            // 不做銷毀 Dispose 動作，交給 DI 容器處理

            // 此處 SaveChanges 後 SQL Server 會 Tracking 回傳新增後的 Id
            SussList = answerQuestions.Select(x => x.Id).ToList();

            var Res = new ApiResponse();
            Res.Data = $@"已新增以下筆數(Id)：[{string.Join(',', SussList)}]";
            Res.Success = true;
            Res.Code = (int)ResponseStatusCode.Success;
            Res.Message = "收到你的報告了，社畜";

            return Res;
        }

        public async Task<ApiResponse> ReadFiInfo(ApiRequest<int?> Req)
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


            var Res = new ApiResponse();
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

            Res.Data = new FiInfo
            {
                UserId = Req.Args,
                CurrentMoney = 0,
                CashFlowIncome = CashFlowIncome,
                CashFlowExpense = CashFlowExpense,
                Asset = Asset,
                Liabilities = Liabilities,
                NowCardId = null,
                NowCardValue = null,
            };
            Res.Success = true;
            Res.Code = (int)ResponseStatusCode.Success;
            return Res;
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

        public async Task<ApiResponse> SupportCardDev()
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

            var AssetDices =
                    AssetAndCategory
                    .Select(x => new RandomItem<int>
                    {
                        SampleObj = x.Id,
                        Weight = 1
                    })
                    .ToList();

            var CashFlowDices =
                    CashFlowAndCategory
                    .Select(x => new RandomItem<int>
                    {
                        SampleObj = x.Id,
                        Weight = 1
                    })
                    .ToList();

            var _Random = StaticRandom();
            var CardList = new List<CategoryMix>();

            // 資產、現金流 0-2 的組合
            for (int i = 0; i < 5; i++)
            {
                var CashFlow = new List<int>();
                var Asset = new List<int>();

                int Count = _Random.Next(0, 2);
                int Count1 = _Random.Next(0, 2);
                var CashFlowList = new List<CashFlowAndCategoryModel>();
                var AssetFlowList = new List<AssetAndCategoryModel>();

                for (int j = 0; j < Count; j++)
                {
                    var CashFlowDice = Method.RandomWithWeight(CashFlowDices);
                    CashFlow.Add(CashFlowDice);
                    var CardCashFlow = CashFlowAndCategory.FirstOrDefault(c => c.Id == CashFlowDice);
                    CashFlowList.Add(CardCashFlow);
                }

                for (int j = 0; j < Count1; j++)
                {
                    var AssetFromDice = Method.RandomWithWeight(AssetDices);
                    Asset.Add(AssetFromDice);
                    var CardAsset = AssetAndCategory.FirstOrDefault(c => c.Id == AssetFromDice);
                    AssetFlowList.Add(CardAsset);
                }

                CardList.Add(new CategoryMix { CashFlows = CashFlowList, Assets = AssetFlowList });
            }

            var Res = new ApiResponse();
            Res.Data = CardList;
            Res.Success = true;

            // 直接新增到卡片表內 ( 會在拆分影響全部類別或影響子類 )
            foreach (var Item in CardList)
            {
                string AssetsName, CashFlowsName;

                var _Card = new Card();
                _Card.Name = "";
                _Card.Status = (int)StatusCode.Disable;
                _Card.Weight = 1;
                _CashFlowDbContext.Cards.Add(_Card);
                _CashFlowDbContext.SaveChanges();

                // EffectTables Id  Name                Status
                //              1   CashFlow            1
                //              2   Asset               1
                //              3   CashFlowCategories  1
                //              4   AssetCategories     1

                var CardName = "";

                foreach (var Asset in Item.Assets)
                {
                    var _CardEffect = new CardEffect();

                    int Count = _Random.Next(1, 3); // 成功了但這都只會抽到 1 = =
                    if (Count == 1) // 類別
                    {
                        CardName += $"[資產類別：{Asset.AssetCategoryName}]";
                        _CardEffect.TableId = Asset.AssetCategoryId;
                        _CardEffect.Description = Asset.AssetCategoryName;
                        _CardEffect.CardId = _Card.Id;
                        _CardEffect.EffectTableId = 4;
                    }
                    else
                    {
                        CardName += $"[資產：{Asset.Name}]";
                        _CardEffect.TableId = Asset.Id;
                        _CardEffect.Description = Asset.Name;
                        _CardEffect.CardId = _Card.Id;
                        _CardEffect.EffectTableId = 2;
                    }

                    _CashFlowDbContext.CardEffects.Add(_CardEffect);
                    _CashFlowDbContext.SaveChanges();
                }

                foreach (var CashFlow in Item.CashFlows)
                {
                    var _CardEffect = new CardEffect();

                    int Count = _Random.Next(1, 2); // 成功了但這都只會抽到 1 = =
                    if (Count == 1) // 類別
                    {
                        CardName += $"[現金流類別：{CashFlow.CashFlowCategoryName}]";
                        _CardEffect.TableId = CashFlow.CashFlowCategoryId; // 影響類別內流水號 ( 外鍵 )
                        _CardEffect.Description = CashFlow.CashFlowCategoryName; // 影響效果
                        _CardEffect.CardId = _Card.Id; // 卡片流水號 ( 外鍵 )
                        _CardEffect.EffectTableId = 3; // 影響資料表流水號 ( 外鍵 )
                    }
                    else
                    {
                        CardName += $"[現金流：{CashFlow.Name}]";
                        _CardEffect.TableId = CashFlow.Id;
                        _CardEffect.Description = CashFlow.Name;
                        _CardEffect.CardId = _Card.Id;
                        _CardEffect.EffectTableId = 1;
                    }

                    _CashFlowDbContext.CardEffects.Add(_CardEffect);
                    _CashFlowDbContext.SaveChanges();
                }

                _Card.Name = CardName;
                _CashFlowDbContext.SaveChanges();
            }

            return Res;
        }
    }
}
