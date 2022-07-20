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

        public int test()
        {
            return cashFlowDbContext.Users.ToList().Count;
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
