using BLL.IServices;
using Common.Enum;
using Common.Methods;
using Common.Model;
using Common.Model.AdminSide;
using DPL.EF;
using System.Text;
using static Common.Model.ClientSideModel;
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
            var JwtCode = Jose.JWT.Encode(ValidateCode, Encoding.UTF8.GetBytes("錢董"), Jose.JwsAlgorithm.HS256);
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
            user.Name = $@"社畜{Method.CreateValidateCode(4)}";
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
                   new { a.Id, a.Name, a.Value, a.Description, AssetCategoryName = ac.Name, a.AssetCategoryId, ac.ParentId })
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
                   new { c.Id, c.Name, c.Value, c.Description, CashFlowCategoryName = cc.Name, c.CashFlowCategoryId, cc.ParentId })
                 .ToList();


            var Res = new ApiResponse();
            var CashFlowResult = new List<dynamic>();
            var AssetResult = new List<dynamic>();

            if (Req.Args == null)
            {
                // 先抽職業
                var Jobs =
                      CashFlowAndCategory
                     .Where(c => c.CashFlowCategoryName == "工作薪水")
                     .Select(x => new RandomItem<int>
                     {
                         SampleObj = x.Id,
                         Weight = x.Value == 0 ? 1 / 300000 : (1 / x.Value) // 降低老闆的機率 value = 0 
                     })
                     // value = 0 ，執行左邊也就是老闆
                     .ToList();
                var Job = Method.RandomWithWeight(Jobs);
                var YourJob = CashFlowAndCategory.FirstOrDefault(c => c.Id == Job);
                CashFlowResult.Add(YourJob);

                // 生活花費
                var _Random = new Random(Guid.NewGuid().GetHashCode()); // 讓隨機機率離散
                var DailyExpenese =
                    CashFlowAndCategory
                    .Select(c => new
                    {
                        c.Id,
                        c.Name,
                        Value = c.Value * _Random.Next(1, 5),
                        c.Description,
                        c.CashFlowCategoryName,
                        c.CashFlowCategoryId,
                        c.ParentId
                    })
                    .FirstOrDefault(c => c.CashFlowCategoryName == "生活花費");

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
                           c.Id == AssetAndCategory.Count() &&
                           c.AssetCategoryName != "公司行號" && // 初始化抽到老闆才能有公司
                           c.ParentId != 28 && // 初始化抽到老闆才能有公司行號的子類別
                           c.Name != "房貸" // 有房子才有房貸，先排除
                    )
                    .Select(x => new RandomItem<int>
                    {
                        SampleObj = x.Id,
                        Weight = x.Value == 0 ? 0 : (1 / x.Value) // value = 0 為浮動利率
                    })
                    .ToList();

                // 先抽再根據抽到類別做特殊邏輯處理
                // 可重複抽取
                for (var i = 0; i < DrawCounts; i++)
                {
                    var AssetFromDice = Method.RandomWithWeight(AssetDices);
                    var YourAssets = AssetAndCategory.FirstOrDefault(a => a.Id == AssetFromDice);
                    AssetResult.Add(YourAssets);

                    // 有房子才有房貸
                    if (YourAssets.AssetCategoryId == 17) // 房地產
                    {
                        var MortgageRatio = _Random.Next(1, 8) / 100; // 新成屋最高八成
                        var MortgageRatioAsset = AssetAndCategory
                            .Select(a => new
                            {
                                a.Id,
                                a.Name,
                                Value = a.Value * MortgageRatio * -1,
                                a.Description,
                                a.AssetCategoryName,
                                a.AssetCategoryId,
                                a.ParentId
                            })
                            .FirstOrDefault(x => x.Name == "房貸");
                        AssetResult.Add(MortgageRatioAsset);
                    }
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

    }
}
