using Common.Model;
using Common.Model.AdminSide;

namespace BLL.IServices
{
    public interface IClientSideService
    {
        Task<ApiResponse> UserSignUp(ApiRequest<UserSignUpDto> Req);
        Task<ApiResponse> UserLogin(ApiRequest<ClientUserLogin> Req);
        Task<ApiResponse> GetJwtValidateCode();
        Task<ApiResponse> UserAnswersUpdate(ApiRequest<List<CreateAnswerQuestionArgs>> Req);
        Task<ApiResponse> ReadFiInfo(ApiRequest<int?> Req);
    }
}
