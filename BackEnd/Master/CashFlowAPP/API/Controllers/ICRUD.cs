using Common.Model;
using Microsoft.AspNetCore.Mvc;

namespace API.IController
{
    public interface ICRUD<T>
    {
        [HttpPost]
        Task<ApiResponse> Create([FromBody] ApiRequest<T> Args);
        [HttpPost]
        Task<ApiResponse> Read([FromBody] ApiRequest<T> Args);
        [HttpPost]
        Task<ApiResponse> Update([FromBody] ApiRequest<T> Args);
        [HttpPost]
        Task<ApiResponse> Delete([FromBody] ApiRequest<T> Args);
    }
}
