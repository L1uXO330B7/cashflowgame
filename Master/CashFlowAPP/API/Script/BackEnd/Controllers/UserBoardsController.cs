

        using Microsoft.AspNetCore.Mvc;
        using Common.Model;
        using BLL.IServices;
        using Common.Model.AdminSide;

        namespace API.Controllers.AdminSide
        {
           
            [Route("api//[controller]/[action]")]
            [ApiController]
            public class UserBoardsController : Controller,ICrudController<List<CreateUserBoardArgs>, List<ReadUserBoardArgs>, List<UpdateUserBoardArgs>, List<int?>>
            {
                private IUserBoardsService<List<CreateUserBoardArgs>, List<ReadUserBoardArgs>, List<UpdateUserBoardArgs>, List<int?>> _UserBoardsService;

                public UserBoardsController(
                    IUserBoardsService<List<CreateUserBoardArgs>, List<ReadUserBoardArgs>, List<UpdateUserBoardArgs>, List<int?>> IUserBoardsService
                )
                {
                    _UserBoardsService = IUserBoardsService;
                }

                [HttpPost]
                public async Task<ApiResponse> Create([FromBody] ApiRequest<List<CreateUserBoardArgs>> Req)
                {
                    return await _UserBoardsService.Create(Req);
                }

                [HttpPost]
                public async Task<ApiResponse> Delete([FromBody] ApiRequest<List<int?>> Req)
                {
                    return await _UserBoardsService.Delete(Req);
                }

                [HttpPost]
                public async Task<ApiResponse> Read([FromBody] ApiRequest<List<ReadUserBoardArgs>> Req)
                {
                    return await _UserBoardsService.Read(Req);
                }

                [HttpPost]
                public async Task<ApiResponse> Update([FromBody] ApiRequest<List<UpdateUserBoardArgs>> Req)
                {
                    return await _UserBoardsService.Update(Req);
                }
            }
        }
