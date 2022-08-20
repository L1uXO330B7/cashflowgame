using API.Filter;
using BLL.Services;
using Common.Methods;
using Common.Model;
using DPL.EF;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Profiling;
using System.IO;
using System.Text;

namespace API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SystemController : Controller
    {
        public static SmtpConfig _SmtpConfig = new SmtpConfig();
        public CashFlowDbContext _CashFlowDbContext;
        public SystemController(
            CashFlowDbContext CashFlowDbContext,
            IConfiguration Configuration
        )
        {
            _CashFlowDbContext = CashFlowDbContext;
            _SmtpConfig.Port = Configuration["SMTP:Port"];
            _SmtpConfig.IsSSL = Configuration["SMTP:IsSSL"];
            _SmtpConfig.AdminEmails = Configuration["SMTP:AdminEmails"];
            _SmtpConfig.Server = Configuration["SMTP:Server"];
            _SmtpConfig.Account = Configuration["SMTP:Account"];
            _SmtpConfig.Password = Configuration["SMTP:Password"];
            _SmtpConfig.SenderEmail = Configuration["SMTP:SenderEmail"];
        }

        /// <summary>
        /// 測試寄信
        /// </summary>
        /// <param name="mail"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ApiResponse> TestSendMail([FromBody] Mail mail)
        {
            return await new ServiceBase().SendMail(_SmtpConfig, mail);
        }

        /// <summary>
        /// 獲取 MiniProfiler HTML 片段
        /// </summary>
        /// <returns></returns>
        //[TypeFilter(typeof(AuthorizationFilter))]
        [HttpGet]
        public IActionResult GetMiniProfilerScript()
        {
            // https://stackoverflow.com/questions/31269525/how-to-create-javascript-file-dynamically-using-c
            // https://stackoverflow.com/questions/2572023/return-a-js-file-from-asp-net-mvc-controller
            // https://stackoverflow.com/questions/57489830/how-to-return-javascript-from-controller
            // https://docs.microsoft.com/zh-tw/dotnet/api/system.io.file.create?view=net-6.0
            // https://stackoverflow.com/questions/16072709/converting-string-to-byte-array-in-c-sharp

            var Script = MiniProfiler.Current.RenderIncludes(HttpContext);
            byte[] ByteArray = Encoding.ASCII.GetBytes(Script.ToString());
            return File(ByteArray, "application/javascript", "MiniProfiler.js");
        }

        /// <summary>
        /// 依表名稱產生模板檔案
        /// </summary>
        [HttpGet]
        public void CreateTemplateByTableName()
        {
            new SystemService(_CashFlowDbContext).CreateTemplateByTableName();
        }
    }
}
