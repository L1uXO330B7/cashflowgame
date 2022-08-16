

        using Microsoft.AspNetCore.Mvc;
        using Common.Model;
        using BLL.IServices;
        using Common.Model.AdminSide;

        namespace API.Controllers.AdminSide
        {
           
            [Route("api//[controller]/[action]")]
            [ApiController]
            public class QustionEffectsController : Controller,ICrudController<List<CreateQustionEffectArgs>, List<ReadQustionEffectArgs>, List<UpdateQustionEffectArgs>, List<int?>>
            {
                private IQustionEffectsService<List<CreateQustionEffectArgs>, List<ReadQustionEffectArgs>, List<UpdateQustionEffectArgs>, List<int?>> _QustionEffectsService;

                public QustionEffectsController(
                    IQustionEffectsService<List<CreateQustionEffectArgs>, List<ReadQustionEffectArgs>, List<UpdateQustionEffectArgs>, List<int?>> IQustionEffectsService
                )
                {
                    _QustionEffectsService = IQustionEffectsService;
                }

                [HttpPost]
                public async Task<ApiResponse> Create([FromBody] ApiRequest<List<CreateQustionEffectArgs>> Req)
                {
                    return await _QustionEffectsService.Create(Req);
                }

                [HttpPost]
                public async Task<ApiResponse> Delete([FromBody] ApiRequest<List<int?>> Req)
                {
                    return await _QustionEffectsService.Delete(Req);
                }

                [HttpPost]
                public async Task<ApiResponse> Read([FromBody] ApiRequest<List<ReadQustionEffectArgs>> Req)
                {
                    return await _QustionEffectsService.Read(Req);
                }

                [HttpPost]
                public async Task<ApiResponse> Update([FromBody] ApiRequest<List<UpdateQustionEffectArgs>> Req)
                {
                    return await _QustionEffectsService.Update(Req);
                }
            }
        }
