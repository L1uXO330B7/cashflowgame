using BLL.IServices;
using Common.Model;
using DPL.EF;
using Common.Enum;
using Common.Model.AdminSide;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace BLL.Services.AdminSide
{
    public class UsersService : ServiceBase,
        IUsersService<List<CreateUserArgs>, List<ReadUserArgs>, List<UpdateUserArgs>, List<int?>>
    {
        private readonly CashFlowDbContext _CashFlowDbContext;

        public UsersService(CashFlowDbContext cashFlowDbContext)
        {
            _CashFlowDbContext = cashFlowDbContext;
        }

        public async Task<ApiResponse> Create(ApiRequest<List<CreateUserArgs>> Req)
        {
            var Users = new List<User>();
            foreach (var Arg in Req.Args)
            {
                User User = new User();
                User.Email = Arg.Email;
                User.Password = Arg.Password;
                User.Name = Arg.Name;
                User.Status = Arg.Status;
                User.RoleId = Arg.RoleId;
                Users.Add(User);
            }

            _CashFlowDbContext.AddRange(Users);
            _CashFlowDbContext.SaveChanges(); // 不做銷毀 Dispose 動作，交給 DI 容器處理

            var Res = new ApiResponse();
            Res.Success = true;
            Res.Code = (int)ResponseStatusCode.Success;
            Res.Message = "成功新增";

            return Res;
        }

        public async Task<ApiResponse> Read(ApiRequest<List<ReadUserArgs>> Req)
        {
            var Res = new ApiResponse();
            if (Req.Args == null)
            {
                Res.Success = true;
                Res.Code = (int)ResponseStatusCode.Success;
                Res.Message = "成功讀取";
                Res.Data = _CashFlowDbContext.Users.ToList();
            }
            else
            {
                if (Req.Args.Count() > 0)
                {
                    var User = _CashFlowDbContext.Users
                        .AsNoTracking();

                    foreach (var Arg in Req.Args.Select((Val, Index) => (Val, Index)))
                    {
                        if (Arg.Val.Key == "Id")
                        {
                            var Ids = JsonConvert.DeserializeObject<List<int>>(Arg.Val.JsonString);
                            User.Where(x => Ids.Contains(x.Id));
                        }
                    }

                    var Data = User
                        .Skip((Req.PageIndex - 1) * Req.PageSize)
                        .Take(Req.PageSize)
                        .ToList();

                    Res.Data = Data;
                    Res.Success = true;
                    Res.Code = (int)ResponseStatusCode.Success;
                    Res.Message = "成功讀取";
                }
            }
            return Res;
        }

        public async Task<ApiResponse> Update(ApiRequest<List<UpdateUserArgs>> Req)
        {
            var Res = new ApiResponse();

            try
            {
                foreach (var Arg in Req.Args)
                {
                    var User = _CashFlowDbContext.Users
                        .FirstOrDefault(x => x.Id == Arg.Id);

                    if (User == null)
                    {
                        Res.Success = false;
                        Res.Code = (int)ResponseStatusCode.CannotFind;
                        Res.Message += $@"Id：{Arg.Id} 無此用戶\n";
                    }
                    else
                    {
                        User.Email = Arg.Email;
                        User.Password = Arg.Password;
                        User.Name = Arg.Name;
                        User.Status = Arg.Status;
                        User.RoleId = Arg.RoleId;
                        _CashFlowDbContext.SaveChanges();

                        Res.Data = User;
                        Res.Success = true;
                        Res.Code = (int)ResponseStatusCode.Success;
                        Res.Message = "成功更改";
                    }
                }
            }
            catch (Exception ex)
            {
                Res.Data = ex;
                Res.Success = false;
                Res.Code = (int)ResponseStatusCode.ExMessage;
                Res.Message = "伺服器忙碌中";
            }

            return Res;
        }

        public async Task<ApiResponse> Delete(ApiRequest<List<int?>> Req)
        {
            var Res = new ApiResponse();

            foreach (var Arg in Req.Args)
            {
                var user = _CashFlowDbContext.Users.FirstOrDefault(x => x.Id == Arg);
                if (user == null)
                {
                    Res.Success = false;
                    Res.Code = (int)ResponseStatusCode.CannotFind;
                    Res.Message = "無此用戶";
                }
                else
                {
                    _CashFlowDbContext.Users.Remove(user);
                    _CashFlowDbContext.SaveChanges();
                    Res.Success = true;
                    Res.Code = (int)ResponseStatusCode.Success;
                    Res.Message = "成功刪除";
                }
            }

            return Res;
        }

    }
}
