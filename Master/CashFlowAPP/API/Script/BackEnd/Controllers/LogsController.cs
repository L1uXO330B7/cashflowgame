

        using Microsoft.AspNetCore.Mvc;
        using Common.Model;
        using BLL.IServices;
        using Common.Model.AdminSide;

        namespace API.Controllers.AdminSide
        {
           
            [Route("api//[controller]/[action]")]
            [ApiController]
            public class LogsController : Controller,ICrudController<List<CreateLogArgs>, List<ReadLogArgs>, List<UpdateLogArgs>, List<int?>>
            {
                private ILogsService<List<CreateLogArgs>, List<ReadLogArgs>, List<UpdateLogArgs>, List<int?>> _LogsService;

                public LogsController(
                    ILogsService<List<CreateLogArgs>, List<ReadLogArgs>, List<UpdateLogArgs>, List<int?>> ILogsService
                )
                {
                    _LogsService = ILogsService;
                }

                [HttpPost]
                public async Task<ApiResponse> Create([FromBody] ApiRequest<List<CreateLogArgs>> Req)
                {
                    return await _LogsService.Create(Req);
                }

                [HttpPost]
                public async Task<ApiResponse> Delete([FromBody] ApiRequest<List<int?>> Req)
                {
                    return await _LogsService.Delete(Req);
                }

                [HttpPost]
                public async Task<ApiResponse> Read([FromBody] ApiRequest<List<ReadLogArgs>> Req)
                {
                    return await _LogsService.Read(Req);
                }

                [HttpPost]
                public async Task<ApiResponse> Update([FromBody] ApiRequest<List<UpdateLogArgs>> Req)
                {
                    return await _LogsService.Update(Req);
                }
            }
        }
