using BLL.IServices;
using Common.Model;
using DPL.EF;
using Common.Enum;
using Common.Model.AdminSide;
using Newtonsoft.Json;

namespace BLL.Services.AdminSide
{
    public class UsersService : IUsersService<
            List<CreateUserArgs>,
            List<ReadUserArgs>,
            List<UpdateUserArgs>,
            List<int?>
        >
    {
        private readonly CashFlowDbContext _CashFlowDbContext;

        public UsersService(CashFlowDbContext cashFlowDbContext)
        {
            _CashFlowDbContext = cashFlowDbContext;
        }

        public async Task<ApiResponse> Create(ApiRequest<List<CreateUserArgs>> Req)
        {
            var users = new List<User>();

            var SussList = new List<int>();

            foreach (var Arg in Req.Args)
            {
                var user = new User();
                user.Email = Arg.Email;
                user.Password = Arg.Password;
                user.Name = Arg.Name;
                user.Status = Arg.Status;
                user.RoleId = Arg.RoleId;
                users.Add(user);
            }

            _CashFlowDbContext.AddRange(users);
            _CashFlowDbContext.SaveChanges();
            // 不做銷毀 Dispose 動作，交給 DI 容器處理

            // 此處 SaveChanges 後 SQL Server 會 Tracking 回傳新增後的 Id
            SussList = users.Select(x => x.Id).ToList();

            var Res = new ApiResponse();
            Res.Data = $@"已新增以下筆數(Id)：[{string.Join(',', SussList)}]";
            Res.Success = true;
            Res.Code = (int)ResponseStatusCode.Success;
            Res.Message = "成功新增";

            return Res;
        }

        public async Task<ApiResponse> Read(ApiRequest<List<ReadUserArgs>> Req)
        {
            var Roles = _CashFlowDbContext.Roles.AsQueryable();
            var Res = new ApiResponse();
            var users = _CashFlowDbContext
                .Users
                .Select(x => new UsersResponse
                {
                    Id = x.Id,
                    Email = x.Email,
                    Name = x.Name,
                    Status = x.Status,
                    Password = x.Password,
                    RoleId = x.RoleId,
                    RoleName = x.Role.Name,
                })
                .AsQueryable();

            foreach (var Arg in Req.Args)
            {
                if (Arg.Key == "Id") // Id 篩選條件
                {
                    var Ids = JsonConvert
                        .DeserializeObject<List<int>>(Arg.JsonString);

                    users = users.Where(x => Ids.Contains(x.Id));
                }

                if (Arg.Key == "Status") // 狀態篩選條件
                {
                    var Status = JsonConvert
                        .DeserializeObject<byte>(Arg.JsonString);

                    users = users.Where(x => x.Status == Status);
                }
            }

            var Users = users
                    // 後端分頁
                    // 省略幾筆 ( 頁數 * 每頁幾筆 )
                    .Skip(((int)Req.PageIndex - 1) * (int)Req.PageSize)
                    // 取得幾筆
                    .Take((int)Req.PageSize)
                    .ToList();

            var Data = new { Users, Roles = Roles.ToList() };

            Res.Data = Data;
            Res.Success = true;
            Res.Code = (int)ResponseStatusCode.Success;
            Res.Message = "成功讀取";
            Res.TotalDataCount = users.ToList().Count;

            return Res;
        }

        public async Task<ApiResponse> Update(ApiRequest<List<UpdateUserArgs>> Req)
        {
            var Res = new ApiResponse();

            var SussList = new List<int>();

            foreach (var Arg in Req.Args)
            {
                var user = _CashFlowDbContext.Users
                    .FirstOrDefault(x => x.Id == Arg.Id);

                if (user == null)
                {
                    Res.Success = false;
                    Res.Code = (int)ResponseStatusCode.CannotFind;
                    Res.Message += $@"Id：{Arg.Id} 無此Id\n";
                }
                else
                {
                    user.Email = Arg.Email;
                    user.Password = Arg.Password;
                    user.Name = Arg.Name;
                    user.Status = Arg.Status;
                    user.RoleId = Arg.RoleId;
                    _CashFlowDbContext.SaveChanges();
                    SussList.Add(user.Id);
                }
            }

            Res.Data = $@"SussList：[{string.Join(',', SussList)}]";
            Res.Success = true;
            Res.Code = (int)ResponseStatusCode.Success;
            Res.Message = "成功更改";

            return Res;
        }

        public async Task<ApiResponse> Delete(ApiRequest<List<int?>> Req)
        {
            var Res = new ApiResponse();

            var SussList = new List<int>();

            foreach (var Arg in Req.Args)
            {
                var user = _CashFlowDbContext.Users
                    .FirstOrDefault(x => x.Id == Arg);

                if (user == null)
                {
                    Res.Success = false;
                    Res.Code = (int)ResponseStatusCode.CannotFind;
                    Res.Message = "無此Id";
                }
                else
                {
                    _CashFlowDbContext.Users.Remove(user);
                    _CashFlowDbContext.SaveChanges();
                    SussList.Add(user.Id);
                }
            }

            Res.Data = $@"SussList：[{string.Join(',', SussList)}]"; ;
            Res.Success = true;
            Res.Code = (int)ResponseStatusCode.Success;
            Res.Message = "成功刪除";

            return Res;
        }

    }
}
