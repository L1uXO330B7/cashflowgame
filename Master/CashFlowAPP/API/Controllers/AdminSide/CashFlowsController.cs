

using Microsoft.AspNetCore.Mvc;
using Common.Model;
using BLL.IServices;
using Common.Model.AdminSide;
using API.Module;

namespace API.Controllers.AdminSide
{

    [Route("api/[controller]/[action]")]
            [ApiController]
            public class CashFlowsController : Controller,ICrudController<List<CreateCashFlowArgs>, List<ReadCashFlowArgs>, List<UpdateCashFlowArgs>, List<int?>>
            {
                private ICashFlowsService<List<CreateCashFlowArgs>, List<ReadCashFlowArgs>, List<UpdateCashFlowArgs>, List<int?>> _CashFlowsService;

                public CashFlowsController(
                    ICashFlowsService<List<CreateCashFlowArgs>, List<ReadCashFlowArgs>, List<UpdateCashFlowArgs>, List<int?>> ICashFlowsService
                )
                {
                    _CashFlowsService = ICashFlowsService;
                }

                [HttpPost]
                public async Task<ApiResponse> Create([FromBody] ApiRequest<List<CreateCashFlowArgs>> Req)
                {
                    return await _CashFlowsService.Create(Req);
                }

                [HttpPost]
                public async Task<ApiResponse> Delete([FromBody] ApiRequest<List<int?>> Req)
                {
                    return await _CashFlowsService.Delete(Req);
                }

                [HttpPost]
                public async Task<ApiResponse> Read([FromBody] ApiRequest<List<ReadCashFlowArgs>> Req)
                {
                    return await _CashFlowsService.Read(Req);
                }

                [HttpPost]
                public async Task<ApiResponse> Update([FromBody] ApiRequest<List<UpdateCashFlowArgs>> Req)
                {
                    return await _CashFlowsService.Update(Req);
                }
            }
        }
