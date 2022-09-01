

        using Microsoft.AspNetCore.Mvc;
        using Common.Model;
        using BLL.IServices;
        using Common.Model.AdminSide;

        namespace API.Controllers.AdminSide
        {
           
            [Route("api/[controller]/[action]")]
            [ApiController]
            public class CardEffectsController : Controller,ICrudController<List<CreateCardEffectArgs>, List<ReadCardEffectArgs>, List<UpdateCardEffectArgs>, List<int?>>
            {
                private ICardEffectsService<List<CreateCardEffectArgs>, List<ReadCardEffectArgs>, List<UpdateCardEffectArgs>, List<int?>> _CardEffectsService;

                public CardEffectsController(
                    ICardEffectsService<List<CreateCardEffectArgs>, List<ReadCardEffectArgs>, List<UpdateCardEffectArgs>, List<int?>> ICardEffectsService
                )
                {
                    _CardEffectsService = ICardEffectsService;
                }

                [HttpPost]
                public async Task<ApiResponse> Create([FromBody] ApiRequest<List<CreateCardEffectArgs>> Req)
                {
                    return await _CardEffectsService.Create(Req);
                }

                [HttpPost]
                public async Task<ApiResponse> Delete([FromBody] ApiRequest<List<int?>> Req)
                {
                    return await _CardEffectsService.Delete(Req);
                }

                [HttpPost]
                public async Task<ApiResponse> Read([FromBody] ApiRequest<List<ReadCardEffectArgs>> Req)
                {
                    return await _CardEffectsService.Read(Req);
                }

                [HttpPost]
                public async Task<ApiResponse> Update([FromBody] ApiRequest<List<UpdateCardEffectArgs>> Req)
                {
                    return await _CardEffectsService.Update(Req);
                }
            }
        }
