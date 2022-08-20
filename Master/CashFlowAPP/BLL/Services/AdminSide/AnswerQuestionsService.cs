

        using BLL.IServices;
        using Common.Model;
        using DPL.EF;
        using Common.Enum;
        using Common.Model.AdminSide;
        using Newtonsoft.Json;

        namespace BLL.Services.AdminSide
        {
            public class AnswerQuestionsService : IAnswerQuestionsService<
                    List<CreateAnswerQuestionArgs>,
                    List<ReadAnswerQuestionArgs>,
                    List<UpdateAnswerQuestionArgs>,
                    List<int?>
                >
            {
                    private readonly CashFlowDbContext _CashFlowDbContext;

                    public AnswerQuestionsService(CashFlowDbContext cashFlowDbContext)
                    {
                            _CashFlowDbContext = cashFlowDbContext;
                    }

                    public async Task<ApiResponse> Create(ApiRequest<List<CreateAnswerQuestionArgs>> Req)
                    {
                            var answerQuestions = new List<AnswerQuestion>();

                            var SussList = new List<int>();

                            foreach (var Arg in Req.Args)
                            {
                                var answerQuestion = new AnswerQuestion();        

                                answerQuestion.Id = Arg.Id;
answerQuestion.Answer = Arg.Answer;
answerQuestion.QusetionId = Arg.QusetionId;
answerQuestion.UserId = Arg.UserId;


                                answerQuestions.Add(answerQuestion);
                            }

                            _CashFlowDbContext.AddRange(answerQuestions);
                            _CashFlowDbContext.SaveChanges();
                            // 不做銷毀 Dispose 動作，交給 DI 容器處理

                            // 此處 SaveChanges 後 SQL Server 會 Tracking 回傳新增後的 Id
                            SussList = answerQuestions.Select(x => x.Id).ToList();

                            var Res = new ApiResponse();
                            Res.Data = $@"已新增以下筆數(Id)：[{string.Join(',', SussList)}]";
                            Res.Success = true;
                            Res.Code = (int) ResponseStatusCode.Success;
                            Res.Message = "成功新增";

                            return Res;
                    }

                    public async Task<ApiResponse> Read(ApiRequest<List<ReadAnswerQuestionArgs>> Req)
                    {
                            var Res = new ApiResponse();
                            var answerQuestions = _CashFlowDbContext.AnswerQuestions.AsQueryable();

                            foreach (var Arg in Req.Args)
                            {
                                if (Arg.Key == "Id") // Id 篩選條件
                                {
                                    var Ids = JsonConvert
                                            .DeserializeObject<List<int>>(Arg.JsonString);

                                    answerQuestions = answerQuestions.Where(x => Ids.Contains(x.Id));
                                }
                            }

                            var Data = answerQuestions
                            // 後端分頁
                            // 省略幾筆 ( 頁數 * 每頁幾筆 )
                            .Skip(((int)Req.PageIndex -1) * (int)Req.PageSize)
                            // 取得幾筆，
                            .Take((int)Req.PageSize)
                            .ToList();

                            Res.Data = Data;
                            Res.Success = true;
                            Res.Code = (int)ResponseStatusCode.Success;
                            Res.Message = "成功讀取";
                            Res.TotalDataCount = answerQuestions.ToList().Count;

                            return Res;
                    }

                    public async Task<ApiResponse> Update(ApiRequest<List<UpdateAnswerQuestionArgs>> Req)
                    {
                            var Res = new ApiResponse();

                            var SussList = new List<int>();

                            foreach (var Arg in Req.Args)
                            {
                                var answerQuestion = _CashFlowDbContext.AnswerQuestions
                                         .FirstOrDefault(x => x.Id == Arg.Id);

                                if (answerQuestion == null)
                                {
                                    Res.Success = false;
                                    Res.Code = (int)ResponseStatusCode.CannotFind;
                                    Res.Message += $@"Id：{Arg.Id} 無此Id\n";
                                }
                                else
                                {
                                    answerQuestion.Id = Arg.Id;
answerQuestion.Answer = Arg.Answer;
answerQuestion.QusetionId = Arg.QusetionId;
answerQuestion.UserId = Arg.UserId;
                                    

                                    _CashFlowDbContext.SaveChanges();
                                    SussList.Add(answerQuestion.Id);
                                }
                            }

                            Res.Data = $@"SussList：[{string.Join(',', SussList)}]";
                            Res.Success = true;
                            Res.Code = (int)ResponseStatusCode.Success;
                            Res.Message = "成功更改";

                            return Res;
                    }

                    public async Task<ApiResponse> Delete(ApiRequest<List<int?>> Req)
                    {
                            var Res = new ApiResponse();

                            var SussList = new List<int>();

                            foreach (var Arg in Req.Args)
                            {
                                var answerQuestion = _CashFlowDbContext.AnswerQuestions
                                         .FirstOrDefault(x => x.Id == Arg);

                                if (answerQuestion == null)
                                {
                                    Res.Success = false;
                                    Res.Code = (int)ResponseStatusCode.CannotFind;
                                    Res.Message = "無此Id";
                                }
                                else
                                {
                                    _CashFlowDbContext.AnswerQuestions.Remove(answerQuestion);
                                    _CashFlowDbContext.SaveChanges();
                                    SussList.Add(answerQuestion.Id);
                                }
                            }

                            Res.Data = $@"SussList：[{string.Join(',', SussList)}]";
                            Res.Success = true;
                            Res.Code = (int)ResponseStatusCode.Success;
                            Res.Message = "成功刪除";

                            return Res;
                    }

            }
        }

