using Common.Enum;
using Common.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using System.Text;

namespace API.Filter
{
    /// <summary>
    /// 驗證標籤此處就不用全域註冊，保留彈性可以 Controller 或 Action 註冊
    /// </summary>
    public class AuthorizationFilter : IAuthorizationFilter
    {
        /// <summary>
        /// 可注入參數 [AuthorizationFilter( XXX , XXX , XXX )]
        /// </summary>
        public AuthorizationFilter()
        {
        }

        /// <summary>
        /// 進入 PipeLine 驗證即可
        /// </summary>
        /// <param name="_ActionExecutingContext"></param>
        public void OnAuthorization(AuthorizationFilterContext _AuthorizationContext)
        {
            string Authorization = _AuthorizationContext.HttpContext.Request.Headers["Authorization"];

            //  取得 Authorization 後，要用 Trim() 去掉其中的空白，跟 Substring() 去掉其中 Bearer 字串
            var JwtToken = Authorization.Substring("Bearer ".Length).Trim();

            // 滿足這兩項條件就滿足 OAuth 2.0
            // https://ithelp.ithome.com.tw/articles/10197166
            if (!string.IsNullOrEmpty(JwtToken) && JwtToken != "null")
            {
                // 取得客戶端 IP 可以寫白黑名單
                var ClientIp = _AuthorizationContext.HttpContext.Connection.RemoteIpAddress;

                // 解密
                var JwtObject = Jose.JWT.Decode<UserInfo>(
                        JwtToken, Encoding.UTF8.GetBytes("錢董"),
                        Jose.JwsAlgorithm.HS256);

                // 確定 Token 生命週期
                var IsLiving = (JwtObject.TokenCreatedTime
                    .AddHours(JwtObject.TokenExpiredHours)) > DateTime.Now;

                if (IsLiving)
                {
                    // 在 HTTP 封包塞入 Key:Value
                    _AuthorizationContext.HttpContext.Items.Add("UserInfo", JwtObject);
                }
                else
                {
                    // 過期
                    var Res = new ApiResponse();
                    Res.Code = (int)ResponseStatusCode.Unauthorized;
                    Res.Message = "請重新登入";
                    Res.Success = false;

                    _AuthorizationContext.Result = new ContentResult
                    {
                        // 返回状态码设置为200，表示成功
                        StatusCode = StatusCodes.Status200OK,
                        // 设置返回格式
                        ContentType = "application/json;charset=utf-8",
                        Content = JsonConvert.SerializeObject(Res)
                    };
                }
            }
            else
            {
                var Res = new ApiResponse();
                Res.Code = (int)ResponseStatusCode.Unauthorized;
                Res.Message = "尚未登入";
                Res.Success = false;

                _AuthorizationContext.Result = new ContentResult
                {
                    // 返回状态码设置为200，表示成功
                    StatusCode = StatusCodes.Status200OK,
                    // 设置返回格式
                    ContentType = "application/json;charset=utf-8",
                    Content = JsonConvert.SerializeObject(Res)
                };
            }
        }
    }
}
