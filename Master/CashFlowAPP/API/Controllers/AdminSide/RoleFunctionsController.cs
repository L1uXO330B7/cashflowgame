

        using Microsoft.AspNetCore.Mvc;
        using Common.Model;
        using BLL.IServices;
        using Common.Model.AdminSide;

        namespace API.Controllers.AdminSide
        {
           
            [Route("api/[controller]/[action]")]
            [ApiController]
            public class RoleFunctionsController : Controller,ICrudController<List<CreateRoleFunctionArgs>, List<ReadRoleFunctionArgs>, List<UpdateRoleFunctionArgs>, List<int?>>
            {
                private IRoleFunctionsService<List<CreateRoleFunctionArgs>, List<ReadRoleFunctionArgs>, List<UpdateRoleFunctionArgs>, List<int?>> _RoleFunctionsService;

                public RoleFunctionsController(
                    IRoleFunctionsService<List<CreateRoleFunctionArgs>, List<ReadRoleFunctionArgs>, List<UpdateRoleFunctionArgs>, List<int?>> IRoleFunctionsService
                )
                {
                    _RoleFunctionsService = IRoleFunctionsService;
                }

                [HttpPost]
                public async Task<ApiResponse> Create([FromBody] ApiRequest<List<CreateRoleFunctionArgs>> Req)
                {
                    return await _RoleFunctionsService.Create(Req);
                }

                [HttpPost]
                public async Task<ApiResponse> Delete([FromBody] ApiRequest<List<int?>> Req)
                {
                    return await _RoleFunctionsService.Delete(Req);
                }

                [HttpPost]
                public async Task<ApiResponse> Read([FromBody] ApiRequest<List<ReadRoleFunctionArgs>> Req)
                {
                    return await _RoleFunctionsService.Read(Req);
                }

                [HttpPost]
                public async Task<ApiResponse> Update([FromBody] ApiRequest<List<UpdateRoleFunctionArgs>> Req)
                {
                    return await _RoleFunctionsService.Update(Req);
                }
            }
        }
