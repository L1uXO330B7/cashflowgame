using BLL.IServices;
using DPL.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class UsersService : IUsersService
    {
        public void test(string username, CashFlowDbContext db)
        {
            try
            {

                var _Users = new User();
                _Users.Email = "ccc";
                _Users.Password = "bbb";
                _Users.Name = "aaa";
                _Users.Status = 1;
                _Users.RoleId = 1;
                db.Users.Add(_Users);
                db.SaveChanges();

            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
