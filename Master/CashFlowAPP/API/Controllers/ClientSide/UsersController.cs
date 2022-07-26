using Common.Model;
using DPL.EF;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.ClientSide
{
    [Route("api/[controller]/[action]")]
    public class UsersController : Controller
    {
        /// <summary>
        /// 寄送驗證信
        /// </summary>
        /// <param name="mail"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        [HttpPost]
        public async Task<ApiResponse> SendVerificationMail(Mail mail)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 使用者註冊
        /// </summary>
        /// <param name="Req"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        [HttpPost]
        public async Task<ApiResponse> SignUp(ApiRequest<User> Req)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 使用者登陸
        /// </summary>
        /// <param name="Req"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        [HttpPost]
        public async Task<ApiResponse> Login(ApiRequest<string> Req)
        {
            throw new NotImplementedException();
        }
    }
}
