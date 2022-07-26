using BLL.IServices;
using BLL.Services;
using BLL.Services.ClientSide;
using Common.Model;
using DPL.EF;
using Microsoft.AspNetCore.Mvc;

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
            _SmtpConfig.Port = Configuration["SMTP:Port"];
            _SmtpConfig.IsSSL = Configuration["SMTP:IsSSL"];
            _SmtpConfig.AdminEmails = Configuration["SMTP:AdminEmails"];
            _SmtpConfig.Server = Configuration["SMTP:Server"];
            _SmtpConfig.Account = Configuration["SMTP:Account"];
            _SmtpConfig.Password = Configuration["SMTP:Password"];
            _SmtpConfig.SenderEmail = Configuration["SMTP:SenderEmail"];
        }
        /// <summary>
        /// 寄送驗證信
        /// </summary>
        /// <param name="mail"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        [HttpPost]

        public async Task<ApiResponse> SendVerificationMail([FromBody] Mail mail)
        {
            mail.Title = "";
            mail.Content = "";
            return await new ServiceBase().SendMail(_SmtpConfig, mail);
        }


        /// <summary>
        /// 使用者註冊
        /// </summary>
        /// <param name="Req"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        [HttpPost]
        public async Task<ApiResponse> UserSignUp(ApiRequest<UserSignUpDTO> Req)
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
        public async Task<ApiResponse> UserLogin(ApiRequest<UserSignUpDTO> Req)
        {
            return  await _ClientSideService.UserSignUp(Req);
        }
    }
}
