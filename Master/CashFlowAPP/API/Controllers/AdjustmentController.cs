using BLL.IServices;
using Common.Model;
using DPL.EF;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    /// <summary>
    /// 調試用
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AdjustmentController : Controller
    {
        private IClientSideService _ClientSideService;
        public AdjustmentController(IClientSideService ClientSideService)
        {
            _ClientSideService = ClientSideService;
        }

        [HttpPost]
        public async Task<ApiResponse> ReadFiInfo([FromBody] ApiRequest<int?> Req)
        {
            Req.Args = null;
            return await _ClientSideService.ReadFiInfo(Req);
        }
    }
}
