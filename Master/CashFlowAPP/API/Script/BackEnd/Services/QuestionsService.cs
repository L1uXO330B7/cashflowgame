

        using BLL.IServices;
        using Common.Model;
        using DPL.EF;
        using Common.Enum;
        using Common.Model.AdminSide;
        using Newtonsoft.Json;

        namespace BLL.Services.AdminSide
        {
            public class QuestionsService : IQuestionsService<
                    List<CreateQuestionArgs>,
                    List<ReadQuestionArgs>,
                    List<UpdateQuestionArgs>,
                    List<int?>
                >
            {
                    private readonly CashFlowDbContext _CashFlowDbContext;

                    public QuestionsService(CashFlowDbContext cashFlowDbContext)
                    {
                            _CashFlowDbContext = cashFlowDbContext;
                    }

                    public async Task<ApiResponse> Create(ApiRequest<List<CreateQuestionArgs>> Req)
                    {
                            var questions = new List<Question>();

                            var SussList = new List<int>();

                            foreach (var Arg in Req.Args)
                            {
                                var question = new Question();        

                                question.Id = Arg.Id;
question.Type = Arg.Type;
question.Name = Arg.Name;
question.Answer = Arg.Answer;
question.Status = Arg.Status;


                                questions.Add(question);
                            }

                            _CashFlowDbContext.AddRange(questions);
                            _CashFlowDbContext.SaveChanges();
                            // 不做銷毀 Dispose 動作，交給 DI 容器處理

                            // 此處 SaveChanges 後 SQL Server 會 Tracking 回傳新增後的 Id
                            SussList = questions.Select(x => x.Id).ToList();

                            var Res = new ApiResponse();
                            Res.Data = $@"已新增以下筆數(Id)：[{string.Join(',', SussList)}]";
                            Res.Success = true;
                            Res.Code = (int) ResponseStatusCode.Success;
                            Res.Message = "成功新增";

                            return Res;
                    }

                    public async Task<ApiResponse> Read(ApiRequest<List<ReadQuestionArgs>> Req)
                    {
                            var Res = new ApiResponse();
                            var questions = _CashFlowDbContext.Questions.AsQueryable();

                            foreach (var Arg in Req.Args)
                            {
                                if (Arg.Key == "Id") // Id 篩選條件
                                {
                                    var Ids = JsonConvert
                                            .DeserializeObject<List<int>>(Arg.JsonString);

                                    questions = questions.Where(x => Ids.Contains(x.Id));
                                }

                                if (Arg.Key == "Status") // 狀態篩選條件
                                {
                                    var Status = JsonConvert
                                               .DeserializeObject<byte>(Arg.JsonString);

                                    questions = questions.Where(x => x.Status == Status);
                                }
                            }

                            var Data = questions
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
                            Res.TotalDataCount = questions.ToList().Count;

                            return Res;
                    }

                    public async Task<ApiResponse> Update(ApiRequest<List<UpdateQuestionArgs>> Req)
                    {
                            var Res = new ApiResponse();

                            var SussList = new List<int>();

                            foreach (var Arg in Req.Args)
                            {
                                var question = _CashFlowDbContext.Questions
                                         .FirstOrDefault(x => x.Id == Arg.Id);

                                if (question == null)
                                {
                                    Res.Success = false;
                                    Res.Code = (int)ResponseStatusCode.CannotFind;
                                    Res.Message += $@"Id：{Arg.Id} 無此Id\n";
                                }
                                else
                                {
                                    question.Id = Arg.Id;
question.Type = Arg.Type;
question.Name = Arg.Name;
question.Answer = Arg.Answer;
question.Status = Arg.Status;
                                    

                                    _CashFlowDbContext.SaveChanges();
                                    SussList.Add(question.Id);
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
                                var question = _CashFlowDbContext.Questions
                                         .FirstOrDefault(x => x.Id == Arg);

                                if (question == null)
                                {
                                    Res.Success = false;
                                    Res.Code = (int)ResponseStatusCode.CannotFind;
                                    Res.Message = "無此Id";
                                }
                                else
                                {
                                    _CashFlowDbContext.Questions.Remove(question);
                                    _CashFlowDbContext.SaveChanges();
                                    SussList.Add(question.Id);
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

