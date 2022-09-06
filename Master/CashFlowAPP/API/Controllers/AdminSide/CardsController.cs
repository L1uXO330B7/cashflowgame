

using Microsoft.AspNetCore.Mvc;
using Common.Model;
using BLL.IServices;
using Common.Model.AdminSide;
using API.Module;

namespace API.Controllers.AdminSide
{

    [Route("api/[controller]/[action]")]
            [ApiController]
            public class CardsController : Controller,ICrudController<List<CreateCardArgs>, List<ReadCardArgs>, List<UpdateCardArgs>, List<int?>>
            {
                private ICardsService<List<CreateCardArgs>, List<ReadCardArgs>, List<UpdateCardArgs>, List<int?>> _CardsService;

                public CardsController(
                    ICardsService<List<CreateCardArgs>, List<ReadCardArgs>, List<UpdateCardArgs>, List<int?>> ICardsService
                )
                {
                    _CardsService = ICardsService;
                }

                [HttpPost]
                public async Task<ApiResponse> Create([FromBody] ApiRequest<List<CreateCardArgs>> Req)
                {
                    return await _CardsService.Create(Req);
                }

                [HttpPost]
                public async Task<ApiResponse> Delete([FromBody] ApiRequest<List<int?>> Req)
                {
                    return await _CardsService.Delete(Req);
                }

                [HttpPost]
                public async Task<ApiResponse> Read([FromBody] ApiRequest<List<ReadCardArgs>> Req)
                {
                    return await _CardsService.Read(Req);
                }

                [HttpPost]
                public async Task<ApiResponse> Update([FromBody] ApiRequest<List<UpdateCardArgs>> Req)
                {
                    return await _CardsService.Update(Req);
                }
            }
        }
