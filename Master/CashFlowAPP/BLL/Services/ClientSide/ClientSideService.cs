using BLL.IServices;
using Common.Enum;
using Common.Function;
using Common.Model;
using DPL.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.ClientSide
{
    public class ClientSideService:IClientSideService
    {
        private readonly CashFlowDbContext _CashFlowDbContext;
        public ClientSideService(CashFlowDbContext cashFlowDbContext)
        {
            _CashFlowDbContext = cashFlowDbContext;
        }

        public async Task<ApiResponse> UserSignUp(ApiRequest<UserSignUpDTO> Req)
        {
            var Res = new ApiResponse();
            var DeJWTcode = Jose.JWT.Decode(Req.Args.JWTcode, Encoding.UTF8.GetBytes("錢董"),Jose.JwsAlgorithm.HS256);
            if(Req.Args.ValidateCode!= DeJWTcode)
            {   
                Res.Success = false;
                Res.Code = (int)ResponseStatusCode.ValidateFail;
                Res.Message = "驗證碼錯誤";

                return Res;
            }
            var user = new User();
            user.Email = Req.Args.Email;
            user.Name = $@"社畜{Method.CreateValidateCode(4)}";
            user.Password = Req.Args.Password;//HashToDo
            user.RoleId = 1; //Todo
            user.Status = (byte)StatusCode.Enable;

            _CashFlowDbContext.Users.Add(user);
            _CashFlowDbContext.SaveChanges();

            Res.Success = true;
            Res.Code = (int)ResponseStatusCode.Success;
            Res.Message = "註冊成功";
            return Res;
        }
        public async Task<ApiResponse> UserLogin(ApiRequest<string> Req)
        {
            var Res = new ApiResponse();
            return Res;
        }
    }
}
