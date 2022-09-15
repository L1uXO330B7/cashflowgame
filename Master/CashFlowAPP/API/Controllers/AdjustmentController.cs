using BLL.IServices;
using BLL.Services;
using Common.Model;
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

        //[HttpPost]
        //public async Task<ApiResponse> ReadFiInfo([FromBody] ApiRequest<int?> Req)
        //{
        //    Req.Args = null;
        //    return await _ClientSideService.ReadFiInfo(Req);
        //}

        /// <summary>
        /// 卡片開發輔助
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<ApiResponse> SupportCardDev()
        {
            return await _ClientSideService.SupportCardDev();
        }

        /// <summary>
        /// Sql Server 目前連線明細
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ApiResponse GetSqlServerConnectionDetail()
        {
            return new ServiceBase().GetSqlServerConnectionDetail();
        }
    }
}
