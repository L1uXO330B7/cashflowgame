using Microsoft.AspNetCore.Mvc;
using Common.Model;
using BLL.IServices;
using StackExchange.Profiling;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.Controllers
{
   [Route("api/[controller]/[action]")]
    [ApiController]
    public class UsersController : ControllerBase, ICrudController<CreateUserArgs, int?, string, int?>
    {
        private IUsersService<CreateUserArgs, int?, string, int?> _UserService;

        public UsersController(
            IUsersService<CreateUserArgs, int?, string, int?> IUsersService
        ) // 建構子注入
        {
            _UserService = IUsersService;
        }
        [HttpPost]
        public Task<ApiResponse> Create([FromBody] ApiRequest<CreateUserArgs> Req)
        {
            return _UserService.Create(Req);
        }
        [HttpPost]
        public Task<ApiResponse> Delete([FromBody] ApiRequest<int?> Req)
        {
            return _UserService.Delete(Req);
        }
        [HttpPost]
        public  Task<ApiResponse> Read([FromBody] ApiRequest<int?> Req)
        {
            return _UserService.Read(Req);

        }
        [HttpPost]
        public Task<ApiResponse> Update([FromBody] ApiRequest<string> Req)
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


