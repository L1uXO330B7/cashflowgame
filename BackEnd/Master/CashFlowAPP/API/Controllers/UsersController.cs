using Microsoft.AspNetCore.Mvc;
using DPL.EF;
using BLL.IServices;
using API.IController;
using Common.Model;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase, ICRUD
    {
        private IUsersService userService;

        public UsersController(
            IUsersService _IUsersService
        ) //建構子注入
        {
            userService = _IUsersService;
        }

        public Task<ApiResponse> Create<T>(ApiRequest<T> Args)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse> Delete<T>(ApiRequest<T> Args)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse> Read<T>(ApiRequest<T> Args)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse> Update<T>(ApiRequest<T> Args)
        {
            throw new NotImplementedException();
        }
    }
}
