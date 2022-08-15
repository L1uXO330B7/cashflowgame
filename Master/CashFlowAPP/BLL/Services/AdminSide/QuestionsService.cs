using BLL.IServices;
using Common.Enum;
using Common.Model;
using Common.Model.AdminSide;
using DPL.EF;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using static Common.Model.AdminSide.QuestionsModel;

namespace BLL.Services.AdminSide
{
    public class QuestionsService : IQuestionsService<
            List<CreateUserArgs>,
            List<ReadQuestionArgs>,
            List<UpdateUserArgs>,
            List<int?>
        >
    {
        private readonly CashFlowDbContext _CashFlowDbContext;

        public QuestionsService(CashFlowDbContext cashFlowDbContext)
        {
            _CashFlowDbContext = cashFlowDbContext;
        }

        public Task<ApiResponse> Create(ApiRequest<List<CreateUserArgs>> Req)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse> Delete(ApiRequest<List<int?>> Req)
        {
            throw new NotImplementedException();
        }

        public async Task<ApiResponse> Read(ApiRequest<List<ReadQuestionArgs>> Req)
        {
            var Res = new ApiResponse();
            if (Req.Args.Count() <= 0)
            {
                Res.Success = true;
                Res.Code = (int)ResponseStatusCode.Success;
                Res.Message = "成功讀取";
                Res.Data = _CashFlowDbContext.Users.ToList();
            }
            else
            {
                // 查詢不追蹤釋放連線，避免其餘線程衝到
                var Questions = _CashFlowDbContext.Questions
                    .AsNoTracking();

                foreach (var Arg in Req.Args)
                {
                    if (Arg.Key == "Id") // Id 篩選條件
                    {
                        var Ids = JsonConvert
                            .DeserializeObject<List<int>>(Arg.JsonString);

                        Questions = Questions.Where(x => Ids.Contains(x.Id));
                    }

                    if (Arg.Key == "Status") // 狀態篩選條件
                    {
                        var Status = JsonConvert
                            .DeserializeObject<byte>(Arg.JsonString);

                        Questions = Questions.Where(x => x.Status == Status);
                    }
                }


                var Data = Questions
                    // 後端分頁
                    // 省略幾筆 ( 頁數 * 每頁幾筆 )
                    .Skip(((int)Req.PageIndex - 1) * (int)Req.PageSize)
                    // 取得幾筆
                    .Take((int)Req.PageSize)
                    .ToList();

                Res.Data = Data;
                Res.Success = true;
                Res.Code = (int)ResponseStatusCode.Success;
                Res.Message = "成功讀取";
            }
            return Res;
        }

        public Task<ApiResponse> Update(ApiRequest<List<UpdateUserArgs>> Req)
        {
            throw new NotImplementedException();
        }
    }
}
