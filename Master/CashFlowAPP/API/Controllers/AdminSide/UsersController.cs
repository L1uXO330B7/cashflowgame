using Microsoft.AspNetCore.Mvc;
using Common.Model;
using BLL.IServices;
using StackExchange.Profiling;
using Common.Model.AdminSide;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.Controllers.AdminSide
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UsersController : ControllerBase, ICrudController<CreateUserArgs, int?, UpdateUserArgs, int?>
    {
        private IUsersService<CreateUserArgs, int?, UpdateUserArgs, int?> _UserService;

        public UsersController(
            IUsersService<CreateUserArgs, int?, UpdateUserArgs, int?> IUsersService
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

        /// <summary>
        /// Req.args 如為 null 則查全部喔 !
        /// </summary>
        /// <param name="Req"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ApiResponse> Read([FromBody] ApiRequest<int?> Req)
        {
            return await _UserService.Read(Req);
        }

        [HttpPost]
        public async Task<ApiResponse> Update([FromBody] ApiRequest<UpdateUserArgs> Req)
        {
            return await _UserService.Update(Req);
        }
    }
}


