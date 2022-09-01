

        using Microsoft.AspNetCore.Mvc;
        using Common.Model;
        using BLL.IServices;
        using Common.Model.AdminSide;

        namespace API.Controllers.AdminSide
        {
           
            [Route("api/[controller]/[action]")]
            [ApiController]
            public class AssetCategorysController : Controller,ICrudController<List<CreateAssetCategoryArgs>, List<ReadAssetCategoryArgs>, List<UpdateAssetCategoryArgs>, List<int?>>
            {
                private IAssetCategorysService<List<CreateAssetCategoryArgs>, List<ReadAssetCategoryArgs>, List<UpdateAssetCategoryArgs>, List<int?>> _AssetCategorysService;

                public AssetCategorysController(
                    IAssetCategorysService<List<CreateAssetCategoryArgs>, List<ReadAssetCategoryArgs>, List<UpdateAssetCategoryArgs>, List<int?>> IAssetCategorysService
                )
                {
                    _AssetCategorysService = IAssetCategorysService;
                }

                [HttpPost]
                public async Task<ApiResponse> Create([FromBody] ApiRequest<List<CreateAssetCategoryArgs>> Req)
                {
                    return await _AssetCategorysService.Create(Req);
                }

                [HttpPost]
                public async Task<ApiResponse> Delete([FromBody] ApiRequest<List<int?>> Req)
                {
                    return await _AssetCategorysService.Delete(Req);
                }

                [HttpPost]
                public async Task<ApiResponse> Read([FromBody] ApiRequest<List<ReadAssetCategoryArgs>> Req)
                {
                    return await _AssetCategorysService.Read(Req);
                }

                [HttpPost]
                public async Task<ApiResponse> Update([FromBody] ApiRequest<List<UpdateAssetCategoryArgs>> Req)
                {
                    return await _AssetCategorysService.Update(Req);
                }
            }
        }
