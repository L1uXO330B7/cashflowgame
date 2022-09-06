

using Microsoft.AspNetCore.Mvc;
using Common.Model;
using BLL.IServices;
using Common.Model.AdminSide;
using API.Module;

namespace API.Controllers.AdminSide
{

    [Route("api/[controller]/[action]")]
            [ApiController]
            public class RolesController : Controller,ICrudController<List<CreateRoleArgs>, List<ReadRoleArgs>, List<UpdateRoleArgs>, List<int?>>
            {
                private IRolesService<List<CreateRoleArgs>, List<ReadRoleArgs>, List<UpdateRoleArgs>, List<int?>> _RolesService;

                public RolesController(
                    IRolesService<List<CreateRoleArgs>, List<ReadRoleArgs>, List<UpdateRoleArgs>, List<int?>> IRolesService
                )
                {
                    _RolesService = IRolesService;
                }

                [HttpPost]
                public async Task<ApiResponse> Create([FromBody] ApiRequest<List<CreateRoleArgs>> Req)
                {
                    return await _RolesService.Create(Req);
                }

                [HttpPost]
                public async Task<ApiResponse> Delete([FromBody] ApiRequest<List<int?>> Req)
                {
                    return await _RolesService.Delete(Req);
                }

                [HttpPost]
                public async Task<ApiResponse> Read([FromBody] ApiRequest<List<ReadRoleArgs>> Req)
                {
                    return await _RolesService.Read(Req);
                }

                [HttpPost]
                public async Task<ApiResponse> Update([FromBody] ApiRequest<List<UpdateRoleArgs>> Req)
                {
                    return await _RolesService.Update(Req);
                }
            }
        }
