using Microsoft.AspNetCore.Mvc;
using Common.Model;
using BLL.IServices;
using StackExchange.Profiling;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase, ICrudController<CreateUserArgs, string, int, int>
    {
        private IUsersService<CreateUserArgs, string, string, string, string> userService;

        public UsersController(
            IUsersService<CreateUserArgs, string, string, string, string> _IUsersService
        ) // 建構子注入
        {
            userService = _IUsersService;
        }

        [HttpPost("Create")]
        public async Task<ApiResponse> Create([FromBody] ApiRequest<CreateUserArgs> Req)
        {
           return await userService.Create(Req);
        }
        [HttpPost("Delete")]
        public Task<ApiResponse> Delete([FromBody] ApiRequest<int> Req)
        {
            throw new NotImplementedException();
        }
        [HttpPost("Read")]
        public Task<ApiResponse> Read([FromBody] ApiRequest<string> Req)
        {
            throw new NotImplementedException();
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
            var html = MiniProfiler.Current.RenderIncludes(HttpContext);
            return Ok(html.Value);
        }
    }
}
