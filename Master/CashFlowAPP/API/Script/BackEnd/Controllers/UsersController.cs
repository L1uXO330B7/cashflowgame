

        using Microsoft.AspNetCore.Mvc;
        using Common.Model;
        using BLL.IServices;
        using Common.Model.AdminSide;

        namespace API.Controllers.AdminSide
        {
           
            [Route("api//[controller]/[action]")]
            [ApiController]
            public class UsersController : Controller,ICrudController<List<CreateUserArgs>, List<ReadUserArgs>, List<UpdateUserArgs>, List<int?>>
            {
                private IUsersService<List<CreateUserArgs>, List<ReadUserArgs>, List<UpdateUserArgs>, List<int?>> _UsersService;

                public UsersController(
                    IUsersService<List<CreateUserArgs>, List<ReadUserArgs>, List<UpdateUserArgs>, List<int?>> IUsersService
                )
                {
                    _UsersService = IUsersService;
                }

                [HttpPost]
                public async Task<ApiResponse> Create([FromBody] ApiRequest<List<CreateUserArgs>> Req)
                {
                    return await _UsersService.Create(Req);
                }

                [HttpPost]
                public async Task<ApiResponse> Delete([FromBody] ApiRequest<List<int?>> Req)
                {
                    return await _UsersService.Delete(Req);
                }

                [HttpPost]
                public async Task<ApiResponse> Read([FromBody] ApiRequest<List<ReadUserArgs>> Req)
                {
                    return await _UsersService.Read(Req);
                }

                [HttpPost]
                public async Task<ApiResponse> Update([FromBody] ApiRequest<List<UpdateUserArgs>> Req)
                {
                    return await _UsersService.Update(Req);
                }
            }
        }
