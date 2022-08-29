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

        /// <summary>
        /// RandomItemSample 控制機率隨機取樣
        /// </summary>
        [HttpGet]
        public dynamic RandomItemSample()
        {
            var RandomItemList = new List<RandomItem<string>>(); // 樣本容器，抽取樣本賦予權重，可塞泛型
            var RandomObj = new RandomItem<string>(); // 樣本物件
            RandomObj.SampleObj = "test7"; // 樣本名稱，依型別塞值
            RandomObj.Weight = 7; // 樣本權重
            RandomItemList.Add(RandomObj); // 塞進容器 

            RandomObj = new RandomItem<string>();
            RandomObj.SampleObj = "test3";
            RandomObj.Weight = 3;
            RandomItemList.Add(RandomObj);

            RandomObj = new RandomItem<string>();
            RandomObj.SampleObj = "test2";
            RandomObj.Weight = 2;
            RandomItemList.Add(RandomObj);

            RandomObj = new RandomItem<string>();
            RandomObj.SampleObj = "test11";
            RandomObj.Weight = 11;
            RandomItemList.Add(RandomObj);

            RandomObj = new RandomItem<string>();
            RandomObj.SampleObj = "test1";
            RandomObj.Weight = 1;
            RandomItemList.Add(RandomObj);

            RandomObj = new RandomItem<string>();
            RandomObj.SampleObj = "test0";
            RandomObj.Weight = 0;
            RandomItemList.Add(RandomObj);

            // test 10000 次抽取結果 Group 後回傳
            var Test = new List<string>(); // 測試
            for (var i = 0; i <= 1; i++) // 測試一萬次
            {
                Test.Add(Method.RandomWithWeight<string>(RandomItemList)); // 權重隨機方法
            }

            var SampleObjList = RandomItemList.Select(x => x.SampleObj).Distinct().ToList();
            //  Distinct 抓不重複的樣本物件
            var result = new List<Dictionary<string, int>>();
            // https://ithelp.ithome.com.tw/articles/10194934
            // 可帶類型的 <Tkey,Tvalue> 容器
            foreach (var Sample in SampleObjList)
            {
                var Item = new Dictionary<string, int>();
                Item.Add(Sample, Test.Where(x => x == Sample).Count());
                result.Add(Item);
            }

            return result;
        }
    }
}
