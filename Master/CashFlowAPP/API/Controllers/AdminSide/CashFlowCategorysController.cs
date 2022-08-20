

        using Microsoft.AspNetCore.Mvc;
        using Common.Model;
        using BLL.IServices;
        using Common.Model.AdminSide;

        namespace API.Controllers.AdminSide
        {
           
            [Route("api/[controller]/[action]")]
            [ApiController]
            public class CashFlowCategorysController : Controller,ICrudController<List<CreateCashFlowCategoryArgs>, List<ReadCashFlowCategoryArgs>, List<UpdateCashFlowCategoryArgs>, List<int?>>
            {
                private ICashFlowCategorysService<List<CreateCashFlowCategoryArgs>, List<ReadCashFlowCategoryArgs>, List<UpdateCashFlowCategoryArgs>, List<int?>> _CashFlowCategorysService;

                public CashFlowCategorysController(
                    ICashFlowCategorysService<List<CreateCashFlowCategoryArgs>, List<ReadCashFlowCategoryArgs>, List<UpdateCashFlowCategoryArgs>, List<int?>> ICashFlowCategorysService
                )
                {
                    _CashFlowCategorysService = ICashFlowCategorysService;
                }

                [HttpPost]
                public async Task<ApiResponse> Create([FromBody] ApiRequest<List<CreateCashFlowCategoryArgs>> Req)
                {
                    return await _CashFlowCategorysService.Create(Req);
                }

                [HttpPost]
                public async Task<ApiResponse> Delete([FromBody] ApiRequest<List<int?>> Req)
                {
                    return await _CashFlowCategorysService.Delete(Req);
                }

                [HttpPost]
                public async Task<ApiResponse> Read([FromBody] ApiRequest<List<ReadCashFlowCategoryArgs>> Req)
                {
                    return await _CashFlowCategorysService.Read(Req);
                }

                [HttpPost]
                public async Task<ApiResponse> Update([FromBody] ApiRequest<List<UpdateCashFlowCategoryArgs>> Req)
                {
                    return await _CashFlowCategorysService.Update(Req);
                }
            }
        }
