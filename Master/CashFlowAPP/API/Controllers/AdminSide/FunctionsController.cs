

using Microsoft.AspNetCore.Mvc;
using Common.Model;
using BLL.IServices;
using Common.Model.AdminSide;
using API.Module;

namespace API.Controllers.AdminSide
{

    [Route("api/[controller]/[action]")]
            [ApiController]
            public class FunctionsController : Controller,ICrudController<List<CreateFunctionArgs>, List<ReadFunctionArgs>, List<UpdateFunctionArgs>, List<int?>>
            {
                private IFunctionsService<List<CreateFunctionArgs>, List<ReadFunctionArgs>, List<UpdateFunctionArgs>, List<int?>> _FunctionsService;

                public FunctionsController(
                    IFunctionsService<List<CreateFunctionArgs>, List<ReadFunctionArgs>, List<UpdateFunctionArgs>, List<int?>> IFunctionsService
                )
                {
                    _FunctionsService = IFunctionsService;
                }

                [HttpPost]
                public async Task<ApiResponse> Create([FromBody] ApiRequest<List<CreateFunctionArgs>> Req)
                {
                    return await _FunctionsService.Create(Req);
                }

                [HttpPost]
                public async Task<ApiResponse> Delete([FromBody] ApiRequest<List<int?>> Req)
                {
                    return await _FunctionsService.Delete(Req);
                }

                [HttpPost]
                public async Task<ApiResponse> Read([FromBody] ApiRequest<List<ReadFunctionArgs>> Req)
                {
                    return await _FunctionsService.Read(Req);
                }

                [HttpPost]
                public async Task<ApiResponse> Update([FromBody] ApiRequest<List<UpdateFunctionArgs>> Req)
                {
                    return await _FunctionsService.Update(Req);
                }
            }
        }
