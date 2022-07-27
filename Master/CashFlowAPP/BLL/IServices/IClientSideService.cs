using Common.Model;
using static Common.Model.ClientSideModel;

namespace BLL.IServices
{
    public interface IClientSideService
    {
        Task<ApiResponse> UserSignUp(ApiRequest<UserSignUpDTO> Req);
        Task<ApiResponse> UserLogin(ApiRequest<ClientUserLogin> Req);
        Task<ApiResponse> GetJwtValidateCode();

    }
}
