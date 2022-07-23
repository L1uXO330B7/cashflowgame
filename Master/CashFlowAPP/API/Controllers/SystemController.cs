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
        public SystemController()
        {
        }

        /// <summary>
        /// 獲取 MiniProfiler HTML 片段
        /// </summary>
        /// <returns></returns>
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
    }
}
