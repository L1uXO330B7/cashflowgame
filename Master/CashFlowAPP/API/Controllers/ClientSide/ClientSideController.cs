using BLL.IServices;
using Common.Model;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using static Common.Model.ClientSideModel;

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


        /// <summary>
        /// 使用者註冊
        /// </summary>
        /// <param name="Req"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        [HttpPost]
        public async Task<ApiResponse> UserSignUp([FromBody]ApiRequest<UserSignUpDto> Req)
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
        public async Task<ApiResponse> UserLogin([FromBody]ApiRequest<ClientUserLogin> Req)
        {

            return await _ClientSideService.UserLogin(Req);
        
        }
    }
}
