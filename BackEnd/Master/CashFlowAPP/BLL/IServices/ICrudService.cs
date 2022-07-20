using Common.Model;

namespace BLL.IServices
{
    public interface ICrudService<CreateArgs, ReadArgs, UpdateArgs, DeleteArgs>
    {
        Task<ApiResponse> Create(ApiRequest<CreateArgs> Req);
        Task<ApiResponse> Read(ApiRequest<ReadArgs> Req);
        Task<ApiResponse> Update(ApiRequest<UpdateArgs> Req);
        Task<ApiResponse> Delete(ApiRequest<DeleteArgs> Req);
    }
}
