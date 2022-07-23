using Microsoft.AspNetCore.Mvc;
using StackExchange.Profiling;

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
            // https://stackoverflow.com/questions/57489830/how-to-return-javascript-from-controller
            var Script = MiniProfiler.Current.RenderIncludes(HttpContext);
            var JavaScriptResult = new ContentResult();
            JavaScriptResult.Content = Script.ToString();
            JavaScriptResult.ContentType = "application/javascript";
            return JavaScriptResult;
        }
    }
}
