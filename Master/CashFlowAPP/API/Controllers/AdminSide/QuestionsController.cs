

using Microsoft.AspNetCore.Mvc;
using Common.Model;
using BLL.IServices;
using Common.Model.AdminSide;
using API.Module;

namespace API.Controllers.AdminSide
{

    [Route("api/[controller]/[action]")]
            [ApiController]
            public class QuestionsController : Controller,ICrudController<List<CreateQuestionArgs>, List<ReadQuestionArgs>, List<UpdateQuestionArgs>, List<int?>>
            {
                private IQuestionsService<List<CreateQuestionArgs>, List<ReadQuestionArgs>, List<UpdateQuestionArgs>, List<int?>> _QuestionsService;

                public QuestionsController(
                    IQuestionsService<List<CreateQuestionArgs>, List<ReadQuestionArgs>, List<UpdateQuestionArgs>, List<int?>> IQuestionsService
                )
                {
                    _QuestionsService = IQuestionsService;
                }

                [HttpPost]
                public async Task<ApiResponse> Create([FromBody] ApiRequest<List<CreateQuestionArgs>> Req)
                {
                    return await _QuestionsService.Create(Req);
                }

                [HttpPost]
                public async Task<ApiResponse> Delete([FromBody] ApiRequest<List<int?>> Req)
                {
                    return await _QuestionsService.Delete(Req);
                }

                [HttpPost]
                public async Task<ApiResponse> Read([FromBody] ApiRequest<List<ReadQuestionArgs>> Req)
                {
                    return await _QuestionsService.Read(Req);
                }

                [HttpPost]
                public async Task<ApiResponse> Update([FromBody] ApiRequest<List<UpdateQuestionArgs>> Req)
                {
                    return await _QuestionsService.Update(Req);
                }
            }
        }
