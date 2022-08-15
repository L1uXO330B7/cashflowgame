using BLL.IServices;
using Common.Model;
using Common.Model.AdminSide;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.AdminSide
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class QuestionsController : Controller, ICrudController<List<CreateUserArgs>, List<ReadQuestionArgs>, List<UpdateUserArgs>, List<int?>>
    {
        private IQuestionsService<List<CreateUserArgs>, List<ReadQuestionArgs>, List<UpdateUserArgs>, List<int?>> _QuestionsService;

        /// <summary>
        /// 建構子
        /// </summary>
        /// <param name="IQuestionsService"></param>
        public QuestionsController(
            IQuestionsService<List<CreateUserArgs>, List<ReadQuestionArgs>, List<UpdateUserArgs>, List<int?>> IQuestionsService
        )
        {
            _QuestionsService = IQuestionsService;
        }


        [HttpPost]
        public Task<ApiResponse> Create([FromBody] ApiRequest<List<CreateUserArgs>> Req)
        {
            throw new NotImplementedException();
        }
        [HttpPost]
        public Task<ApiResponse> Delete([FromBody] ApiRequest<List<int?>> Req)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public async Task<ApiResponse> Read([FromBody] ApiRequest<List<ReadQuestionArgs>> Req)
        {
            return await _QuestionsService.Read(Req);
        }
        [HttpPost]
        public Task<ApiResponse> Update([FromBody] ApiRequest<List<UpdateUserArgs>> Req)
        {
            throw new NotImplementedException();
        }
    }
}
