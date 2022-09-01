

        using Microsoft.AspNetCore.Mvc;
        using Common.Model;
        using BLL.IServices;
        using Common.Model.AdminSide;

        namespace API.Controllers.AdminSide
        {
           
            [Route("api/[controller]/[action]")]
            [ApiController]
            public class AnswerQuestionsController : Controller,ICrudController<List<CreateAnswerQuestionArgs>, List<ReadAnswerQuestionArgs>, List<UpdateAnswerQuestionArgs>, List<int?>>
            {
                private IAnswerQuestionsService<List<CreateAnswerQuestionArgs>, List<ReadAnswerQuestionArgs>, List<UpdateAnswerQuestionArgs>, List<int?>> _AnswerQuestionsService;

                public AnswerQuestionsController(
                    IAnswerQuestionsService<List<CreateAnswerQuestionArgs>, List<ReadAnswerQuestionArgs>, List<UpdateAnswerQuestionArgs>, List<int?>> IAnswerQuestionsService
                )
                {
                    _AnswerQuestionsService = IAnswerQuestionsService;
                }

                [HttpPost]
                public async Task<ApiResponse> Create([FromBody] ApiRequest<List<CreateAnswerQuestionArgs>> Req)
                {
                    return await _AnswerQuestionsService.Create(Req);
                }

                [HttpPost]
                public async Task<ApiResponse> Delete([FromBody] ApiRequest<List<int?>> Req)
                {
                    return await _AnswerQuestionsService.Delete(Req);
                }

                [HttpPost]
                public async Task<ApiResponse> Read([FromBody] ApiRequest<List<ReadAnswerQuestionArgs>> Req)
                {
                    return await _AnswerQuestionsService.Read(Req);
                }

                [HttpPost]
                public async Task<ApiResponse> Update([FromBody] ApiRequest<List<UpdateAnswerQuestionArgs>> Req)
                {
                    return await _AnswerQuestionsService.Update(Req);
                }
            }
        }
