using BLL.IServices;
using Common.Model;
using Common.Model.AdminSide;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace API.Controllers.ClientSide
{
    [Route("api/[controller]/[action]")]
    public class ClientSideController : Controller
    {
        private IClientSideService _ClientSideService;
        public static SmtpConfig _SmtpConfig = new SmtpConfig();

        public ClientSideController(

          IClientSideService ClientSideService,
          IConfiguration Configuration
        )
        {
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
        public async Task<ApiResponse> ReadFiInfo([FromBody]ApiRequest<int?> Req)
        {
            return await _ClientSideService.ReadFiInfo(Req);
        }

        #endregion
    }
}
