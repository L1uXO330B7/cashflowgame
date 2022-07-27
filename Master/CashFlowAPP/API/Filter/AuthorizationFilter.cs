using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Text;
using static Common.Model.ClientSideModel;

namespace API.Filter
{
    /// <summary>
    /// 驗證標籤此處就不用全域註冊，保留彈性可以 Controller 或 Action 註冊
    /// </summary>
    public class AuthorizationFilter : ActionFilterAttribute
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
        public override void OnActionExecuting(ActionExecutingContext _ActionExecutingContext)
        {

            string Authorization = _ActionExecutingContext.HttpContext.Request.Headers["Authorization"];

            // 滿足這兩項條件就滿足 OAuth 2.0
            // https://ithelp.ithome.com.tw/articles/10197166
            if (Authorization != null && Authorization.StartsWith("Bearer"))
            {
                // 取得客戶端 IP 
                var ClientIp = _ActionExecutingContext.HttpContext.Connection.RemoteIpAddress;

                //  取得 Authorization 後，要用 Trim() 去掉其中的空白，跟 Substring() 去掉其中 Bearer 字串
                var JwtToken = Authorization.Substring("Bearer ".Length).Trim();

                // 解密
                var JwtObject = Jose.JWT.Decode<UserInfo>(
                        JwtToken, Encoding.UTF8.GetBytes("錢董"),
                        Jose.JwsAlgorithm.HS256);
                // 確定 Token 生命週期
                var IsLiving = (JwtObject.TokenCreatedTime
                    .AddHours(JwtObject.TokenExpiredHours)) > DateTime.Now;
                if (IsLiving)
                {   // 在 HTTP 封包塞入 Key:Value
                    _ActionExecutingContext.HttpContext.Items.Add("UserInfo", JwtObject);

                }
                else
                {   // 過期
                    _ActionExecutingContext.Result = new UnauthorizedResult();
                }
            }
        }

    }
}
