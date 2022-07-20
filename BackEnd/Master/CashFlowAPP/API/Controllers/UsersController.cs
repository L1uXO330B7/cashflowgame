using Microsoft.AspNetCore.Mvc;
using API.IController;
using Common.Model;
using BLL.IServices;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase, ICRUD<string, string, string, string>
    {
        private IUsersService userService;

        public UsersController(
            IUsersService _IUsersService
        ) //建構子注入
        {
            userService = _IUsersService;
        }

        public Task<ApiResponse> Create([FromBody] ApiRequest<string> Args)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse> Delete([FromBody] ApiRequest<string> Args)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse> Read([FromBody] ApiRequest<string> Args)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse> Update([FromBody] ApiRequest<string> Args)
        {
            throw new NotImplementedException();
        }
    }
}
