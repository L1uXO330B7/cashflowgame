using Common.Model;
using Microsoft.AspNetCore.Mvc;

namespace API
{
    public interface ICrudController<CreateArgs, ReadArgs, UpdateArgs, DeleteArgs>
    {
        [HttpPost]
        Task<ApiResponse> Create([FromBody] ApiRequest<CreateArgs> Req);
        [HttpPost]
        Task<ApiResponse> Read([FromBody] ApiRequest<ReadArgs> Req);
        [HttpPost]
        Task<ApiResponse> Update([FromBody] ApiRequest<UpdateArgs> Req);
        [HttpPost]
        Task<ApiResponse> Delete([FromBody] ApiRequest<DeleteArgs> Req);
    }
}
