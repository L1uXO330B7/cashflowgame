using Microsoft.AspNetCore.Mvc;
using API.IController;
using Common.Model;
using BLL.IServices;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase, ICrudController<CreateUserArgs, string, int, int>
    {
        private IUsersService<CreateUserArgs, string, string, string, string> _UserService;

        public UsersController(
            IUsersService<CreateUserArgs, string, string, string, string> IUsersService
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
    }
}
