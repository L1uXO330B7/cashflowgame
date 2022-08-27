using BLL.IServices;
using Common.Enum;
using Common.Methods;
using Common.Model;
using Common.Model.AdminSide;
using DPL.EF;
using System.Text;
using static Common.Model.ClientSideModel;

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

    }
}
