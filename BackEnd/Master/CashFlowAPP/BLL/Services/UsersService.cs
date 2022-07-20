using BLL.IServices;
using Common.Model;
using DPL.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class UsersService : ServiceBase, IUsersService<CreateUserArgs, string, string, string, string>
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

        public async Task<ApiResponse> Create(ApiRequest<CreateUserArgs> Req)
        {
            var users = new User();
            users.Email = Req.Args.Email;
            users.Password = Req.Args.Password;
            users.Name = Req.Args.Name;
            users.Status = Req.Args.Status;
            users.RoleId = Req.Args.RoleId;
            cashFlowDbContext.Add(users);
            cashFlowDbContext.SaveChanges();

            var Res = new ApiResponse();
            Res.Success = true;
            Res.Code = "0000";
            Res.Message = "成功";

            return Res;
        }

        public Task<ApiResponse> Delete(ApiRequest<string> Args)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse> Read(ApiRequest<string> Args)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse> ReadAll(ApiRequest<string> Args)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse> Update(ApiRequest<string> Args)
        {
            throw new NotImplementedException();
        }
    }
}
