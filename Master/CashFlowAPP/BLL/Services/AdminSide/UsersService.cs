using BLL.IServices;
using Common.Model;
using DPL.EF;
using Common.Enum;


namespace BLL.Services.AdminSide
{
    public class UsersService : ServiceBase, IUsersService<CreateUserArgs, int?, UpdateUserArgs, int?>
    {
        public UsersService(CashFlowDbContext _CashFlowDbContext) : base(_CashFlowDbContext)
        {

        }

        public async Task<ApiResponse> Create(ApiRequest<CreateUserArgs> Req)
        {
            User User = new User();
            User.Email = Req.Args.Email;
            User.Password = Req.Args.Password;
            User.Name = Req.Args.Name;
            User.Status = Req.Args.Status;
            User.RoleId = Req.Args.RoleId;
            _CashFlowDbContext.Add(User);
            _CashFlowDbContext.SaveChanges();//不做銷毀dispose動作，交給 DI 容器處理

            var Res = new ApiResponse();
            Res.Success = true;
            Res.Code = (int)ResponseStatusCode.Success;
            Res.Message = "成功新增";

            return Res;
        }
        public async Task<ApiResponse> Read(ApiRequest<int?> Req)
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
                var User = _CashFlowDbContext.Users.Find(Req.Args);
                if (User == null)
                {
                    Res.Success = false;
                    Res.Code = (int)ResponseStatusCode.CannotFind;
                    Res.Message = "無此用戶";
                }
                else
                {
                    Res.Data = User;
                    Res.Success = true;
                    Res.Code = (int)ResponseStatusCode.Success;
                    Res.Message = "成功讀取";
                }
            }
            return Res;
        }
        public async Task<ApiResponse> Update(ApiRequest<UpdateUserArgs> Req)
        {
            var Res = new ApiResponse();

            try
            {
                var User = _CashFlowDbContext.Users.Find(Req.Args.Id);
                if (User == null)
                {
                    Res.Success = false;
                    Res.Code = (int)ResponseStatusCode.CannotFind;
                    Res.Message = "無此用戶";
                }
                else
                {
                    User.Email = Req.Args.Email;
                    User.Password = Req.Args.Password;
                    User.Name = Req.Args.Name;
                    User.Status = Req.Args.Status;
                    User.RoleId = Req.Args.RoleId;
                    _CashFlowDbContext.SaveChanges();

                    Res.Data = User;
                    Res.Success = true;
                    Res.Code = (int)ResponseStatusCode.Success;
                    Res.Message = "成功更改";
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

        public async Task<ApiResponse> Delete(ApiRequest<int?> Req)
        {
            var Res = new ApiResponse();

            var user = _CashFlowDbContext.Users.Find(Req.Args);
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

            return Res;

        }
    }
}
