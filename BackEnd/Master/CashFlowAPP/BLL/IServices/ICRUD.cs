using Common.Model;

namespace BLL.IServices
{
    public interface ICRUD
    {
        Task<ApiResponse> Create<T>(ApiRequest<T> Args);
        Task<ApiResponse> Read<T>(ApiRequest<T> Args);
        Task<ApiResponse> Update<T>(ApiRequest<T> Args);
        Task<ApiResponse> Delete<T>(ApiRequest<T> Args);
    }
}
