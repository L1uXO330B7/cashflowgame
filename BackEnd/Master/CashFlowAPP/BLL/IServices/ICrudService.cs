using Common.Model;

namespace BLL.IServices
{
    public interface ICrudService<CreateArgs, ReadArgs, UpdateArgs, DeleteArgs>
    {
        Task<ApiResponse> Create(ApiRequest<CreateArgs> Args);
        Task<ApiResponse> Read(ApiRequest<ReadArgs> Args);
        Task<ApiResponse> Update(ApiRequest<UpdateArgs> Args);
        Task<ApiResponse> Delete(ApiRequest<DeleteArgs> Args);
    }
}
