using Microsoft.AspNetCore.Mvc;
using DPL.EF;
using BLL.IServices;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly CashFlowDbContext _db;
        private IUsersService _UserService;

        public UsersController(
            CashFlowDbContext db,
            IUsersService UserService
            ) //建構子注入
        {
            _db = db;
            _UserService = UserService;
        }

        [HttpPost]
        public void Post([FromBody] User item)
        {
            _UserService.test(item.Name, _db);
        }
    }
}
