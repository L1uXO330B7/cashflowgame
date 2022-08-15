using Microsoft.AspNetCore.Mvc;
using Common.Model;
using BLL.IServices;
using StackExchange.Profiling;
using Common.Model.AdminSide;
using DPL.EF;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.Controllers.AdminSide
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UsersController : Controller, 
        ICrudController<List<CreateUserArgs>, List<ReadUserArgs>, List<UpdateUserArgs>, List<int?>>
    {
        private IUsersService<List<CreateUserArgs>, List<ReadUserArgs>, List<UpdateUserArgs>, List<int?>> _UserService;

        /// <summary>
        /// 建構子
        /// </summary>
        /// <param name="IUsersService"></param>
        public UsersController(
            IUsersService<List<CreateUserArgs>, List<ReadUserArgs>, List<UpdateUserArgs>, List<int?>> IUsersService
        )
        {
            _UserService = IUsersService;
        }

        [HttpPost]
        public async Task<ApiResponse> Create([FromBody] ApiRequest<List<CreateUserArgs>> Req)
        {
            return await _UserService.Create(Req);
        }

        [HttpPost]
        public async Task<ApiResponse> Delete([FromBody] ApiRequest<List<int?>> Req)
        {
            return await _UserService.Delete(Req);
        }

        /// <summary>
        /// Args = [] 時為未篩選條件全查
        /// </summary>
        /// <param name="Req"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ApiResponse> Read([FromBody] ApiRequest<List<ReadUserArgs>> Req)
        {
            return await _UserService.Read(Req);
        }

        [HttpPost]
        public async Task<ApiResponse> Update([FromBody] ApiRequest<List<UpdateUserArgs>> Req)
        {
            return await _UserService.Update(Req);
        }
    }
}


