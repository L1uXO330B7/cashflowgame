using BLL.IServices;
using Common.Enum;
using Common.Model;
using Common.Model.AdminSide;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Net;

namespace API.Controllers.ClientSide
{
    [Route("api/[controller]/[action]")]
    public class ClientSideController : Controller
    {
        private IClientSideService _ClientSideService;

        // 注入快取記憶體套件
        private readonly IMemoryCache _MemoryCache;
        public static SmtpConfig _SmtpConfig = new SmtpConfig();

        public ClientSideController
        (
          IClientSideService ClientSideService,
          IConfiguration Configuration,
          IMemoryCache memoryCache
        )
        {
            _MemoryCache = memoryCache;
            _ClientSideService = ClientSideService;
            //_SmtpConfig.Port = Configuration["SMTP:Port"];
            //_SmtpConfig.IsSSL = Configuration["SMTP:IsSSL"];
            //_SmtpConfig.AdminEmails = Configuration["SMTP:AdminEmails"];
            //_SmtpConfig.Server = Configuration["SMTP:Server"];
            //_SmtpConfig.Account = Configuration["SMTP:Account"];
            //_SmtpConfig.Password = Configuration["SMTP:Password"];
            //_SmtpConfig.SenderEmail = Configuration["SMTP:SenderEmail"];
        }

        /// <summary>
        /// 取得數字驗證碼
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        [HttpPost]
        public async Task<ApiResponse> GetVerificationCode()
        {
            return await _ClientSideService.GetJwtValidateCode();
        }

        #region 使用者

        /// <summary>
        /// 使用者註冊
        /// </summary>
        /// <param name="Req"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        [HttpPost]
        public async Task<ApiResponse> UserSignUp([FromBody] ApiRequest<UserSignUpDto> Req)
        {
            return await _ClientSideService.UserSignUp(Req);
        }

        /// <summary>
        /// 使用者登陸
        /// </summary>
        /// <param name="Req"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        [HttpPost]
        public async Task<ApiResponse> UserLogin([FromBody] ApiRequest<ClientUserLogin> Req)
        {
            return await _ClientSideService.UserLogin(Req);
        }

        /// <summary>
        /// 使用者答案全刪全建
        /// </summary>
        /// <param name="Req"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ApiResponse> UserAnswersUpdate([FromBody] ApiRequest<List<CreateAnswerQuestionArgs>> Req)
        {
            return await _ClientSideService.UserAnswersUpdate(Req);
        }

        /// <summary>
        /// 讀取用戶財報
        /// </summary>
        /// <param name="Req"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ApiResponse> ReadFiInfo([FromBody] ApiRequest<int?> Req)
        {
            var Res = new ApiResponse();

            if (Req.Args != null)
            {
                // Req.Args 是 UserId，用 UserId 來作為快取記憶體的 key
                // 如找無此 Key 就空出一個單位的記憶體來儲存
                var UserFiInfo = _MemoryCache.GetOrCreate(Req.Args, async (not) =>
                {
                    Res = await _ClientSideService.ReadFiInfo(Req);
                    FiInfo Data = (FiInfo)Res.Data;
                    //_MemoryCache.Set(
                    //Req.Args, Data,
                    //    new MemoryCacheEntryOptions()
                    //   .SetPriority(CacheItemPriority.NeverRemove));
                    //
                    //   <= 這樣是正常的但是 UserFiInfo 會變成 Task 包住導致解出來時還要再包層 Task，把滑鼠移上去看
                    return Data;
                });

                // Todo: Cache count
                Res.Success = true;
                Res.Code = (int)ResponseStatusCode.Success;
                Res.Data = UserFiInfo;
            }
            else
            {
                Res = await _ClientSideService.ReadFiInfo(Req);
            }
            return Res;
        }

        // Todo remove cache

        #endregion
    }
}
