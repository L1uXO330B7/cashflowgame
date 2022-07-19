using Microsoft.AspNetCore.Mvc;
using DPL.EF;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly CashFlowDbContext _db;


        public UsersController(CashFlowDbContext db) //建構子注入
        {
 
            _db= db;
        }

        // GET: api/<UsersController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<UsersController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<UsersController>
        [HttpPost]
        public ActionResult<User> Post([FromBody] User item)
        {
            _db.Users.Add(item);
            _db.SaveChanges();

            return CreatedAtAction(nameof(Get), new { id = item.Id }, item);
        }

        // PUT api/<UsersController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<UsersController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
