using Microsoft.AspNetCore.Mvc;
using Common.Model;
using BLL.IServices;
using StackExchange.Profiling;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase, ICrudController<CreateUserArgs, int, int, int>
    {
        private IUsersService<CreateUserArgs, int, string, int> _UserService;

        public UsersController(
            IUsersService<CreateUserArgs, int, string,int> IUsersService
        ) // 建構子注入
        {
            _UserService = IUsersService;
        }

        [HttpPost("Create")]
        public async Task<ApiResponse> Create([FromBody] ApiRequest<CreateUserArgs> Req)
        {
           return await _UserService.Create(Req);
        }
        [HttpPost("Delete")]
        public async Task<ApiResponse> Delete([FromBody] ApiRequest<int> Req)
        {
            var New = new ApiRequest<string>(); //轉型多此一舉
            New.Args = Req.Args.ToString();
            return await _UserService.Delete(Req);
        }
        [HttpPost("Read")]
        public async Task<ApiResponse> Read([FromBody] ApiRequest<int> Req)
        {
            return await _UserService.Read(Req);
        }
        [HttpPost("ReadAll")]
        public Task<ApiResponse> ReadAll([FromBody] ApiRequest<string> Req)
        {
            throw new NotImplementedException();
        }
        [HttpPost("Update")]
        public Task<ApiResponse> Update([FromBody] ApiRequest<int> Req)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 獲取 MiniProfiler HTML 片段
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetMiniProfilerScript")]
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
