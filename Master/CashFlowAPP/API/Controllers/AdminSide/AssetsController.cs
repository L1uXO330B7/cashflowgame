

using Microsoft.AspNetCore.Mvc;
using Common.Model;
using BLL.IServices;
using Common.Model.AdminSide;
using API.Module;

namespace API.Controllers.AdminSide
{

    [Route("api/[controller]/[action]")]
            [ApiController]
            public class AssetsController : Controller,ICrudController<List<CreateAssetArgs>, List<ReadAssetArgs>, List<UpdateAssetArgs>, List<int?>>
            {
                private IAssetsService<List<CreateAssetArgs>, List<ReadAssetArgs>, List<UpdateAssetArgs>, List<int?>> _AssetsService;

                public AssetsController(
                    IAssetsService<List<CreateAssetArgs>, List<ReadAssetArgs>, List<UpdateAssetArgs>, List<int?>> IAssetsService
                )
                {
                    _AssetsService = IAssetsService;
                }

                [HttpPost]
                public async Task<ApiResponse> Create([FromBody] ApiRequest<List<CreateAssetArgs>> Req)
                {
                    return await _AssetsService.Create(Req);
                }

                [HttpPost]
                public async Task<ApiResponse> Delete([FromBody] ApiRequest<List<int?>> Req)
                {
                    return await _AssetsService.Delete(Req);
                }

                [HttpPost]
                public async Task<ApiResponse> Read([FromBody] ApiRequest<List<ReadAssetArgs>> Req)
                {
                    return await _AssetsService.Read(Req);
                }

                [HttpPost]
                public async Task<ApiResponse> Update([FromBody] ApiRequest<List<UpdateAssetArgs>> Req)
                {
                    return await _AssetsService.Update(Req);
                }
            }
        }
