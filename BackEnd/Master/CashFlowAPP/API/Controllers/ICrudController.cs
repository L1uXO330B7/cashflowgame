using Common.Model;
using Microsoft.AspNetCore.Mvc;

namespace API.IController
{
    public interface ICrudController<CreateArgs, ReadArgs, UpdateArgs, DeleteArgs>
    {
        [HttpPost]
        Task<ApiResponse> Create([FromBody] ApiRequest<CreateArgs> Args);
        [HttpPost]
        Task<ApiResponse> Read([FromBody] ApiRequest<ReadArgs> Args);
        [HttpPost]
        Task<ApiResponse> Update([FromBody] ApiRequest<UpdateArgs> Args);
        [HttpPost]
        Task<ApiResponse> Delete([FromBody] ApiRequest<DeleteArgs> Args);
    }
}
