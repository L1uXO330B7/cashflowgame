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
    public class UsersService : ServiceBase, IUsersService<CreateUserArgs,int?,string,int?>
    {
        public UsersService(CashFlowDbContext _CashFlowDbContext) : base(_CashFlowDbContext)
        {

        }

        public int test()
        {
            return _CashFlowDbContext.Users.ToList().Count;
        }

        public async Task<ApiResponse> Create(ApiRequest<CreateUserArgs> Req)
        {
            User Users = new User();
            Users.Email = Req.Args.Email;
            Users.Password = Req.Args.Password;
            Users.Name = Req.Args.Name;
            Users.Status = Req.Args.Status;
            Users.RoleId = Req.Args.RoleId;
            _CashFlowDbContext.Add(Users);
            _CashFlowDbContext.SaveChanges();//不做銷毀dispose動作，交給 DI 容器處理


            var Res = new ApiResponse();
            Res.Success = true;
            Res.Code = "0000";
            Res.Message = "成功";

            return Res;
        }
        public async Task<ApiResponse> Read(ApiRequest<int?> Req)
        {
          var Res = new ApiResponse();
            if (Req.Args == null)
            {
                Res.Success = true;
                Res.Code = "0000";
                Res.Message = "成功";
                Res.Data = _CashFlowDbContext.Users.ToList();
            }
            else
            {
                var User = _CashFlowDbContext.Users.Find(Req.Args);
                if (User == null)
                {
                    Res.Success = true;
                    Res.Code = "0010";
                    Res.Message = "無此用戶";
                }
                else
                {
                    Res.Data = User;
                    Res.Success = true;
                    Res.Code = "0000";
                    Res.Message = "成功讀取";
                }
            }
            return Res;
        }


        public async Task<ApiResponse> Update(ApiRequest<string> Req)
        {
            var Res = new ApiResponse();

                var User = _CashFlowDbContext.Users.Find(Req.Args.Id);
                if (User == null)
                {
                    Res.Success = true;
                    Res.Code = "0010";
                    Res.Message = "無此用戶";
                }
                else
                {
                    Res.Data = User;
                    Res.Success = true;
                    Res.Code = "0000";
                    Res.Message = "成功讀取";
                }
            return Res;
        }

        public async Task<ApiResponse> Delete(ApiRequest<int?> Req)
        {
            var Res = new ApiResponse();
            if (Req.Args == null)
            {
                Res.Success = true;
                Res.Code = "0001";
                Res.Message = "刪除全部";
            }
            else
            {
                var user = _CashFlowDbContext.Users.Find(Req.Args);
                if (user == null)
                {
                    Res.Success = true;
                    Res.Code = "0010";
                    Res.Message = "無此用戶";
                }
                else
                {
                    _CashFlowDbContext.Users.Remove(user);
                    _CashFlowDbContext.SaveChanges();
                    Res.Success = true;
                    Res.Code = "0000";
                    Res.Message = "成功刪除";
                }

            }

            return Res;

        }




    }
}
