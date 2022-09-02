using BLL.IServices;
using Common.Enum;
using Common.Methods;
using Common.Model;
using Common.Model.AdminSide;
using DPL.EF;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace BLL.Services.ClientSide
{
    public class ClientSideService : IClientSideService
    {
        private readonly CashFlowDbContext _CashFlowDbContext;

        public ClientSideService(CashFlowDbContext cashFlowDbContext)
        {
            _CashFlowDbContext = cashFlowDbContext;
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
            _CashFlowDbContext.SaveChanges();
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
            var CashFlowResult = new List<dynamic>();
            var AssetResult = new List<dynamic>();

            if (Req.Args == null)
            {
                var _Random = StaticRandom();

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
                        // 金融商品筆數
                        var InvestCount = AssetAndCategory.Where(x => x.ParentId == 46).ToList().Count;
                        // 定存價值=薪水*隨機數字*
                        var Dice = _Random.Next(1, InvestCount);
                        YourAssets.Value = (YourJob.Value - DailyExpenese.Value) * Dice;
                        YourAssets.GuidCode = GuidCode;
                    }

                    // 學貸
                    if (YourAssets.AssetCategoryId == 24)
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

            Res.Data = new { CashFlowResult, AssetResult };
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
    }
}
