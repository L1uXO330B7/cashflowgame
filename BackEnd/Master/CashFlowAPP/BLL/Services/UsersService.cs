using BLL.IServices;
using DPL.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class UsersService : ServiceBase, IUsersService
    {
        public UsersService(CashFlowDbContext _CashFlowDbContext) : base(_CashFlowDbContext)
        {
        }

        public void test(string username)
        {
            try
            {
                var _Users = new User();
                _Users.Email = "ccc";
                _Users.Password = "bbb";
                _Users.Name = "aaa";
                _Users.Status = 1;
                _Users.RoleId = 1;

                cashFlowDbContext.Users.Add(_Users);
                cashFlowDbContext.SaveChanges();
                cashFlowDbContext.Dispose();

                // var UserList = cashFlowDbContext.Users.ToList(); // 會錯因為已經斷開連線

                using (var db = cashFlowDbContext) // 再次連線
                {
                    var UserList = db.Users.ToList();
                } // 斷開連線
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
