

        using Microsoft.AspNetCore.Mvc;
        using Common.Model;
        using BLL.IServices;
        using Common.Model.AdminSide;

        namespace API.Controllers.AdminSide
        {
           
            [Route("api/[controller]/[action]")]
            [ApiController]
            public class EffectTablesController : Controller,ICrudController<List<CreateEffectTableArgs>, List<ReadEffectTableArgs>, List<UpdateEffectTableArgs>, List<int?>>
            {
                private IEffectTablesService<List<CreateEffectTableArgs>, List<ReadEffectTableArgs>, List<UpdateEffectTableArgs>, List<int?>> _EffectTablesService;

                public EffectTablesController(
                    IEffectTablesService<List<CreateEffectTableArgs>, List<ReadEffectTableArgs>, List<UpdateEffectTableArgs>, List<int?>> IEffectTablesService
                )
                {
                    _EffectTablesService = IEffectTablesService;
                }

                [HttpPost]
                public async Task<ApiResponse> Create([FromBody] ApiRequest<List<CreateEffectTableArgs>> Req)
                {
                    return await _EffectTablesService.Create(Req);
                }

                [HttpPost]
                public async Task<ApiResponse> Delete([FromBody] ApiRequest<List<int?>> Req)
                {
                    return await _EffectTablesService.Delete(Req);
                }

                [HttpPost]
                public async Task<ApiResponse> Read([FromBody] ApiRequest<List<ReadEffectTableArgs>> Req)
                {
                    return await _EffectTablesService.Read(Req);
                }

                [HttpPost]
                public async Task<ApiResponse> Update([FromBody] ApiRequest<List<UpdateEffectTableArgs>> Req)
                {
                    return await _EffectTablesService.Update(Req);
                }
            }
        }
