using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

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
            //var secret = AppSettingHelper.GetSection("TokenSecret").Value; //加密字串

            //string Authorization = actionContext.HttpContext.Request.Headers["Authorization"];

            //if (Authorization != null && Authorization.StartsWith("Bearer"))
            //{
            //    try
            //    {
            //        //get service
            //        var authService = actionContext.HttpContext.RequestServices.GetService<IAuthService>();
            //        //get client ip 
            //        var iP = IpHelper.GetClinetIPAddress(actionContext.HttpContext);

            //        var jwtToken = Authorization.Substring("Bearer ".Length).Trim();
            //        var jwtObject = Jose.JWT.Decode<JWTPayload>(
            //                jwtToken,
            //                Encoding.UTF8.GetBytes(secret),
            //                JwsAlgorithm.HS256);

            //        // TODO 驗證是否存在
            //        //var taskCheckJWTLive = _service.CheckJWTLive(jwtObject.UserId, jwtToken);
            //        //taskCheckJWTLive.Wait();
            //        var isLive = true;

            //        if (isLive)
            //        {
            //            //actionContext.HttpContext.Items.Add("jwtPayload", jwtObject);
            //            //驗權限
            //            var isRoleResult = authService.CheckAuth(new UserAuthArgs() { FunctionId = this.EnumFunctionId, Ip = iP }, jwtObject).Result;
            //            var isRole = isRoleResult.Entries;

            //            if (isRole)
            //                actionContext.HttpContext.Items.Add("jwtPayload", jwtObject);
            //            else // 沒權限
            //                actionContext.Result = new CustomForbiddenResult("權限不足");
            //        }
            //        else // 過期
            //            actionContext.Result = new UnauthorizedResult();


            //    }
            //    catch (Exception ex)
            //    {
            //        actionContext.Result = new UnauthorizedResult();
            //    }
            //}
            //else
            //{
            //    // 前台頁面不處理
            //    if (this.EnumFunctionId != (int)Enum.Functions.none)
            //    {
            //        actionContext.Result = new UnauthorizedResult();
            //    }
            //}

            //base.OnActionExecuting(_ActionExecutingContext);
        }
    }
}
